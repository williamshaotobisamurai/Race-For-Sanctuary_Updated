using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBulletBase : MonoBehaviour
{
    [SerializeField] private int damamge = 10;
    public int Damage { get => damamge; }

    [SerializeField] protected GameObject hitParticle;

    [SerializeField] private float rotateRate = 20f;
    public float RotateRate { get => rotateRate; set => rotateRate = value; }
    [SerializeField] private float speed = 10f;

    public float Speed { get => speed; set => speed = value; }

    protected bool isOverSkully = false;

    private void Update()
    {
        Fly();
    }

    protected virtual void Fly()
    {
        Skully skully = LevelManager.Instance.Skully;

        bool flyOverSkully =
            (skully.transform.forward.z > 0 && transform.position.z < skully.transform.position.z - 5f)
            || (skully.transform.forward.z < 0 && transform.position.z > skully.transform.position.z + 5f); 

        if (flyOverSkully)
        {
            if (!isOverSkully)
            {
                rotateRate = 0f;
                isOverSkully = true;
                OnFlyOverSkully();
            }
        }
        else
        {
            Quaternion prevRotation = transform.rotation;
            transform.LookAt(LevelManager.Instance.Skully.transform);
            Quaternion desiredRotation = transform.rotation;

            transform.rotation = Quaternion.Lerp(prevRotation, desiredRotation, Time.deltaTime * RotateRate);
        }
        transform.Translate(Vector3.forward * Speed * Time.deltaTime, Space.Self);
    }

    public virtual void OnHit()
    {
        GameObject explode = Instantiate(hitParticle);
        explode.transform.position = transform.position;
        Destroy(gameObject);
    }

    public abstract void OnFlyOverSkully();

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BlockBullet>() != null)
        {
            OnHit();
        }
    }
}
