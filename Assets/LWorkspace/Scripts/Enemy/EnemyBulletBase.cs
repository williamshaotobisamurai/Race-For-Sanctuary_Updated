using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBase : MonoBehaviour
{
    [SerializeField] private int damamge = 10;
    public int Damage { get => damamge; }

    [SerializeField] protected GameObject hitParticle;
    [SerializeField] protected float rotateRate = 20f;
    [SerializeField] protected float speed = 10f;

    [SerializeField] private float delayDestroy = 5f;

    private bool isFlying = false;
    public bool IsFlying => isFlying;

    private void Update()
    {
        Fly();
    }

    protected virtual void Fly()
    {
        if (transform.position.z < GameManager.Instance.Skully.transform.position.z - 5f)
        {
            isFlying = false;
            DOVirtual.DelayedCall(delayDestroy, () => { Destroy(gameObject); });
        }
        else
        {
            isFlying = true;
            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);

            Quaternion prevRotation = transform.rotation;

            transform.LookAt(GameManager.Instance.Skully.transform);

            Quaternion desiredRotation = transform.rotation;

            transform.rotation = Quaternion.Lerp(prevRotation, desiredRotation, Time.deltaTime * rotateRate);
        }
    }

    public virtual void OnHitPlayer()
    {
        GameObject explode = Instantiate(hitParticle);
        explode.transform.position = transform.position;
        gameObject.SetActive(true);
    }
}
