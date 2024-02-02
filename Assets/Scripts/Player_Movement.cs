using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public Rigidbody rb;

    public float forwardForce = 2000f; // Force to move forward
    public float sidewaysForce = 500f; // Force to move sideways
    public float upwardForce = 500f;   // Force to move upwards
    public float downwardForce = 500f; // Force to move downwards
    [SerializeField] protected int MaxSpeed=200;
    void Start()
    {
        // Ensure the Rigidbody is not kinematic
        rb.isKinematic = false;

        // Optional: Freeze rotation if you don't want the player to rotate upon collisions
        rb.freezeRotation = false;
    }

    void FixedUpdate()
    {
        // Constantly apply a forward force,until speed reaches MaxSpeed(Editable)
        if (rb.velocity.z <=MaxSpeed){
            Debug.Log($"velocity is {rb.velocity.z}");
         rb.AddForce(transform.forward * forwardForce * Time.deltaTime, ForceMode.Force);
        }

        // Apply sideways force based on user input
        if (Input.GetKey("d"))
        {
            rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }

        if (Input.GetKey("a"))
        {
            rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }

        // Add upward force when the "w" key is pressed
        if (Input.GetKey("w"))
        {
            rb.AddForce(0, upwardForce * Time.deltaTime, 0, ForceMode.VelocityChange);
        }

        // Add downward force when the "s" key is pressed
        if (Input.GetKey("s"))
        {
            rb.AddForce(0, -downwardForce * Time.deltaTime, 0, ForceMode.VelocityChange);
        }

        // Example check for falling below a certain level, can be modified or removed
        if (rb.position.y < -100f)
        {
            // Replace this with your game's end game logic
            Debug.Log("Game Over");
        }
    }
}