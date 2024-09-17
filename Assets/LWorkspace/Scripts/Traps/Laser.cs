using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private float maxLength = 50f;

    [SerializeField] private Transform rayOrigin;

    [SerializeField] private LayerMask layerMask;

    [SerializeField] private GameObject laserHit;

    [SerializeField] private int damage = 10;

    [SerializeField] private float hitInterval = 0.5f;
    private float lastHitTimestamp = 0f;

    private void Update()
    {
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);

        Vector3 dest = ray.origin;

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxLength, layerMask))
        {
            Debug.Log(hitInfo.collider.name + " : " + hitInfo.distance);
            dest = rayOrigin.position + rayOrigin.forward * hitInfo.distance;

            if (GameHelper.IsSkully(hitInfo.collider,out Skully skully))
            {
                if (IsReadyToHit())
                {
                    HitSkully(skully);
                }
            }

            laserHit.SetActive(true);
        }
        else
        {
            dest = rayOrigin.position + rayOrigin.forward * maxLength;
            laserHit.SetActive(false);
        }

        lineRenderer.SetPositions(new Vector3[2] { rayOrigin.position, dest });
        laserHit.transform.position = dest;
    }


    private bool IsReadyToHit()
    {
        return Time.time > lastHitTimestamp + hitInterval;
    }

    private void HitSkully(Skully skully)
    {
        skully.HitByLaser(damage);
        lastHitTimestamp = Time.time;
    }
}
