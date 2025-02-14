using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class SkullyMovement : MonoBehaviour
{
    private Vector2 currentXYMovement = Vector2.zero;
    [SerializeField] private float maxForwardSpeedFactor = 1f;
    [SerializeField] private bool isRunning = false;
    [SerializeField] private float currentZSpeed = 0f;
    [SerializeField] private float maxForwardSpeed = 50f;
    [SerializeField] private float zAcceleration = 1f;
    [SerializeField] private float xyMovementSpeed = 100f;
    [SerializeField] private float xyMovementAcceleration = 3f;
    [SerializeField] private float maxXYRotateAngle = 45f;
    [SerializeField] private Skully skully;

    [SerializeField] private CharacterController characterController;

    private bool xyControlEnabled = true;

    private Vector3 externalSpeed = Vector3.zero;

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

    public void SetMaxForwardSpeedFactor(float speedFactor)
    {
        maxForwardSpeedFactor = speedFactor;
    }

    public float GetMaxSpeedFactor()
    {
        return maxForwardSpeedFactor;
    }

    private Vector3 lastFramePos;

    void FixedUpdate()
    {
        lastFramePos = transform.position; 
        
        if (!isRunning) return;

        currentZSpeed = Time.deltaTime * maxForwardSpeed * maxForwardSpeedFactor;

        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        if (!xyControlEnabled)
        {
            xInput = yInput = 0;
        }

        if (skully.IsLookingBack)
        {
            xInput *= -1;
        }

        Vector2 desiredMovement = new Vector2(xInput, yInput);
        desiredMovement.Normalize();
        desiredMovement *= xyMovementSpeed;

        currentXYMovement = Vector2.MoveTowards(currentXYMovement, desiredMovement, Time.deltaTime * xyMovementAcceleration);

        Vector3 actualMovement = new Vector3(currentXYMovement.x, currentXYMovement.y, currentZSpeed);
        actualMovement += externalSpeed;
        characterController.Move(actualMovement);

        Vector2 desiredRotation = new Vector2(currentXYMovement.x / xyMovementSpeed * maxXYRotateAngle, currentXYMovement.y / xyMovementSpeed * maxXYRotateAngle);
        if (skully.IsLookingBack)
        {
            desiredRotation.x = -desiredRotation.x + 180;
        }

        transform.eulerAngles = new Vector3(-desiredRotation.y, desiredRotation.x, 0);
    }

    public Vector3 GetCurrentVelocity()
    {
        if (!isActiveAndEnabled || !isRunning)
        {
            return Vector3.zero;
        }

        if(skully.IsLookingBack)
        {
            return new Vector3(-currentXYMovement.x, currentXYMovement.y, currentZSpeed);
        }
        else
        {
            return new Vector3(currentXYMovement.x, currentXYMovement.y, currentZSpeed);
        }
    }

    public void EnableXYControl()
    {
        xyControlEnabled = true;
    }

    public void DisableXYControl()
    {
        xyControlEnabled = false;
    }

    public void AddExternalSpeed(Vector3 speed)
    {
        this.externalSpeed = speed;
    }

    public void StopExternalSpeed(float decay)
    {
        Vector3 originalSpeed = externalSpeed;

        DOVirtual.Float(0, 1, decay, (t) =>
        {
            Debug.Log("decay " + decay + " external speed " + externalSpeed);
            externalSpeed = Vector3.Lerp(originalSpeed, Vector3.zero, t);
        });
    }

    public float GetActualZSpeed()
    {
        return transform.position.z - lastFramePos.z;
    }

    public void SetMaxFowardSpeed(float speed)
    {
        maxForwardSpeed = speed;
    }
}
