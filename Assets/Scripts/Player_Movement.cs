using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public Rigidbody rb;
    public float forwardForce = 2000f;
    public float sidewaysForce = 500f;
    public float upwardForce = 500f;
    public float downwardForce = 500f;
    public float tiltAmount = 15f; // Adjust the tilt amount as needed
    private bool isTiltingW = false;
    private bool isTiltingS = false;

    void FixedUpdate()
    {
        // Ensure Rigidbody's rotation is not frozen
        rb.freezeRotation = false;

        // Apply forward force
        rb.AddForce(transform.forward * forwardForce * Time.deltaTime);

        // Apply sideways force based on user input
        float horizontalInput = Input.GetAxis("Horizontal");
        float horizontalForce = horizontalInput * sidewaysForce * Time.deltaTime;
        rb.AddForce(transform.right * horizontalForce, ForceMode.VelocityChange);

        // Tilt the player based on its velocity when "a" or "d" is pressed
        if (isTiltingW)
        {
            float tilt = Mathf.Clamp(rb.velocity.y, -1f, 1f) * tiltAmount;
            transform.localRotation = Quaternion.Euler(-tilt, 0, 0);
        }
        else if (isTiltingS)
        {
            float tilt = Mathf.Clamp(rb.velocity.y, -1f, 1f) * tiltAmount;
            transform.localRotation = Quaternion.Euler(tilt, 0, 0);
        }
        else
        {
            // Smoothly return to the original rotation when not tilting
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime);
        }
    }

    void Update()
    {
        // Detect if movement keys are pressed to enable tilt
        isTiltingW = Input.GetKey("w");
        isTiltingS = Input.GetKey("s");
    }
}
