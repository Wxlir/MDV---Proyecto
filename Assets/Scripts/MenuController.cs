using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [Tooltip("Drag the Options button here")]
    [SerializeField] private Button optionsButton;

    [Tooltip("Drag the Exit button here")]
    [SerializeField] private Button exitButton;

    [Header("Scene Settings")]
    [Tooltip("The name of the game scene to load when Play is clicked")]
    [SerializeField] private string gameSceneName = "SampleScene";

    private void Start()
    {
        // Automatically connect buttons to their functions
        if (playButton != null)
            playButton.onClick.AddListener(PlayGame);
        else
            Debug.LogWarning("Play button is not assigned in MenuController!");

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
    /// Call this from the Play button's OnClick event
    /// </summary>
    public void PlayGame()
    {
        Debug.Log($"Loading game scene: {gameSceneName}");
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
