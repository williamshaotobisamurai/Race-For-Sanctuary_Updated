using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;

    [SerializeField] private int damamge = 10;
    public int Damage { get => damamge; }

    [SerializeField] private float lifeTime = 3f;

    private void Start()
    {
        DOVirtual.DelayedCall(lifeTime, () => { Destroy(gameObject); });
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }
}
