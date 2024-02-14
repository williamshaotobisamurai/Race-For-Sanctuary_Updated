using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : EnemyBulletBase
{
    [SerializeField] private float coverScreenDuration;
    public float CoverScreenDuration => coverScreenDuration;
}
