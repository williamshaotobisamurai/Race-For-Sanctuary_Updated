using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplatterManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    private Tween hideTween;

    [SerializeField] private List<Image> splatterList;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Show(3f);
        }
    }

    public void Show(float duration)
    {
        if (hideTween != null)
        {
            hideTween.Kill();
            hideTween = null;
        }

        splatterList.ForEach(s => { s.GetComponent<RectTransform>().DOLocalMoveY(0f, 0f); });

        canvasGroup.transform.localScale = Vector3.one * 0.75f;
        canvasGroup.alpha = 0f;
        canvasGroup.transform.DOScale(Vector3.one, 0.2f);
        canvasGroup.DOFade(1f, 0.2f);

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(duration);      
        seq.Append(canvasGroup.DOFade(0f, Random.Range(1f, 2f)));
        splatterList.ForEach(s =>
        {
            seq.Join(s.GetComponent<RectTransform>().DOLocalMoveY(Random.Range(-50f, -150f), Random.Range(1f, 2f)));
        });
        seq.Play();

        hideTween = seq; 
    }
}
