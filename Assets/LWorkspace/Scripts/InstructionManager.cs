using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InstructionManager : MonoBehaviour
{
    private static InstructionManager instance;
    [SerializeField] private CanvasGroup textCanvas;
    [SerializeField] private TMP_Text instructionText;
    private Tween showTextTween;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public static void ShowText(string text, float seconds = 3f,Action OnComplete = null)
    {
        if(instance.showTextTween != null) 
        {
            instance.showTextTween.Kill();
            instance.showTextTween = null;
        }

        instance.instructionText.text = text;
        Sequence seq = DOTween.Sequence();
        seq.Append(instance.textCanvas.DOFade(1f, 0.5f));
        seq.AppendInterval(seconds);
        seq.Append(instance.textCanvas.DOFade(0, 0.5f));
        seq.OnComplete(() => OnComplete?.Invoke());
        seq.Play();
        instance.showTextTween = seq;
    }
}
