using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    public Player_Movement movement;
    private AudioSource collisionSound;
    //public AudioClip collisionClip;
    //public float volume = 1000.0f;

    void Start() 
    {
        collisionSound = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Obstacle")
        {
            movement.enabled = false;
            collisionSound.Play();
           //AudioSource.PlayClipAtPoint(collisionClip, transform.position, volume);
            FindObjectOfType<GameManager>().EndGame();
        }
    }
}
