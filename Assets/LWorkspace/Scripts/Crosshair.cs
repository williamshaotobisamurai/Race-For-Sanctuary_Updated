using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Canvas parentCanvas;
    [SerializeField] private Image hitIndicator;

    private void Start()
    {
        indicatorOriginalScale = hitIndicator.transform.localScale.x;
    }

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

    private float indicatorOriginalScale;
    private Tween zoomIndicatorTween;

    public void OnHitEnemy()
    {
        Debug.Log("hit enemy ");
        if (zoomIndicatorTween != null)
        {
            zoomIndicatorTween.Kill();
            zoomIndicatorTween = null;
        }

        hitIndicator.color = Color.red;
        Sequence seq = DOTween.Sequence();
        seq.Append(hitIndicator.transform.DOScale(indicatorOriginalScale * 1.5f, 0f));
        seq.Append(hitIndicator.transform.DOScale(indicatorOriginalScale, 0.1f));
        seq.Join(hitIndicator.DOColor(Color.white, 0.1f));
        seq.Play();
        zoomIndicatorTween = seq;        
    }
}
