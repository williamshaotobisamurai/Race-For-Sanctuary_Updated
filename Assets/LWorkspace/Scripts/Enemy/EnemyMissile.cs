using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissile : EnemyBulletBase
{
    [SerializeField] private GameObject explodeTrigger;
    [SerializeField] private ParticleSystem fireParticle;
    [SerializeField] private ParticleSystem smokeParticle;

    [SerializeField] private Transform muzzle;

    void Start()
    {
        fireParticle.Play();
        smokeParticle.Play();
        explodeTrigger.SetActive(true);
    }
}
