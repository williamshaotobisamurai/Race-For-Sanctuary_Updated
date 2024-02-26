using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashMat : MonoBehaviour
{
    [SerializeField] private MeshRenderer m_meshRenderer;
    [SerializeField] private float interval;

    [SerializeField] private bool redFirst = false;

    IEnumerator Start()
    {
        float randomDelay = Random.Range(0, 1f);
        yield return new WaitForSeconds(randomDelay);

        Color firstColor = Color.blue;
        Color secondColor = Color.red;

        if (redFirst)
        {
            firstColor = Color.red;
            secondColor = Color.blue;
        }


        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() =>
        m_meshRenderer.material.SetColor("_EmissionColor", firstColor));
        seq.AppendInterval(interval);
        seq.AppendCallback(() =>
        m_meshRenderer.material.SetColor("_EmissionColor", secondColor));
        seq.AppendInterval(interval);
        seq.Play();
        seq.SetLoops(-1);
    }
}
