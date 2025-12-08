using UnityEngine;

/// <summary>
/// Proyectil que daña al jugador
/// </summary>
public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifetime = 5f; // Se destruye después de este tiempo

    private Vector2 direction;

    private void Start()
    {
        // Autodestruirse después de un tiempo
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Mover el proyectil
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    /// <summary>
    /// Establece la dirección del proyectil
    /// </summary>
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        
        // Rotar el proyectil para que apunte en la dirección correcta
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si toca al jugador
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamage(damage);
            Destroy(gameObject); // Destruir el proyectil
            return;
        }

        // Si toca una pared o suelo (opcional)
        if (collision.CompareTag("Ground") || collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
