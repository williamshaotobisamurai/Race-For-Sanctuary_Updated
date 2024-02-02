using UnityEngine;

public class PlaySoundNearPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    public string playerName = "Stylized Astronaut"; // Replace with your player object's name

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player by name
        if (other.gameObject.name == playerName && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
