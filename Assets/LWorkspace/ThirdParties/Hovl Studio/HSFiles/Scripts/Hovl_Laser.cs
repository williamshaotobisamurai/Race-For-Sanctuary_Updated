using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Burst.CompilerServices;

public class Hovl_Laser : MonoBehaviour
{
    public int damageOverTime = 30;

    public GameObject HitEffect;
    public float HitOffset = 0;
    public bool useLaserRotation = false;

    public float MaxLength;

    public float MainTextureLength = 1f;
    public float NoiseTextureLength = 1f;

    [SerializeField] private LayerMask layerMask;
 
    public void DisablePrepare()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameHelper.IsSkully(other, out Skully skully))
        {
            if (IsReadyToHit())
            {
                HitSkully(skully);
            }
        }
    }

    private float lastHitTimestamp = 0f;
    [SerializeField] private float hitInterval = 0.5f;

    private void HitSkully(Skully skully)
    {
        skully.HitByLaser(damageOverTime);
        lastHitTimestamp = Time.time;
    }

    private bool IsReadyToHit()
    {
        return Time.time > lastHitTimestamp + hitInterval;
    }
}
