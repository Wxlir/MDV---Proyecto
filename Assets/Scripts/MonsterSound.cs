using UnityEngine;
using System.Collections;

public class MonsterSound : MonoBehaviour
{
    public AudioSource audioSource;
    public float minDelay = 2f;
    public float maxDelay = 5f;

    private void Start()
    {
        StartCoroutine(PlayRandomGrowl());
    }

    private IEnumerator PlayRandomGrowl()
    {
        while (true)
        {
            // espera tiempo aleatorio
            float wait = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(wait);

            // reproduce el gruñido
            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
    }
}
