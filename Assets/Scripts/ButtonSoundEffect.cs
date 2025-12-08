using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Plays a sound effect when a button is highlighted/hovered.
/// Attach this to each button GameObject that should play a sound.
/// </summary>
public class ButtonSoundEffect : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    [Header("Sound Effect")]
    [SerializeField] private AudioClip hoverSound;
    [SerializeField][Range(0f, 1f)] private float sfxVolume = 1f;

    private AudioSource audioSource;

    private void Awake()
    {
        // Get or add AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configure the AudioSource for SFX
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    /// <summary>
    /// This is called when the mouse enters the button area (hover)
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayHoverSound();
    }

    /// <summary>
    /// This is called when the button is selected with keyboard/gamepad navigation
    /// </summary>
    public void OnSelect(BaseEventData eventData)
    {
        PlayHoverSound();
    }

    /// <summary>
    /// Plays the button hover sound effect
    /// </summary>
    private void PlayHoverSound()
    {
        if (hoverSound == null)
        {
            Debug.LogWarning($"Hover sound is not assigned on {gameObject.name}!");
            return;
        }

        audioSource.PlayOneShot(hoverSound, sfxVolume);
    }
}
