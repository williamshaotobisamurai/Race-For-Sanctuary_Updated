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

    private bool xyControlEnabled = true;

    public void Init()
    {
        maxForwardSpeedFactor = 1f;
    }

    public void StartRunning()
    {
        isRunning = true;
    }

    public void StopRunning()
    {
        isRunning = false;
        currentXYMovement = Vector2.zero; 
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

        currentZSpeed = Time.deltaTime * maxForwardSpeed * maxForwardSpeedFactor;

        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        if (!xyControlEnabled)
        {
            xInput = yInput = 0;
        }

        Vector2 desiredMovement = new Vector2(xInput, yInput);
        desiredMovement.Normalize();
        desiredMovement *= xyMovementSpeed;

        currentXYMovement = Vector2.MoveTowards(currentXYMovement, desiredMovement, Time.deltaTime * xyMovementAcceleration);

        characterController.Move(new Vector3(currentXYMovement.x, currentXYMovement.y, currentZSpeed));
        Vector2 desiredRotation = new Vector2(currentXYMovement.x / xyMovementSpeed * maxXYRotateAngle, currentXYMovement.y / xyMovementSpeed * maxXYRotateAngle);

        transform.eulerAngles = new Vector3(-desiredRotation.y, desiredRotation.x, 0);
    }

    public Vector3 GetCurrentVelocity()
    {
        if (!isActiveAndEnabled)
        {
            return Vector3.zero;
        }
        return new Vector3(currentXYMovement.x, currentXYMovement.y, currentZSpeed);
    }

    public void EnableXYControl()
    {
        xyControlEnabled = true;
    }

    public void DisableXYControl()
    {
        xyControlEnabled = false;
    }
}
