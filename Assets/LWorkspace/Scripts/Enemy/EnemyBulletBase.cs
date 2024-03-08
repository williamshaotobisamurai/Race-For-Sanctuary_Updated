using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBase : MonoBehaviour
{
    [SerializeField] private int damamge = 10;
    public int Damage { get => damamge; }

    [SerializeField] protected GameObject hitParticle;
    
    [SerializeField] private float rotateRate = 20f;
    public float RotateRate { get => rotateRate; set => rotateRate = value; }
    [SerializeField] private float speed = 10f;

    public float Speed { get => speed; set => speed = value; }
    [SerializeField] private float delayDestroy = 5f;

    [SerializeField] protected bool isFlying = false;
    public bool IsFlying => isFlying;

    private void Update()
    {
        Fly();
    }

    protected virtual void Fly()
    {
        if (isFlying && transform.position.z < GameManager.Instance.Skully.transform.position.z - 5f)
        {
            isFlying = false;
            DOVirtual.DelayedCall(delayDestroy, () => { Destroy(gameObject); });
        }
        else
        {
            isFlying = true;
            transform.Translate(Vector3.forward * Speed * Time.deltaTime, Space.Self);

            Quaternion prevRotation = transform.rotation;

            transform.LookAt(GameManager.Instance.Skully.transform);

            Quaternion desiredRotation = transform.rotation;

            transform.rotation = Quaternion.Lerp(prevRotation, desiredRotation, Time.deltaTime * RotateRate);
        }
    }

    public virtual void OnHitPlayer()
    {
        GameObject explode = Instantiate(hitParticle);
        explode.transform.position = transform.position;
        gameObject.SetActive(true);
    }

//    public abstract
}
