using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienCreature : EnemyBase
{
    [SerializeField] private Animator animator;

    protected override GameObject Shoot()
    {
        animator.Play("Fly Fireball Shoot");
        DOVirtual.DelayedCall(0.6f, () =>
        {
            GameObject blobInstance = GameObject.Instantiate(bullet);
            blobInstance.transform.position = muzzle.position;
            blobInstance.transform.rotation = muzzle.rotation;
            blobInstance.transform.LookAt(GameManager.Instance.Skully.transform);
        });
        return null;
    }
}
