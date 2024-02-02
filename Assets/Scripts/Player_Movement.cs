using System;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public Rigidbody rb;

    public float forwardForce = 2000f; // Force to move forward
    public float sidewaysForce = 500f; // Force to move sideways
    public float upwardForce = 500f;   // Force to move upwards
    public float downwardForce = 500f; // Force to move downwards
    [SerializeField] protected int MaxSpeed = 200;
    [SerializeField] protected float RotateAngle = 200.0f;

    [SerializeField] protected float RotationRecoverRate = 2.0f;


    /*
    *The angles displayed in the editor and the angle returned 
    by eulerAngles of Rotation is different:
    the one in the editor is from -180 to 180
    euler angle is 0 to 360
    This function transforms euler angle to editor angle which is
    much more intuitive
    */
    public static float getEditorAngle(float angle)
    {
        if (angle > 360.0f || angle < 0.0f)
        {
            Debug.Log("Error: Angle greater than 360");
            return angle;
        }
        else
        {
            if (angle >= 0.0f && angle <= 180.0f)
            {
                return angle;
            }
            else
            {
                return angle - 360.0f;
            }

        }
    }
    void Start()
    {
        // Ensure the Rigidbody is not kinematic
        rb.isKinematic = false;

        // Optional: Freeze rotation if you don't want the player to rotate upon collisions
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        // Constantly apply a forward force,until speed reaches MaxSpeed(Editable)
        if (rb.velocity.z <= MaxSpeed)
        {
            //Debug.Log($"velocity is {rb.velocity.z}");
            rb.AddForce(transform.forward * forwardForce * Time.deltaTime, ForceMode.Force);
        }

        // Apply sideways force based on user input
        if (Input.GetKey("a"))
        {
            rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            //Debug.Log($"current angle is {rb.transform.localRotation.eulerAngles.y}");
            if (getEditorAngle(rb.transform.localRotation.eulerAngles.y) >= -RotateAngle)
            {
                //Debug.Log($"rotate left");
                rb.transform.Rotate(new Vector3(0.0f, -RotateAngle, 0.0f));
            }

        }
        if (Input.GetKey("d"))
        {
            rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            //Debug.Log($"current angle is {rb.transform.localRotation.eulerAngles.y}");
            if (getEditorAngle(rb.transform.localRotation.eulerAngles.y) <= RotateAngle)
            {
                //Debug.Log($"rotate right");
                rb.transform.Rotate(new Vector3(0.0f, RotateAngle, 0.0f));
            }
        }

        // Add upward force when the "w" key is pressed
        if (Input.GetKey("w"))
        {
            rb.AddForce(0, upwardForce * Time.deltaTime, 0, ForceMode.VelocityChange);
            if (getEditorAngle(rb.transform.localRotation.eulerAngles.x) >= -RotateAngle)
            {
                rb.transform.Rotate(new Vector3(-RotateAngle, 0.0f, 0.0f));
            }
        }

        // Add downward force when the "s" key is pressed
        if (Input.GetKey("s"))
        {
            rb.AddForce(0, -downwardForce * Time.deltaTime, 0, ForceMode.VelocityChange);
            if (getEditorAngle(rb.transform.localRotation.eulerAngles.x) <= RotateAngle)
            {
                rb.transform.Rotate(new Vector3(RotateAngle, 0.0f, 0.0f));
            }
        }
        if (!Input.anyKey)
        {
            //gradually return orientation
            Vector3 currentAngle = new Vector3(rb.transform.localRotation.x, rb.transform.localRotation.y, rb.transform.localRotation.z);
            // returning to original orientation through interpolation
            rb.transform.Rotate((new Vector3(0.0f, 0.0f, 0.0f) - currentAngle) * RotationRecoverRate);
        }

        // Example check for falling below a certain level, can be modified or removed
        if (rb.position.y < -100f)
        {
            // Replace this with your game's end game logic
            Debug.Log("Game Over");
        }
    }
}