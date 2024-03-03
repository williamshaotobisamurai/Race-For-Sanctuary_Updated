using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class SkullyMovement : MonoBehaviour
{
    [SerializeField] private float maxForwardSpeedFactor = 1f;
    [SerializeField] private bool isRunning = false;
    [SerializeField] private float currentZSpeed = 0f;
    [SerializeField] private float maxForwardSpeed = 50f;
    [SerializeField] private float zAcceleration = 1f;
    [SerializeField] private float xyMovementSpeed = 100f;
    [SerializeField] private float xyMovementAcceleration = 3f;

    [SerializeField] private float maxXYRotateAngle = 45f;

    private Vector2 currentXYMovement = Vector2.zero;

    [SerializeField] private CharacterController characterController;

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
        DOVirtual.DelayedCall(speedBoost.GetSpeedUpDuration(), () =>
        {
            maxForwardSpeedFactor = 1f;
        });
    }

    public void SetMaxSpeedFactor(float speedFactor)
    {
        maxForwardSpeedFactor = speedFactor;
    }

    void FixedUpdate()
    {
        if (!isRunning) return;

        float zSpeed = Time.deltaTime * maxForwardSpeed * maxForwardSpeedFactor;
        
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        Vector2 desiredMovement = new Vector2(xInput, yInput);
        desiredMovement.Normalize();
        desiredMovement *= xyMovementSpeed;

        currentXYMovement = Vector2.MoveTowards(currentXYMovement, desiredMovement, Time.deltaTime * xyMovementAcceleration);

        characterController.Move(new Vector3(currentXYMovement.x, currentXYMovement.y, zSpeed));
        Vector2 desiredRotation = new Vector2(currentXYMovement.x / xyMovementSpeed * maxXYRotateAngle, currentXYMovement.y / xyMovementSpeed * maxXYRotateAngle);

        transform.eulerAngles = new Vector3(-desiredRotation.y, desiredRotation.x, 0);
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
