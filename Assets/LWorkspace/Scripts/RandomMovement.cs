using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RandomMovement : MonoBehaviour
{
    [SerializeField] private float range = 1.5f;
    [Range(0.5f, 10)]
    [SerializeField] private float speed = 0.5f;

    [SerializeField] private Vector3 originPos;
    [SerializeField] private Vector3 targetPos;

    private Coroutine movingCoroutine;
    private Tween movingTween;

    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.localPosition;
        DOVirtual.DelayedCall(Random.Range(0, 2f), () =>
        {
            StartMoving();
        });
    }

    private IEnumerator MoveRandomlyCoroutine()
    {
        while (true)
        {     
            targetPos = originPos + Random.onUnitSphere * range * Random.Range(1,1.5f);
            movingTween = transform.DOLocalMove(targetPos, 1f / speed).SetEase(Ease.InOutSine);
            yield return new WaitForSeconds(1f / speed + 0.2f);
        }
    }

    public void StartMoving()
    {
        StopMoving();
        movingCoroutine = StartCoroutine(MoveRandomlyCoroutine());
    }

    public void StopMoving()
    {
        if (movingTween != null)
        {
            movingTween.Kill();
            StopCoroutine(movingCoroutine);
            movingTween = null;
        }
    }
}
