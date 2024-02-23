using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienCreature : EnemyBase
{
    [SerializeField] private Animator animator;
    [SerializeField] private string initAnimation;
    [SerializeField] private string attackAnimation;

    [SerializeField] private Obstacle standingObstacle;

    private void Start()
    {
        animator.Play(initAnimation);
        if ( standingObstacle != null ) 
        {
            standingObstacle.OnMissileHitEvent += StandingObstacle_OnMissileHitEvent;
        }
    }

    private void StandingObstacle_OnMissileHitEvent(Obstacle obstacle)
    {
        Kill();
    }

    protected override GameObject Shoot()
    {
        animator.Play(attackAnimation);
        DOVirtual.DelayedCall(0.2f, () =>
        {
            GameObject blobInstance = GameObject.Instantiate(bullet);
            blobInstance.transform.position = muzzle.position;
            blobInstance.transform.rotation = muzzle.rotation;
            blobInstance.transform.LookAt(GameManager.Instance.Skully.transform);
        });
        return null;
    }
}
