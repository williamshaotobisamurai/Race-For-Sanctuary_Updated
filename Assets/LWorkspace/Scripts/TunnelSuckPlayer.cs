using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelSuckPlayer : MonoBehaviour
{
    [SerializeField] private float changeForceDirectionInterval;

    private float lastChangeDirectionTimeStamp = 0;

    [SerializeField] private float force = 200;

    private Vector3 currentForce;

    private void OnTriggerStay(Collider other)
    {
        Skully skully = other.GetComponent<Skully>();      

        if (skully != null )
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
