using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStationWall : MonoBehaviour
{ 
    private void OnTriggerEnter(Collider other)
    {
        Skully skully = other.GetComponent<Skully>();
        if (skully != null)
        {
           // skully.SkullyHitSpaceStationWall();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("111111 " + collision.gameObject.name);
    }
}
