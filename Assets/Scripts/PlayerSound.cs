using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip landingSfx;

    private bool hasPlayedLanding = true; // empieza true porque al inicio ya estas en el piso

    public void NotifyJumpStarted()
    {
        hasPlayedLanding = false;
    }

    public void PlayLandingSound()
    {
        // Si ya sonó para este salto, no hagas nada
        if (hasPlayedLanding)
            return;

        hasPlayedLanding = true;

        if (audioSource != null && landingSfx != null)
        {
            audioSource.PlayOneShot(landingSfx);
        }
    }
}