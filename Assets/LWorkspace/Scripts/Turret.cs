using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private Transform headTrans;

    [SerializeField] private float shootInterval = 1f;

    [SerializeField] private GameObject bullet;

    [SerializeField] private Transform muzzle;

    private float lastTimeShootTimeStamp = 0f;

    private void Update()
    {
        headTrans.LookAt(GameManager.Instance.Skully.transform.position );
    }

    private void OnTriggerStay(Collider other)
    {
        Skully skully = other.GetComponent<Skully>();

        if (skully != null)
        {
            if (Time.time > lastTimeShootTimeStamp + shootInterval)
            {
                Shoot();
                lastTimeShootTimeStamp = Time.time;
            }
        }
    }

    private void Shoot()
    {
        GameObject bulletInstance = GameObject.Instantiate(bullet);
        bulletInstance.transform.position = muzzle.position;
        bulletInstance.transform.rotation = muzzle.rotation;
    }
}
