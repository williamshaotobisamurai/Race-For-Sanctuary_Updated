using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceShip : MonoBehaviour
{
    private Vector3 targetPos;

    [SerializeField] private Skully skully;
    [SerializeField] private float closeDuration = 5f;

    private bool isCloseToSkully = false;

    public event OnCaughtSkully OnCaughtSkullyEvent;
    public delegate void OnCaughtSkully();

    private Sequence moveCloserTween = null;

    [SerializeField] private Vector3 originalOffset;
    [SerializeField] private Vector3 closeOffset;
    [SerializeField] private Vector3 caughtOffset;
    private Vector3 currentOffset;

    private void Start()
    {
        currentOffset = originalOffset;
        skully.OnHitByMeteorEvent += Skully_OnHitByMeteorEvent;
    }

    private void OnDestroy()
    {
        skully.OnHitByMeteorEvent -= Skully_OnHitByMeteorEvent;
    }

    private void Skully_OnHitByMeteorEvent()
    {
        Debug.Log("police ship move closer");
        MoveCloser();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            MoveCloser();
        }

        targetPos = GameManager.Instance.Skully.transform.position + currentOffset;
        transform.position = targetPos;
    }

    public void MoveCloser()
    {
        if (!isCloseToSkully)
        {
            MoveCloseToSkully();
        }
        else
        {
            CaughtSkully();
        }
    }

    private void MoveCloseToSkully()
    {

        Debug.Log("move close to skully ");
        moveCloserTween = DOTween.Sequence();

        moveCloserTween.Append(DOVirtual.Vector3(originalOffset, closeOffset, 2f, (t) =>
        {
            currentOffset = t;
        }));
        moveCloserTween.AppendCallback(() => isCloseToSkully = true);
        moveCloserTween.AppendInterval(closeDuration);
        moveCloserTween.Append(DOVirtual.Vector3(closeOffset, originalOffset, 2f, (t) =>
        {
            currentOffset = t;
        }));
        moveCloserTween.AppendCallback(() => isCloseToSkully = false);
        moveCloserTween.Play();
    }

    public void CaughtSkully()
    {
        if (moveCloserTween != null)
        {
            moveCloserTween.Kill();
        }

        DOVirtual.Vector3(closeOffset, caughtOffset, 1f, (t) =>
        {
            currentOffset = t;
        }).OnComplete(() =>
        {
            OnCaughtSkullyEvent?.Invoke();
        });
    }

    public void MoveToOriginalPosition()
    {
        if (moveCloserTween != null)
        {
            moveCloserTween.Kill();
        }
        
        currentOffset = originalOffset;
        isCloseToSkully = false;
    }
}

