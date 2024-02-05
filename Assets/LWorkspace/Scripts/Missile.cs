using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private float accelerate = 10f;

    [SerializeField] private float topSpeed = 100f;

    private float launchedTime = 0f;

    [SerializeField] private GameObject explodeParticle;

    private void Start()
    {
        launchedTime = Time.time;
    }

    private void Update()
    {
        if (Time.time > launchedTime + lifeTime)
        {
            Explode();
        }
        else
        {
            Launch();
        }
    }

    private void Launch()
    {
        float speedThisFrame = Mathf.Lerp(0, topSpeed, accelerate * Time.deltaTime);
        transform.Translate(new Vector3(0, 0, speedThisFrame) * Time.deltaTime, Space.Self);
    }

    public void Explode()
    {
        GameObject particle = Instantiate(explodeParticle);
        particle.transform.position = transform.position;
        DOVirtual.DelayedCall(3f, () => Destroy(particle));
        Destroy(gameObject);
    }
}
