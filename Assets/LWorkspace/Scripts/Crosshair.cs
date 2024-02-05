using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image insideImg;
    private Sequence insideSeq;

    public void Show()
    {
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero + Vector2.up * 100f;
        canvasGroup.DOFade(1, 0.1f);
    }

    public void Hide()
    {
        canvasGroup.DOFade(0, 0.1f);
    }
}
