using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidingMeteorite : MonoBehaviour
{
    public event OnHitByOtherMeteorite OnHitByOtherMeteoriteEvent;
    public delegate void OnHitByOtherMeteorite(CollidingMeteorite a, CollidingMeteorite b);


    private void OnCollisionEnter(Collision collision)
    {
        CollidingMeteorite otherMeteorite = collision.collider.GetComponent<CollidingMeteorite>();
        if (otherMeteorite != null)
        {
            OnHitByOtherMeteoriteEvent?.Invoke(this, otherMeteorite);
        }
    }
}
