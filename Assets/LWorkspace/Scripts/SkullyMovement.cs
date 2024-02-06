using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class SkullyMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    [SerializeField] private float forwardForce = 2000f; // Force to move forward
    [SerializeField] private float sidewaysForce = 500f; // Force to move sideways
    [SerializeField] private float upwardForce = 500f;   // Force to move upwards
    [SerializeField] private float downwardForce = 500f; // Force to move downwards
    [SerializeField] private float maxSpeed = 200;

    [SerializeField] protected float RotateAngle = 200.0f;
    [SerializeField] private float maxForwardSpeedFactor = 1f;
    [SerializeField] protected float RotationRecoverRate = 2.0f;
    [SerializeField] private bool isRunning = false;

    public void Init()
    {
        //isRunning = false;
        maxForwardSpeedFactor = 1f;
    }

    public void StartRunning()
    {
        isRunning = true;
    }

    public void StopRunning()
    {
        isRunning = false;
    }

    public void SpeedBoost(SpeedBoost speedBoost)
    {
        maxForwardSpeedFactor = speedBoost.GetSpeedBoostFactor();
        rb.AddForce(transform.forward * forwardForce * Time.deltaTime * 10, ForceMode.VelocityChange);
        DOVirtual.DelayedCall(speedBoost.GetSpeedUpDuration(), () =>
        {
            maxForwardSpeedFactor = 1f;
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, maxSpeed);
        });
    }

    void FixedUpdate()
    {
        if (!isRunning) return;

        // Constantly apply a forward force,until speed reaches MaxSpeed(Editable)
        if (rb.velocity.z <= maxSpeed * maxForwardSpeedFactor)
        {
            rb.AddForce(Vector3.forward * forwardForce * Time.deltaTime * maxForwardSpeedFactor, ForceMode.Force);
        }

        // Apply sideways force based on user input
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);

            if (getEditorAngle(rb.transform.localRotation.eulerAngles.y) <= RotateAngle)
            {
                //Debug.Log($"rotate right");
                rb.transform.Rotate(new Vector3(0.0f, RotateAngle, 0.0f));
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);

            if (getEditorAngle(rb.transform.localRotation.eulerAngles.y) >= -RotateAngle)
            {
                //Debug.Log($"rotate left");
                rb.transform.Rotate(new Vector3(0.0f, -RotateAngle, 0.0f));
            }
        }

        // Add upward force when the "w" key is pressed
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(0, upwardForce * Time.deltaTime, 0, ForceMode.VelocityChange);

            if (getEditorAngle(rb.transform.localRotation.eulerAngles.x) >= -RotateAngle)
            {
                rb.transform.Rotate(new Vector3(-RotateAngle, 0.0f, 0.0f));
            }
        }

        // Add downward force when the "s" key is pressed
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(0, -downwardForce * Time.deltaTime, 0, ForceMode.VelocityChange);

            if (getEditorAngle(rb.transform.localRotation.eulerAngles.x) <= RotateAngle + 20)
            {
                rb.transform.Rotate(new Vector3(RotateAngle, 0.0f, 0.0f));
            }
        }

        if (!Input.anyKey)
        {
            //gradually return orientation
            Vector3 currentAngle = new Vector3(rb.transform.eulerAngles.x, rb.transform.eulerAngles.y, rb.transform.eulerAngles.z);
   

            float xThisFrame = Mathf.LerpAngle(rb.transform.eulerAngles.x, 20f, Time.deltaTime * 10);
            float yThisFrame = Mathf.LerpAngle(rb.transform.eulerAngles.y, 0, Time.deltaTime * 10);
            float zThisFrame = Mathf.LerpAngle(rb.transform.eulerAngles.z, 0, Time.deltaTime * 10);



            Vector3 eulerThisFrame = new Vector3(xThisFrame, yThisFrame, zThisFrame);
            Debug.Log(currentAngle + " : " + eulerThisFrame);
            rb.transform.localEulerAngles = eulerThisFrame;
         //   rb.transform.Rotate((new Vector3(0.0f, 0.0f, 0.0f) - currentAngle) * RotationRecoverRate);
        }

        // Example check for falling below a certain level, can be modified or removed
        if (rb.position.y < -100f)
        {
            // Replace this with your game's end game logic
            Debug.Log("Game Over");
        }
    }

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
}
