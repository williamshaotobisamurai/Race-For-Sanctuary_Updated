using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robot : MonoBehaviour
{
    public Transform parent; // Assign the parent object in the Inspector
    public float rotationSpeed = 80.0f; // Adjust rotation speed as needed
    public float circleRadius = 2.0f; // Adjust the radius of the circle

    private Vector3 axis;

    void Start()
    {
        if (parent != null)
        {
            // Calculate the initial position of the child object relative to the parent
            axis = transform.position - parent.position;
        }
    }

    void Update()
    {
        if (parent != null)
        {
            // Update the position of the child object in a tighter circle around the parent
            Vector3 desiredPosition = parent.position + Quaternion.AngleAxis(rotationSpeed * Time.time, Vector3.up) * axis.normalized * circleRadius;
            transform.position = desiredPosition;
        }
    }
}
