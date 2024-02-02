using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Skully skully;
    private Action<Coin> OnCompleteAct;

   private float duration = 1f;

    private float movingDuration = 0f;

    public void MoveToSkully(Skully skully, Action<Coin> OnComplete)
    {
        this.skully = skully;
        this.OnCompleteAct = OnComplete;
        movingDuration = 0f;
        transform.SetParent(skully.transform);
    }

    private void Update()
    {
        if (skully != null)
        {
            duration += Time.deltaTime;
            movingDuration += Time.deltaTime;
            float progress = movingDuration / duration;
            transform.position =  Vector3.MoveTowards(transform.position, skully.GetPosition(), Time.deltaTime * 60f);

            if (Vector3.Distance(transform.position,skully.GetPosition()) <= 0.5f)
            {
                gameObject.SetActive(false);
                OnCompleteAct?.Invoke(this);
            }
        }
    }
}
