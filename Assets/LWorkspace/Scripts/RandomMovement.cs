using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RandomMovement : MonoBehaviour
{
    [SerializeField] private float range = 1.5f;
    [Range(0.5f, 10)]
    [SerializeField] private float moveDuration = 1.5f;

    private Vector3 originPos;
    private Vector3 targetPos;

    private Coroutine movingCoroutine;
    private Tween movingTween;

    [SerializeField] private Ease ease;

    public float Range { get => range; set => range = value; }
    public float MoveDuration { get => moveDuration; set => moveDuration = value; }

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
        bool completed = true;
        while (completed)
        {
            completed = false;
            targetPos = originPos + Random.onUnitSphere * Range * Random.Range(1, 1.5f);
            movingTween = transform.DOLocalMove(targetPos, moveDuration).SetEase(ease).OnComplete(() => completed = true);
            while (completed == false)
            {
                yield return new WaitForEndOfFrame();
            }
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
