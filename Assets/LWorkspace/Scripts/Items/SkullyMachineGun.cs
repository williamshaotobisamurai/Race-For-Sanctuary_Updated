using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullyMachineGun : MonoBehaviour
{
    private const int rayLength = 1000;
    [SerializeField] private float firingInterval;

    private float lastFireTimeStamp;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform muzzleTrans;
    [SerializeField] private GameObject muzzleParticle;
    [SerializeField] private float spread = 3f;
    [SerializeField] private AudioSource gunAudio;

    [SerializeField] private AudioSource reloadAudio;

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

    private void Shoot(Ray ray)
    {
        GameObject bulletInstance = Instantiate(bulletPrefab);
        bulletInstance.transform.localPosition = muzzleTrans.position;
        bulletInstance.transform.LookAt(ray.origin + ray.direction * rayLength + Random.insideUnitSphere * spread);

        GameObject muzzleFireInstance = Instantiate(muzzleParticle);
        muzzleFireInstance.transform.position = muzzleTrans.position;
        muzzleFireInstance.transform.rotation = Quaternion.identity;

    }
}
