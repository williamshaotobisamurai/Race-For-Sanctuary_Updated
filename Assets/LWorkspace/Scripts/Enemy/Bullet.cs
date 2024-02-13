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

    [SerializeField] private float rotateRate = 20f;

    private void Start()
    {
        DOVirtual.DelayedCall(lifeTime, () => { Destroy(gameObject); });
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);

        //transform.LookAt()

        Quaternion prevRotation = transform.rotation;

        //transform.LookAt
        transform.LookAt(GameManager.Instance.Skully.transform);

        Quaternion desiredRotation = transform.rotation;

        transform.rotation =  Quaternion.Lerp(prevRotation, desiredRotation, Time.deltaTime * rotateRate);
        //  Quaternion.lerp

        if (transform.position.z < GameManager.Instance.Skully.transform.position.z - 3f)
        {
            gameObject.SetActive(false);
        }
    }
}
