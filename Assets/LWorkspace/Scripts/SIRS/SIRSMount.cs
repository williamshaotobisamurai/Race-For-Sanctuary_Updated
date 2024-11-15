using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SIRSMount : MonoBehaviour
{
    [SerializeField] private Transform attachPoint;
    public Transform AttachPoint { get => attachPoint; }

    [SerializeField] private float rotateSpeed = 10f;

    [SerializeField] private bool isSpinning = false;
    void Update()
    {
        if (isSpinning)
        {
            transform.Rotate(Vector3.up, Time.deltaTime * rotateSpeed);
            attachPoint.LookAt(Camera.main.transform);
        }
    }

    public void StartSpinning()
    { 
        isSpinning = true;
    }

    public void StopSpinning()
    {
        isSpinning = false;
    }
}
