using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : MonoBehaviour
{
    [SerializeField] private float coverScreenDuration;
    public float CoverScreenDuration => coverScreenDuration;

    [SerializeField] private float speed = 10f;

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
