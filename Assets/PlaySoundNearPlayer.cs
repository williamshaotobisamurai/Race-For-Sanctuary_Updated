using DG.Tweening;
using UnityEngine;

public class PlaySoundNearPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("other name " + other.name);

        Skully skully = other.GetComponent<Skully>();

        // Check if the collider belongs to the player by name
        if (skully!= null && !audioSource.isPlaying)
        {
            audioSource.Play();

            DOVirtual.DelayedCall(3f, () => audioSource.Stop());
        }
    }
}
