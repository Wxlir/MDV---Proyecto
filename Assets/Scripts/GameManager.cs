using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages game state and handles pause menu functionality.
/// This is a singleton that persists between scenes.
/// Attach this to an empty GameObject in the game scene (or create it programmatically).
/// </summary>
public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [Header("Scene Settings")]
    [Tooltip("The name of the menu scene")]
    [SerializeField] private string menuSceneName = "Main Menu";

    private string GetMenuSceneName()
    {
        // If menuSceneName is empty, use default
        if (string.IsNullOrEmpty(menuSceneName))
        {
            return "Main Menu";
        }
        return menuSceneName;
    }

    private void Awake()
    {
        // Singleton pattern: ensure only one instance exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If another instance already exists, destroy this one
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Ensure GameManager is initialized
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// Gets or creates the GameManager instance
    /// </summary>
    public static GameManager GetInstance()
    {
        if (instance == null)
        {
            // Create GameManager if it doesn't exist
            GameObject gmObject = new GameObject("GameManager");
            instance = gmObject.AddComponent<GameManager>();
            DontDestroyOnLoad(gmObject);
        }
        return instance;
    }

    private void Update()
    {
        // Only check for Escape key when we're in the game scene
        // Check if we're in the game scene by checking the current scene name
        string currentSceneName = SceneManager.GetActiveScene().name;
        string menuScene = GetMenuSceneName();
        
        if (currentSceneName == menuScene)
        {
            return; // Don't process Escape in menu scene
        }

        // Check for Escape key to open menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log($"Escape pressed! Current scene: {currentSceneName}, Menu scene: {menuScene}");
            OpenMenu();
        }
    }

    /// <summary>
    /// Opens the main menu from the game scene
    /// </summary>
    public void OpenMenu()
    {
        string menuScene = GetMenuSceneName();
        Debug.Log($"OpenMenu called. Attempting to load scene: {menuScene}");
        
        // Save player data before leaving the scene
        Player player = FindFirstObjectByType<Player>();
        if (player != null)
        {
            Vector3 playerPosition = player.transform.position;
            bool facingRight = player.GetFacingRight();
            
            // Store player data in PlayerPrefs so menu can access it
            PlayerPrefs.SetFloat("LastPlayerPosX", playerPosition.x);
            PlayerPrefs.SetFloat("LastPlayerPosY", playerPosition.y);
            PlayerPrefs.SetFloat("LastPlayerPosZ", playerPosition.z);
            PlayerPrefs.SetInt("LastPlayerFacingRight", facingRight ? 1 : 0);
            Debug.Log($"Player data saved: Position ({playerPosition.x}, {playerPosition.y}, {playerPosition.z}), FacingRight: {facingRight}");
        }
        else
        {
            Debug.LogWarning("Player not found when trying to save!");
        }

        // Set a flag to indicate we're coming from in-game
        PlayerPrefs.SetInt("FromGameScene", 1);
        PlayerPrefs.Save();
        
        Debug.Log($"Loading scene: {menuScene}");
        
        try
        {
            SceneManager.LoadScene(menuScene);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error loading scene '{menuScene}': {e.Message}");
            Debug.LogError("Available scenes in build:");
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                Debug.LogError($"  - {sceneName}");
            }
        }
    }
}

