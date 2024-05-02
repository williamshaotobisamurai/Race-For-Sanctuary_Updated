using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Skully skully;

    private Action<Coin> OnCompleteAct;

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Collider m_collider;

    private float duration = 1f;

    private float movingDuration = 0f;

    [SerializeField] private AudioSource coinAudio;

    public void MoveToSkully(Skully skully, Action<Coin> OnComplete)
    {
        this.skully = skully;
        this.OnCompleteAct = OnComplete;
        movingDuration = 0f;
        transform.SetParent(skully.transform);
        m_collider.enabled = false;
    }

    private void Update()
    {
        if (skully != null)
        {
            duration += Time.deltaTime;
            movingDuration += Time.deltaTime;
            float progress = movingDuration / duration;
            transform.position = Vector3.MoveTowards(transform.position, skully.GetPosition(), Time.deltaTime * 60f);

            bool isCollected = meshRenderer.enabled;
            if (Vector3.Distance(transform.position, skully.GetPosition()) <= 0.5f && isCollected)
            {
                meshRenderer.enabled = false;

                coinAudio.Play();

                OnCompleteAct?.Invoke(this);
                DOVirtual.DelayedCall(1f, () =>
                {
                    gameObject.SetActive(false);
                });
            }
        }
    }
}
