using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveAttachment : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> meshRenderers;

    private Tween tween;

    public void StartFlashingForSeconds(float seconds)
    {
        float phaseDuration = 0.3f;
        Sequence flash = DOTween.Sequence();
        flash.AppendCallback(() =>
        {
            meshRenderers.ForEach(t => t.material.DOFade(0.2f, phaseDuration));
        });
        flash.AppendInterval(phaseDuration);
        flash.AppendCallback(() =>
        {
            meshRenderers.ForEach(t => t.material.DOFade(1f, phaseDuration));
        });
        flash.SetLoops(20);
        flash.Play();
        tween = flash;
    }

    private void OnEnable()
    {
        StopFlashing();
    }

    private void OnDisable()
    {
        StopFlashing();
    }

    public void StopFlashing()
    {
        if (tween != null)
        {
            tween.Kill();
            tween = null;
        }
        meshRenderers.ForEach(t => t.material.DOFade(1f, 0));

    }
}
