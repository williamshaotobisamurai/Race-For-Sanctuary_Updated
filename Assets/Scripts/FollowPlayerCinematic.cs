using UnityEngine;

public class FollowPlayerCinematic : MonoBehaviour
{
    public Transform player; // Assign your player's transform here in the inspector
    public Vector3 offset; // Set this in the inspector to control the relative position of the camera

    private bool isBehindPlayer = false;

    void Update()
    {
        if (player == null)
            return;

        // Check if the player has passed the camera
        if (!isBehindPlayer && player.position.z > transform.position.z)
        {
            isBehindPlayer = true;
        }

        if (isBehindPlayer)
        {
            // Position the camera behind the player
            Vector3 behindPosition = player.position - player.forward * offset.z + Vector3.up * offset.y;
            transform.position = behindPosition;

            // Make the camera face the player's back
            transform.rotation = Quaternion.LookRotation(player.forward);
        }
        else
        {
            // Before the camera gets behind the player, it can rotate to look at the player
            Vector3 frontPosition = player.position + player.forward * offset.z + Vector3.up * offset.y;
            transform.position = frontPosition;
            transform.LookAt(player);
        }
    }
}
