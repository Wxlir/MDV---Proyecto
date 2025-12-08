using UnityEngine;

/// <summary>
/// Enemigo estático que dispara proyectiles al jugador
/// </summary>
public class NPC : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player; // Referencia al jugador
    
    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab; // El prefab del proyectil
    [SerializeField] private Transform firePoint; // Desde dónde dispara (opcional)
    [SerializeField] private float fireRate = 2f; // Tiempo entre disparos (segundos)
    [SerializeField] private float detectionRange = 15f; // Distancia máxima para disparar
    
    [Header("Aim Settings")]
    [SerializeField] private bool predictPlayerMovement = false; // Predecir movimiento del jugador
    [SerializeField] private float projectileSpeed = 10f; // Velocidad del proyectil (para predicción)
    
    private float nextFireTime;

    private void Start()
    {
        // Si no hay firePoint, usar la posición del NPC
        if (firePoint == null)
        {
            firePoint = transform;
        }
        
        // Buscar al jugador automáticamente si no está asignado
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogWarning("NPC: No se encontró al jugador. Asegúrate de que tenga el tag 'Player'");
            }
        }
    }

    private void Update()
    {
        if (player == null || projectilePrefab == null)
            return;

        // Verificar si el jugador está en rango
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= detectionRange)
        {
            // Disparar si es tiempo
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    private void Shoot()
    {
        // Crear el proyectil
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        
        // Calcular dirección hacia el jugador
        Vector2 direction = (player.position - firePoint.position).normalized;
        
        // Si queremos predecir el movimiento del jugador
        if (predictPlayerMovement)
        {
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // Predicción simple basada en la velocidad actual del jugador
                float distance = Vector2.Distance(firePoint.position, player.position);
                float timeToReach = distance / projectileSpeed;
                Vector2 predictedPosition = (Vector2)player.position + playerRb.linearVelocity * timeToReach;
                direction = (predictedPosition - (Vector2)firePoint.position).normalized;
            }
        }
        
        // Configurar el proyectil
        Projectile proj = projectile.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.SetDirection(direction);
        }
        
        Debug.Log("NPC dispara al jugador!");
    }

    // Visualizar el rango de detección en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // Dibujar línea hacia el jugador si está asignado
        if (player != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }
}

