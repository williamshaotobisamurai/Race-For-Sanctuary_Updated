using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullyBulletBase : MonoBehaviour
{
    [SerializeField] private int damamge = 10;
    public int Damage { get => damamge; }

    [SerializeField] protected GameObject hitParticle;
    [SerializeField] protected float speed = 10f;
}
