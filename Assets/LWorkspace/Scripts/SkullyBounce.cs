using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullyBounce : MonoBehaviour
{
    [SerializeField] private float bounceStrength = 2f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
    }

    public void Bounce(Vector3 currentDirection)
    {
        transform.DOMove(bounceStrength * currentDirection, 2f);
    }
}
