using DG.Tweening;
using UnityEngine;

public class PlaySoundNearPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    public string playerName = "Skully"; // Replace with your player object's name

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("other name " + other.name);
        // Check if the collider belongs to the player by name
        if (other.gameObject.name == playerName && !audioSource.isPlaying)
        {
            audioSource.Play();

            DOVirtual.DelayedCall(3f, () => audioSource.Stop());
        }
    }
}
