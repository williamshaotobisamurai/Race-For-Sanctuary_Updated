using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    [SerializeField] private Image image;

    private static FadeManager instance;
    public static FadeManager Instance { get => instance; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance.gameObject);
        }
    }

    public void Transition(Action OnBlack, Action OnComplete)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(image.DOFade(1, 0.75f));
        seq.AppendCallback(() =>
        {
            OnBlack?.Invoke();
        });
        seq.Append(image.DOFade(0f, 0.75f));
        seq.AppendCallback(() => OnComplete?.Invoke());
        seq.Play();
    }
}
