using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityTrap : MonoBehaviour
{
    [SerializeField] private Transform pullDirection;
    [SerializeField] private float force;
    [SerializeField] private float decay;


    private void OnTriggerEnter(Collider other)
    {
        if (GameHelper.IsSkully(other, out Skully skully))
        {
            skully.AddExternalSpeed(pullDirection.forward * force,decay);
        }
    }
}
