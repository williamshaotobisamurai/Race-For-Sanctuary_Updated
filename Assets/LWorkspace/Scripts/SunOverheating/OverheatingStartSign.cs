using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OverheatingStartSign : MonoBehaviour
{
    [SerializeField] private Light m_light;
    [SerializeField] private TMP_Text text;

    private void OnTriggerEnter(Collider other)
    {
        Skully skully = other.GetComponent<Skully>();
        if (skully != null)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(m_light.DOIntensity(1.2f, 0.5f));
            seq.Append(text.DOFade(1f, 0.5f));
            seq.AppendInterval(1f);
            seq.Append(text.DOFade(0, 0.5f));
            seq.AppendCallback(() => skully.StartOverheating());
            seq.Play();
        }
    }
}
 