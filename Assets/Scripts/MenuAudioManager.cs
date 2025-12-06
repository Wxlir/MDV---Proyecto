using UnityEngine;

/// <summary>
/// Manages background music for the menu scene.
/// Attach this to an empty GameObject in your menu scene.
/// </summary>
public class MenuAudioManager : MonoBehaviour
{
    [Header("Background Music")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] [Range(0f, 1f)] private float musicVolume = 0.5f;

    private AudioSource audioSource;

    private void Awake()
    {
        // Get or add AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Start()
    {
        SetupBackgroundMusic();
    }

    private void SetupBackgroundMusic()
    {
        if (backgroundMusic == null)
        {
            Debug.LogWarning("Background music clip is not assigned in MenuAudioManager!");
            return;
        }

        audioSource.clip = backgroundMusic;
        audioSource.volume = musicVolume;
        audioSource.loop = true; // Loop the music
        audioSource.playOnAwake = true;
        audioSource.Play();
    }

    /// <summary>
    /// Call this to change the volume during runtime
    /// </summary>
    public void SetVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        audioSource.volume = musicVolume;
    }
}
