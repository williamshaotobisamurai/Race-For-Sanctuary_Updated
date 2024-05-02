using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullyPistolGun : SkullyMachineGun
{
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

    protected override void Shoot(Ray ray)
    {
        base.Shoot(ray);
        gunAudio.Play();
        //gunAudio.PlayOneShot();
    }
}
