using UnityEngine;

public class PlayerTilt : MonoBehaviour
{
    public Rigidbody playerRb; // Reference to the player's rigidbody
    public float tiltAngle = 20f;

    void Update()
    {
        // Tilt the child object based on the player's velocity
        Vector3 tilt = new Vector3(0, -playerRb.velocity.x, playerRb.velocity.y) * tiltAngle;
        transform.localRotation = Quaternion.Euler(tilt);
    }
}
