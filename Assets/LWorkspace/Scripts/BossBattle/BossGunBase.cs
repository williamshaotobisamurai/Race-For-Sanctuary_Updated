using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGunBase : MonoBehaviour
{
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected Transform muzzleTrans;
    [SerializeField] protected float bulletInterval = 0.2f;

    [SerializeField] private protected Skully skully;
    protected float lastBulletTimeStamp = 0f;
    protected bool isFiring = false;
    // Start is called before the first frame update
    [SerializeField] protected float spread = 3f;
    [SerializeField] private GameObject muzzleParticlePrefab;

    private void Start()
    {
        skully = LevelManager.Instance.Skully;
    }

    public virtual void StartFiring()
    {
        Debug.Log("boss machine gun start firing");
        isFiring = true;
    }

    public virtual void StopFiring()
    {
        Debug.Log("boss machine gun stop firing");
        isFiring = false;
    }

    private void Update()
    {
        if (isFiring)
        {
            Fire();
        }
    }

    public virtual void Fire()
    {
        if (Time.time > lastBulletTimeStamp + bulletInterval)
        {
            GameObject bulletInstance = Instantiate(bulletPrefab);

            bulletInstance.transform.position = muzzleTrans.position;
            bulletInstance.transform.LookAt(skully.transform.position + Random.insideUnitSphere * spread);
            lastBulletTimeStamp = Time.time;

            if (muzzleParticlePrefab != null)
            {
                GameObject muzzleInstance = Instantiate(muzzleParticlePrefab, muzzleTrans);
                muzzleInstance.SetActive(true);
                muzzleInstance.transform.localPosition = Vector3.zero;
                muzzleInstance.transform.localRotation = Quaternion.identity;
            }
        }
    }
}
