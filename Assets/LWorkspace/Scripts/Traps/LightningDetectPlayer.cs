using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningDetectPlayer : MonoBehaviour
{
    [SerializeField] private LightningArea lightningArea;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Skully>() != null)
        {
            lightningArea.ShowHintParticle();
        }
    }
}
