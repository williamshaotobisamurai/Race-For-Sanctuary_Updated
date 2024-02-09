using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningHitArea : MonoBehaviour
{
    [SerializeField] private LightningArea lightningArea;

    private void OnTriggerEnter(Collider other)
    {
        Skully skully = other.GetComponent<Skully>();
        if (skully != null)
        {
            lightningArea.HitSkully(skully);
        }
    }
}
