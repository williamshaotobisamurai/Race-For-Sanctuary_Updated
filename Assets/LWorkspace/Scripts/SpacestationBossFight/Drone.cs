using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : EnemyBase
{
    private bool enableAutoMovement = false;

    private float autoMoveInterval = 10f;
    private float autoMoveIntervalVariation = 3f;

    private float nextAutoMoveTimestamp;

    [SerializeField] private float targetDistance = 15f;
    [SerializeField] private float speed = 10f;

    private Tween movementTween;

    [SerializeField] private Animator m_animator;

    [SerializeField] private Transform droneVisual;


    public void Init(Transform initTrans ,float targetDistance)
    {
        this.targetDistance = targetDistance;
        transform.DOMove(initTrans.position + initTrans.forward * Random.Range(10f, 20f), Random.Range(1f, 2f)).SetEase(Ease.OutQuart).OnComplete(() =>
        {
            enableAutoMovement = true;
        });
    }

    private void Update()
    {
        transform.LookAt(LevelManager.Instance.Skully.transform.position);
        if (enableAutoMovement)
        {
            if (Time.time > nextAutoMoveTimestamp)
            {
                m_animator.enabled = false;

                nextAutoMoveTimestamp = Time.time + autoMoveInterval + Random.Range(-autoMoveIntervalVariation, autoMoveIntervalVariation);

                Vector3 targetPos = GetRandomPositionAroundSkully(targetDistance);

                float distance = Vector3.Distance(targetPos, transform.position);

                if (movementTween != null)
                {
                    movementTween.Kill();
                    movementTween = null;
                }

                m_animator.enabled = false;
                movementTween = transform.DOMove(targetPos, distance / speed).OnComplete(() =>
                {
                    m_animator.enabled = true;
                });
            }
            else
            {
                AimAtSkully(LevelManager.Instance.Skully);
            }
        }
    }

    private bool isShaking = false;
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (health > 0 && !isShaking)
        {
            droneVisual.DOShakePosition(0.2f, 0.5f);
            droneVisual.DOShakeRotation(0.2f, 1f).OnComplete(() =>
            {
                isShaking = false;
                droneVisual.transform.localPosition = Vector3.zero;
                droneVisual.transform.localRotation = Quaternion.identity;
            });
        }
    }

    private Vector3 GetRandomPositionAroundSkully(float targetDistance)
    {
        int tries = 0;
        float maxDistance = 0;
        Vector3 pendingPos = transform.position;
        while (tries < 10)
        {
            tries++;

            Skully skully = LevelManager.Instance.Skully;
            Vector3 pos = skully.transform.position + Random.insideUnitSphere * 50f;
            pos.z = skully.transform.position.z + targetDistance;

            float distance = Vector3.Distance(pos, transform.position);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                pendingPos = pos;
            }
        }

        return pendingPos;
    }
}
