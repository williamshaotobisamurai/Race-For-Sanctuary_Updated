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
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, rayLength, SkullyWeaponManager.Instance.TargetsLayer))
                {
                    Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.cyan, 5f);

                    if (hit.collider.tag.Equals(GameConstants.OBSTACLE) || hit.collider.tag.Equals(GameConstants.ENEMY_HITPOINT))
                    {
                        Debug.DrawRay(ray.origin, hit.collider.transform.position - ray.origin, Color.magenta, 5f);
                        // aimmingObject.transform.position = hit.collider.transform.position;
                    }
                }
                transform.DOShakeRotation(firingInterval, 5);
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
