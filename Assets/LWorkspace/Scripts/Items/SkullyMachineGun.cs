using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullyMachineGun : MonoBehaviour
{
    protected private const int rayLength = 1000;
    [SerializeField] protected float firingInterval;

    protected float lastFireTimeStamp;

    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected Transform muzzleTrans;
    [SerializeField] protected GameObject muzzleParticle;
    [SerializeField] protected float spread = 3f;
    [SerializeField] protected AudioSource gunAudio;

    [SerializeField] private AudioSource reloadAudio;

    public event OnShootBullet OnShootBulletEvent;
    public delegate void OnShootBullet(SkullyBulletBase bulletBase);

    private void OnEnable()
    {
        reloadAudio.Play();
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            gunAudio.Play();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            gunAudio.Stop();
        }

        if (Input.GetMouseButton(0))
        {
            if (Time.time >= lastFireTimeStamp + firingInterval)
            {
                transform.DOShakeRotation(firingInterval, 8);
                lastFireTimeStamp = Time.time;
                Shoot(ray);
            }
        }
        transform.LookAt(ray.origin + ray.direction * rayLength);
    }

    protected virtual SkullyBulletBase Shoot(Ray ray)
    {
        GameObject bulletInstance = Instantiate(bulletPrefab);
        bulletInstance.transform.localPosition = muzzleTrans.position;
        bulletInstance.transform.LookAt(ray.origin + ray.direction * rayLength + Random.insideUnitSphere * spread);

        GameObject muzzleFireInstance = Instantiate(muzzleParticle, muzzleTrans);
        muzzleFireInstance.transform.position = muzzleTrans.position;
        muzzleFireInstance.transform.rotation = Quaternion.identity;

        SkullyBulletBase bullet =  bulletInstance.GetComponent<SkullyBulletBase>();
        OnShootBulletEvent?.Invoke(bullet);
        return bullet;
    }
}
