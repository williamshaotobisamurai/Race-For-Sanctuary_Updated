using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelSuckPlayer : MonoBehaviour
{
    [SerializeField] private float changeForceDirectionInterval;

    private float lastChangeDirectionTimeStamp = 0;

    [SerializeField] private float force = 200;

    private Vector3 currentForce;

    [SerializeField] private float speedFactor = 3f;

    private void OnTriggerEnter(Collider other)
    {
        Skully skully = other.GetComponent<Skully>();

        if (skully != null)
        {
            skully.SetMaxSpeedFactor(speedFactor);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Skully skully = other.GetComponent<Skully>();

        if (skully != null)
        {
            skully.SetMaxSpeedFactor(1);
            skully.Rigidbody.velocity = Vector3.zero;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Skully skully = other.GetComponent<Skully>();

        if (skully != null)
        {
            if (Time.time > lastChangeDirectionTimeStamp + changeForceDirectionInterval)
            {
                lastChangeDirectionTimeStamp = Time.time;
                Vector3 randomDirection = Random.onUnitSphere;
                randomDirection.z = 0;
                currentForce = randomDirection * force;
            }
            else
            {
                skully.Rigidbody.AddForce(currentForce, ForceMode.Acceleration);
            }
        }
    }
}
