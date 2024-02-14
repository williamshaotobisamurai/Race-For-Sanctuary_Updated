using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileWarningUI : MonoBehaviour
{
    [SerializeField] private Image img;

    private Tween flashingTween;

    private bool isShowing = false;

    private void Update()
    {
        
    }

    public void Show()
    {
        if (isShowing) return;

        isShowing = true;
        if (flashingTween != null)
        {
            flashingTween.Kill();
            flashingTween = null;
        }

        Sequence seq = DOTween.Sequence();
        seq.Append( img.DOColor(new Color(1, 1, 1, 1), 0.5f));
        seq.AppendInterval(1f);
        seq.Append(img.DOColor(new Color(1, 1, 1, 0), 0.5f));
        seq.Play();
        seq.SetLoops(-1);
        seq.Play();

        flashingTween = seq;
    }

    public void Hide()
    {
        isShowing = false;
        img.DOColor(new Color(1, 1, 1, 0), 0.5f);
    }
}
