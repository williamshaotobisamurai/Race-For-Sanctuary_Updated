using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private float maxLength = 50f;

    [SerializeField] private Transform rayOrigin;

    [SerializeField] private LayerMask layerMask;

    [SerializeField] private int damage = 10;

   

    private void Update()
    {
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);

        Vector3 dest = ray.origin;

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxLength, layerMask))
        {
            dest = rayOrigin.position + rayOrigin.forward * hitInfo.distance;

          
        }
        else
        {
            dest = rayOrigin.position + rayOrigin.forward * maxLength;
        }

        lineRenderer.SetPositions(new Vector3[2] { rayOrigin.position, dest });
    }


}

