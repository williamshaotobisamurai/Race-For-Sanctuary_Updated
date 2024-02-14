using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float lastTimeShootTimeStamp = 0f;

    [SerializeField] private GameObject bullet;

    [SerializeField] protected Transform muzzle;

    [SerializeField] protected float shootInterval = 1f;

    [SerializeField] private Transform lookAtTrans;

    [SerializeField] private SphereCollider detectTrigger;

    [SerializeField] private LayerMask layerMask;

    private void Update()
    {
        LookAtSkully();
    }

    protected virtual void LookAtSkully()
    {
        lookAtTrans.LookAt(GameManager.Instance.Skully.transform.position);
    }

    void OnTriggerStay(Collider other)
    {
        Skully skully = other.GetComponent<Skully>();

        if (skully != null)
        {
            Ray ray = new Ray(muzzle.transform.position, (skully.transform.position - muzzle.transform.position).normalized * detectTrigger.radius);
     
            if (Physics.Raycast(ray, out RaycastHit hitInfo, detectTrigger.radius,layerMask))
            {
                if (hitInfo.collider.GetComponent<Skully>() != null)
                {
                    Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.red);
                    AimAtSkully(skully);
                }
                else
                {
                    Debug.Log(gameObject.name + " blocked by " + hitInfo.collider.gameObject.name);
                    Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.yellow);
                }
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * detectTrigger.radius, Color.green);
            }
        }
    }

    protected virtual void AimAtSkully(Skully skully)
    {
        if (Time.time > lastTimeShootTimeStamp + shootInterval)
        {
            Shoot();
            lastTimeShootTimeStamp = Time.time;
        }

    }

    protected virtual GameObject Shoot()
    {
        GameObject bulletInstance = GameObject.Instantiate(bullet);
        bulletInstance.transform.position = muzzle.position;
        bulletInstance.transform.rotation = muzzle.rotation;
        return bulletInstance;
    }
}
