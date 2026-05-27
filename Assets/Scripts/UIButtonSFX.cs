using UnityEngine;

public class UIButtonSFX : MonoBehaviour
{
    public AudioClip clickSFX;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayClickSound()
    {
        if (clickSFX != null)
        {
            audioSource.PlayOneShot(clickSFX);
        }
    }
}