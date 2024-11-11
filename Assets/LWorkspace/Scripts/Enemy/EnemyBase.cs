using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float lastTimeShootTimeStamp = 0f;

    [SerializeField] protected GameObject bullet;

    [SerializeField] protected Transform muzzle;

    [SerializeField] protected float shootInterval = 1f;

    [SerializeField] private Transform lookAtTrans;

    [SerializeField] protected SphereCollider detectTrigger;

    [SerializeField] private LayerMask layerMask;

    [SerializeField] protected int health = 100;

    [SerializeField] protected float advanceDistance = 30f;

    [SerializeField] private GameObject killedParticlePrefab;

    [SerializeField] private Image healthImg;

    [SerializeField] private ParticleSystem muzzleParticle;

    [SerializeField] protected float bulletSpread = 0f;

    protected bool isKilled = false;
    public bool IsKilled { get => isKilled; }

    protected int maxHealth = 0;


    private void Awake()
    {
        maxHealth = health;
    }

    private void Update()
    {
        LookAtSkully();
    }

    protected virtual void LookAtSkully()
    {
        if (lookAtTrans != null)
        {
            Skully skully = LevelManager.Instance.Skully;
            lookAtTrans.LookAt(skully.transform.position + skully.transform.forward * advanceDistance);
        }
    }

    void OnTriggerStay(Collider other)
    {
        Skully skully = other.GetComponent<Skully>();

        if (skully != null && skully.transform.position.z < transform.position.z)
        {
            AimAtSkully(skully);
        }
    }

    protected virtual void AimAtSkully(Skully skully)
    {
        if ((Time.time > lastTimeShootTimeStamp + shootInterval) && !IsBlocked(skully))
        {
            Shoot();
            lastTimeShootTimeStamp = Time.time;
        }
    }

    protected bool IsBlocked(Skully skully)
    {
        Vector3 direction = skully.transform.position - muzzle.transform.position;
        if (Physics.Raycast(muzzle.transform.position, direction, out RaycastHit hit, direction.magnitude * 1.2f, layerMask))
        {
            if (GameHelper.IsSkully(hit.collider, out Skully s))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    protected virtual GameObject Shoot()
    {
        muzzleParticle.Play();
        GameObject bulletInstance = GameObject.Instantiate(bullet);
        bulletInstance.transform.position = muzzle.position;
        bulletInstance.transform.eulerAngles = muzzle.eulerAngles + Random.insideUnitSphere * 90f * bulletSpread;
        return bulletInstance;
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, int.MaxValue);

        UpdateHealthBar();

        if (health <= 0)
        {
            Kill();
        }
    }

    protected virtual void UpdateHealthBar()
    {
        if (healthImg != null)
        {
            healthImg.fillAmount = (float)health / maxHealth;
        }
    }

    public virtual void Kill()
    {
        gameObject.SetActive(false);
        isKilled = true;
        if (killedParticlePrefab != null)
        {
            GameObject instance = Instantiate(killedParticlePrefab);
            instance.transform.position = transform.position;
        }
    }
}
