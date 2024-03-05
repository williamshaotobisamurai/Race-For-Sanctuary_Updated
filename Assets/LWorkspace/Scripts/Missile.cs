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

    [SerializeField] private ParticleSystem fireParticle;
    [SerializeField] private ParticleSystem smokeParticle;

    [SerializeField] private GameObject explodeTrigger;

    private bool isLaunched = false;

    [SerializeField] private Transform muzzle;
    [SerializeField] private AudioSource launchAudio;

    public void AttachTo(Transform muzzle)
    {
        this.muzzle = muzzle;
    }

    public void Launch()
    {
        fireParticle.Play();
        smokeParticle.Play();
  
        isLaunched = true;
        launchedTime = Time.time;
        explodeTrigger.SetActive(true);
        launchAudio.Play();
    }

    private void Update()
    {
        if (!isLaunched)
        {
            transform.position = muzzle.position;
            return;
        }

        if (Time.time > launchedTime + lifeTime)
        {
            Explode();
        }
        else
        {
            Fly();
        }
    }

    private void Fly()
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
