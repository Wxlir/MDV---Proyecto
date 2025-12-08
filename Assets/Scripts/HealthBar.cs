using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Maneja la barra de vida del jugador en la UI
/// </summary>
public class HealthBar : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image fillImage; // La imagen que se llena/vacía
    [SerializeField] private Gradient colorGradient; // Colores según la vida (verde->amarillo->rojo)
    
    [Header("Settings")]
    [SerializeField] private bool useColorGradient = true;
    [SerializeField] private float smoothSpeed = 5f; // Velocidad de la animación

    private float targetFillAmount;
    private float currentFillAmount;

    private void Start()
    {
        // Inicializar llena
        targetFillAmount = 1f;
        currentFillAmount = 1f;
        
        if (fillImage != null)
        {
            fillImage.fillAmount = 1f;
        }
    }

    private void Update()
    {
        // Animar suavemente la barra
        if (fillImage != null && currentFillAmount != targetFillAmount)
        {
            currentFillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, Time.deltaTime * smoothSpeed);
            fillImage.fillAmount = currentFillAmount;

            // Cambiar color según la vida
            if (useColorGradient && colorGradient != null)
            {
                fillImage.color = colorGradient.Evaluate(currentFillAmount);
            }
        }
    }

    /// <summary>
    /// Actualiza la barra de vida
    /// </summary>
    /// <param name="currentHealth">Vida actual</param>
    /// <param name="maxHealth">Vida máxima</param>
    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        if (maxHealth <= 0)
            return;

        targetFillAmount = (float)currentHealth / maxHealth;
    }

    /// <summary>
    /// Establece la barra instantáneamente (sin animación)
    /// </summary>
    public void SetHealthInstant(int currentHealth, int maxHealth)
    {
        if (maxHealth <= 0)
            return;

        targetFillAmount = (float)currentHealth / maxHealth;
        currentFillAmount = targetFillAmount;
        
        if (fillImage != null)
        {
            fillImage.fillAmount = currentFillAmount;
            
            if (useColorGradient && colorGradient != null)
            {
                fillImage.color = colorGradient.Evaluate(currentFillAmount);
            }
        }
    }
}
