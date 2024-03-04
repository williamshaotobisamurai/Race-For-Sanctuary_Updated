using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : EnemyBase
{
    [SerializeField] private List<MeshRenderer> meshRendererlist;
    [SerializeField] private bool redFirst = false;
    [SerializeField] private float interval = 0.2f;

    private void Start()
    {
        meshRendererlist = new List<MeshRenderer>(GetComponentsInChildren<MeshRenderer>());
        StartCoroutine(StartFlashing());
    }

    private IEnumerator StartFlashing()
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
        meshRendererlist.ForEach(t => t.material.SetColor("_EmissionColor", firstColor)));
        seq.AppendInterval(interval);
        seq.AppendCallback(() =>
        meshRendererlist.ForEach(t => t.material.SetColor("_EmissionColor", secondColor)));
        seq.AppendInterval(interval);
        seq.Play();
        seq.SetLoops(-1);
    }

    Tween shakeTween = null;

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (shakeTween != null) 
        {
            shakeTween.Kill();
            shakeTween = null;
        }
        shakeTween =  transform.DOShakeRotation(0.2f, 10);
    }

}
