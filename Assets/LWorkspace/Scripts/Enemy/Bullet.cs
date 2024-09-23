using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : EnemyBulletBase
{
    public override void OnFlyOverSkully()
    {
        Debug.Log("fly over skully " + gameObject.name);
        Destroy(gameObject);
    }
}
