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
    [SerializeField] private float destoryDelayAfterFlyOver = 3f;

    void Start()
    {
        fireParticle.Play();
        smokeParticle.Play();
        explodeTrigger.SetActive(true);
    }

    public override void OnFlyOverSkully()
    {       
        explodeTrigger.SetActive(true);

        DOVirtual.DelayedCall(destoryDelayAfterFlyOver, () =>
        {
            Destroy(gameObject);
        });
    }
}
