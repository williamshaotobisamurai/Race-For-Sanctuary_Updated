using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDebris : EnemyBulletBase
{
    private Vector3 targetPos;

    private bool isFlying = false;

    public void FlyToPosition(Vector3 pos)
    {
        this.targetPos = pos;
        isFlying = true;
    }

    protected override void Fly()
    {
        if (isFlying)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * Speed);
            transform.Rotate(Vector3.up, Time.deltaTime * Speed);
        }
    }

    public override void OnHitPlayer()
    {
        GameObject explode = Instantiate(hitParticle);
        explode.transform.position = transform.position;
        gameObject.SetActive(true);
    }

    public override void OnFlyOverSkully()
    {
        Destroy(gameObject);
    }
}
