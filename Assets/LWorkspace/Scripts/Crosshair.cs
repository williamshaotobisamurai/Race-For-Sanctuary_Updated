using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image insideImg;
    [SerializeField] private Canvas parentCanvas;

    private Sequence insideSeq;

    public void Show()
    {
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero + Vector2.up * 100f;
        canvasGroup.DOFade(1, 0.1f);
    }

    private void Update()
    {
        Vector2 movePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas.transform as RectTransform,
                Input.mousePosition, parentCanvas.worldCamera,
                out movePos);
        transform.position = parentCanvas.transform.TransformPoint(movePos);
    }

    public void Hide()
    {
        canvasGroup.DOFade(0, 0.1f);
    }
}
