using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullyPistolGun : SkullyMachineGun
{
    public event OnShootBullet OnShootBulletEvent;
    public delegate void OnShootBullet(SkullyBulletBase bulletBase);

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

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

    protected override SkullyBulletBase Shoot(Ray ray)
    {
        SkullyBulletBase bullet = base.Shoot(ray);
        gunAudio.Play();
        OnShootBulletEvent?.Invoke(bullet);
        return bullet;
        //gunAudio.PlayOneShot();
    }
}
