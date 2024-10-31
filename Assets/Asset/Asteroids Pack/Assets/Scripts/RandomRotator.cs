using UnityEngine;
using System.Collections;

public class RandomRotator : MonoBehaviour
{
    [SerializeField]
    private float tumble;

    void Start()
    {
        return;
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
    }
}