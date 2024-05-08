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

    [SerializeField] private LookAtConstraint healthUILookAtConstraint;

    protected bool isKilled = false;
    public bool IsKilled { get => isKilled; }

    protected int maxHealth = 0;


    private void Awake()
    {
        maxHealth = health;
    }

    protected virtual void Start()
    {
        ConstraintSource src = new ConstraintSource();
        src.sourceTransform = Camera.main.transform;
        if (healthUILookAtConstraint.sourceCount == 0)
        {
            healthUILookAtConstraint.AddSource(src);
        }
        else
        {
            healthUILookAtConstraint.SetSource(0, src);
        }
    }

    private void Update()
    {
        LookAtSkully();
    }

    protected virtual void LookAtSkully()
    {
        Skully skully = GameManager.Instance.Skully;
        lookAtTrans.LookAt(skully.transform.position + skully.transform.forward * advanceDistance);
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

    private bool IsBlocked(Skully skully)
    {
        Vector3 direction = skully.transform.position - muzzle.transform.position;
        if (Physics.Raycast(muzzle.transform.position,direction,  out RaycastHit hit, direction.magnitude * 1.2f,layerMask))
        {
            return true;
        }
        return false;
    }

    protected virtual GameObject Shoot()
    {
        GameObject bulletInstance = GameObject.Instantiate(bullet);
        bulletInstance.transform.position = muzzle.position;
        bulletInstance.transform.rotation = muzzle.rotation;
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
        healthImg.fillAmount = (float)health / maxHealth;
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
