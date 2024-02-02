using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullyMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    [SerializeField] private float forwardForce = 2000f; // Force to move forward
    [SerializeField] private float sidewaysForce = 500f; // Force to move sideways
    [SerializeField] private float upwardForce = 500f;   // Force to move upwards
    [SerializeField] private float downwardForce = 500f; // Force to move downwards
    [SerializeField] private int MaxSpeed = 200;

    [SerializeField] private float maxForwardSpeedFactor = 1f;

    [SerializeField] private bool isRunning = false;

    public void StartRunning()
    {
        isRunning = true;
    }

    public void StopRunning()
    {
        isRunning = false;
    }

    void FixedUpdate()
    {
        if (!isRunning) return;

        // Constantly apply a forward force,until speed reaches MaxSpeed(Editable)
        if (rb.velocity.z <= MaxSpeed)
        {
            rb.AddForce(transform.forward * forwardForce * Time.deltaTime * maxForwardSpeedFactor, ForceMode.Force);
        }

        // Apply sideways force based on user input
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }

        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }

        // Add upward force when the "w" key is pressed
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(0, upwardForce * Time.deltaTime, 0, ForceMode.VelocityChange);
        }

        // Add downward force when the "s" key is pressed
        if (Input.GetKey(KeyCode.S))
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
