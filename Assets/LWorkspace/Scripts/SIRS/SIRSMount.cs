using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SIRSMount : MonoBehaviour
{
    [SerializeField] private Transform attachPoint;
    public Transform AttachPoint { get => attachPoint; }

    [SerializeField] private float rotateSpeed = 10f;
    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * rotateSpeed);
        attachPoint.LookAt(Camera.main.transform);
    }
}
