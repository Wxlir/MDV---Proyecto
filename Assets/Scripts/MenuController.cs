using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles menu button functionality: Play, Options, and Exit.
/// Attach this to a Canvas or empty GameObject in your menu.
/// Then assign the buttons in the Inspector and it will automatically connect them!
/// </summary>
public class MenuController : MonoBehaviour
{
    [Header("Button References")]
    [Tooltip("Drag the Play button here")]
    [SerializeField] private Button playButton;

    [Tooltip("Drag the Continue button here")]
    [SerializeField] private Button continueButton;

    [Tooltip("Drag the Options button here")]
    [SerializeField] private Button optionsButton;

    [Tooltip("Drag the Exit button here")]
    [SerializeField] private Button exitButton;

    [Header("Scene Settings")]
    [Tooltip("The name of the game scene to load when Play is clicked")]
    [SerializeField] private string gameSceneName = "SampleScene";

    private bool isFromGameScene = false;

    private void Start()
    {
        // Check if we're coming from the game scene
        isFromGameScene = PlayerPrefs.GetInt("FromGameScene", 0) == 1;
        PlayerPrefs.SetInt("FromGameScene", 0); // Reset the flag

        // Configure menu based on context
        ConfigureMenuForContext();

        // Automatically connect buttons to their functions
        if (playButton != null)
            playButton.onClick.AddListener(PlayGame);
        else
            Debug.LogWarning("Play button is not assigned in MenuController!");

        if (continueButton != null)
            continueButton.onClick.AddListener(ContinueGame);
        else
            Debug.LogWarning("Continue button is not assigned in MenuController!");


        if (optionsButton != null)
            optionsButton.onClick.AddListener(OpenOptions);
        else
            Debug.LogWarning("Options button is not assigned in MenuController!");

        if (exitButton != null)
            exitButton.onClick.AddListener(ExitGame);
        else
            Debug.LogWarning("Exit button is not assigned in MenuController!");
    }

    /// <summary>
    /// Configures the menu UI based on whether we're in-game or at the start screen
    /// </summary>
    private void ConfigureMenuForContext()
    {
        bool saveExists = SaveSystem.SaveExists();

        if (isFromGameScene)
        {
            // Coming from game scene: Show "Salvar partida" and "Cargar partida"
            SetButtonText(playButton, "Salvar partida");
            SetButtonText(continueButton, "Cargar partida");
            
            // Change play button to save functionality
            if (playButton != null)
            {
                playButton.onClick.RemoveAllListeners();
                playButton.onClick.AddListener(SaveGame);
            }
            
            // Change continue button to load functionality when coming from game
            if (continueButton != null)
            {
                continueButton.onClick.RemoveAllListeners();
                continueButton.onClick.AddListener(LoadGameFromMenu);
                
                continueButton.interactable = saveExists;
                if (saveExists)
                {
                    SetButtonOpacity(continueButton, 1f);
                }
                else
                {
                    SetButtonOpacity(continueButton, 0.2f);
                }
            }
        }
        else
        {
            // At start screen: Show "Nuevo juego" and "Continuar"
            SetButtonText(playButton, "Nuevo juego");
            SetButtonText(continueButton, "Continuar");

            // Configure Continue button based on save existence
            if (continueButton != null)
            {
                if (saveExists)
                {
                    // Enable Continue button if save exists
                    continueButton.interactable = true;
                    SetButtonOpacity(continueButton, 1f);
                }
                else
                {
                    // Disable Continue button if no save exists
                    continueButton.interactable = false;
                    // Set opacity to 0.2 (20% opacity) for disabled state
                    SetButtonOpacity(continueButton, 0.2f);
                }
            }
        }
    }

    /// <summary>
    /// Sets the text of a button's TextMeshPro component
    /// </summary>
    private void SetButtonText(Button button, string text)
    {
        if (button == null) return;

        TMP_Text textComponent = button.GetComponentInChildren<TMP_Text>();
        if (textComponent != null)
        {
            textComponent.text = text;
        }
        else
        {
            Debug.LogWarning($"Could not find TMP_Text component in button: {button.name}");
        }
    }

    /// <summary>
    /// Sets the opacity of the entire button (both image and text)
    /// </summary>
    private void SetButtonOpacity(Button button, float opacity)
    {
        if (button == null) return;

        // Set opacity of the button's image
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            Color imageColor = buttonImage.color;
            imageColor.a = opacity;
            buttonImage.color = imageColor;
        }

        // Set opacity of the button's text
        TMP_Text textComponent = button.GetComponentInChildren<TMP_Text>();
        if (textComponent != null)
        {
            Color textColor = textComponent.color;
            textColor.a = opacity;
            textComponent.color = textColor;
        }
    }

    /// <summary>
    /// Call this from the Play button's OnClick event (or Save button when in-game)
    /// </summary>
    public void PlayGame()
    {
        // Clear any load flag
        PlayerPrefs.SetInt("LoadGame", 0);
        
        // Set flag for new game (will reset player to initial position)
        PlayerPrefs.SetInt("NewGame", 1);
        
        // Clear any saved position data so player starts fresh
        PlayerPrefs.DeleteKey("LastPlayerPosX");
        PlayerPrefs.DeleteKey("LastPlayerPosY");
        PlayerPrefs.DeleteKey("LastPlayerPosZ");
        PlayerPrefs.DeleteKey("LastPlayerFacingRight");
        
        PlayerPrefs.Save();

        Debug.Log($"Loading game scene: {gameSceneName} (New Game)");
        SceneManager.LoadScene(gameSceneName);
    }

    /// <summary>
    /// Saves the current game state using the player data stored when leaving the game scene
    /// After saving, returns to the game scene
    /// </summary>
    public void SaveGame()
    {
        Debug.Log("SaveGame called!");
        
        // Check if we have stored player data (stored when Escape was pressed)
        if (PlayerPrefs.HasKey("LastPlayerPosX"))
        {
            float posX = PlayerPrefs.GetFloat("LastPlayerPosX");
            float posY = PlayerPrefs.GetFloat("LastPlayerPosY");
            float posZ = PlayerPrefs.GetFloat("LastPlayerPosZ");
            bool facingRight = PlayerPrefs.GetInt("LastPlayerFacingRight", 1) == 1;
            
            Vector3 playerPosition = new Vector3(posX, posY, posZ);
            Debug.Log($"Saving game with position: ({posX}, {posY}, {posZ}), facingRight: {facingRight}");
            
            SaveSystem.SaveGame(playerPosition, facingRight);
            Debug.Log("Game saved successfully!");
            
            // Return to game scene after saving
            PlayerPrefs.SetInt("LoadGame", 0); // Don't load, just continue from where we were
            PlayerPrefs.Save();
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogWarning("No player data available to save. PlayerPrefs keys:");
            Debug.LogWarning($"Has LastPlayerPosX: {PlayerPrefs.HasKey("LastPlayerPosX")}");
            Debug.LogWarning("Please return to game and try again.");
        }
    }

    /// <summary>
    /// Loads the saved game (called from Continue button at start screen)
    /// </summary>
    public void ContinueGame()
    {
        if (!SaveSystem.SaveExists())
        {
            Debug.LogWarning("No save file exists!");
            return;
        }

        // Set flag to load game when scene loads
        PlayerPrefs.SetInt("LoadGame", 1);
        PlayerPrefs.Save();

        Debug.Log("Loading saved game");
        SceneManager.LoadScene(gameSceneName);
    }

    /// <summary>
    /// Loads the saved game and returns to game scene (called from "Cargar partida" when in-game menu)
    /// </summary>
    public void LoadGameFromMenu()
    {
        if (!SaveSystem.SaveExists())
        {
            Debug.LogWarning("No save file exists!");
            return;
        }

        // Set flag to load game when scene loads
        PlayerPrefs.SetInt("LoadGame", 1);
        PlayerPrefs.Save();

        Debug.Log("Loading saved game from menu");
        SceneManager.LoadScene(gameSceneName);
    }

    /// <summary>
    /// Call this from the Options button's OnClick event
    /// </summary>
    public void OpenOptions()
    {
        Debug.Log("Options menu opened (not implemented yet)");
        // You can implement this later by showing an options panel
        // For example:
        // optionsPanel.SetActive(true);
    }

    /// <summary>
    /// Call this from the Exit button's OnClick event
    /// </summary>
    public void ExitGame()
    {
        Debug.Log("Exiting game...");

#if UNITY_EDITOR
        // If running in the Unity Editor, stop play mode
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // If running as a build, quit the application
        Application.Quit();
#endif
    }
}
