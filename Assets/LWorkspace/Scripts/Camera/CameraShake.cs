using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private Skully skully;
    private void Start()
    {
        skully.OnRequestCameraShake += Skully_OnRequestCameraShake;
    }

    private void OnDestroy()
    {
        skully.OnRequestCameraShake -= Skully_OnRequestCameraShake;
    }


    private bool isShaking = false;

    private void Skully_OnRequestCameraShake()
    {
        if (isShaking) { return; }
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOShakePosition(1f, 1f, 50));
        seq.Join(transform.DOShakeRotation(1f, 1f, 50));
        seq.OnComplete(() => 
        {
            isShaking = false;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        });
        seq.Play();
    }
}
