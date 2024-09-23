using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullyBounce : MonoBehaviour
{
    [SerializeField] private float bounceStrength = 2f;  

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
    }

    public void Bounce(Vector3 currentDirection)
    {
        transform.DOMove(transform.position +  bounceStrength * currentDirection, 2f);
    }
}
