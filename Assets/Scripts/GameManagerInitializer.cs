using UnityEngine;

/// <summary>
/// Ensures GameManager is created when the game scene loads.
/// Attach this to any GameObject in the game scene, or it will auto-create.
/// </summary>
public class GameManagerInitializer : MonoBehaviour
{
    private void Awake()
    {
        // Ensure GameManager exists
        GameManager.GetInstance();
    }
}

