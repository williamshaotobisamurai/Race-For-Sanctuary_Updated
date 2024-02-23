using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Skully : MonoBehaviour
{
    [SerializeField] private SkullyMovement skullyMovement;
    [SerializeField] private SIRS sirs;
    [SerializeField] private AudioSource collisionSound;
    [SerializeField] private Image healthBar;

    private int maxHealth = 100;
    private int healthAmount = 100;

    [SerializeField] private Transform skullyVisual;

    public event OnHitByMeteor OnHitByMeteorEvent;
    public delegate void OnHitByMeteor();

    public event OnSkullyDied OnSkullyDiedEvent;
    public delegate void OnSkullyDied();

    public event OnCollectCoin OnCollectCoinEvent;
    public delegate void OnCollectCoin(Coin coin);

    public event OnCollectItem OnCollectItemEvent;
    public delegate void OnCollectItem(ItemBase item);

    [SerializeField] private GameObject speedBoostAttachment;
    [SerializeField] private GameObject defensiveBoostAttachment;

    [SerializeField] private bool isInvincible = false;

    [SerializeField] private Rigidbody rb;
    public Rigidbody Rigidbody { get { return rb; } }

    [SerializeField] private AudioSource boostAudioSource;

    [SerializeField] private GameObject killByEnergyFieldParticle;
    [SerializeField] private ParticleSystem hitByBlobParticle;

    [SerializeField] private SplatterManager splatterManager;

    [SerializeField] private Text healthText;

    private void Start()
    {
        skullyMovement.Init();
        sirs.OnCollectCoinEvent += Sirs_OnCollectCoinEvent;
    }

    private void OnDestroy()
    {
        sirs.OnCollectCoinEvent -= Sirs_OnCollectCoinEvent;
    }

    private void Sirs_OnCollectCoinEvent(Coin coin)
    {
        OnCollectCoinEvent?.Invoke(coin);
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        Obstacle obstacle = collisionInfo.gameObject.GetComponent<Obstacle>();
        if (collisionInfo.collider.tag.Equals(GameConstants.OBSTACLE))
        {
            collisionSound.Play();
            if (isInvincible)
            {
                ReflectMeteor(collisionInfo);
            }
            else
            {
                HitByMeteroCollider(obstacle);
            }
        }
    }

    private void ReflectMeteor(Collision collisionInfo)
    {
        Vector3 forceDirection = collisionInfo.transform.position - transform.position;
        forceDirection.z = 0;

        collisionInfo.collider.enabled = false;
        collisionInfo.rigidbody.isKinematic = true;
        RandomMovement randomMovement = collisionInfo.collider.GetComponent<RandomMovement>();
        randomMovement?.StopMoving();

        Vector3 randomDirection = Random.onUnitSphere;
        Vector3 offset = new Vector3(randomDirection.x * Random.Range(200, 300), randomDirection.y * Random.Range(200, 300), 50);
        Vector3 currentPos = collisionInfo.transform.position;
        Vector3 target = offset + currentPos;
        collisionInfo.transform.DOMove(target, 2);
    }

    private void HitByMeteroCollider(Obstacle obstacle)
    {
        if (obstacle.DoDamage)
        {
            TakeDamage(obstacle.Damage);
        }
        else
        {
            skullyVisual.transform.DOShakeRotation(0.1f, 10);
            OnHitByMeteorEvent?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(GameConstants.BOOST_ITEM))
        {
            ItemBase itemBase = other.GetComponent<ItemBase>();
            switch (itemBase.ItemType)
            {
                case ItemBase.EItemType.SPEED_BOOST:
                    CollectSpeedBoost(itemBase as SpeedBoost);
                    break;

                case ItemBase.EItemType.DEFENSIVE_BOOST:
                    CollectDefensiveBoost(itemBase as DefensiveBoost);
                    break;

                case ItemBase.EItemType.JETPACK_FUEL:
                    CollectJetpackFuel(itemBase as JetpackFuelItem);
                    break;

                case ItemBase.EItemType.HEAL_ITEM:
                    CollectHealItem(itemBase as HealItem);
                    break;

                default:
                    break;
            }
            itemBase.Collect();
        }
        else if (other.tag.Equals(GameConstants.BULLLET))
        {
            HitByBullet(other.GetComponent<EnemyBulletBase>());
        }
        else if (other.tag.Equals(GameConstants.BLOB))
        {
            HitByBlob(other.GetComponent<Blob>());
        }
        else if (other.tag.Equals(GameConstants.TRAP))
        {
            EnergyField field = other.GetComponent<EnergyField>();
            if (field != null)
            {
                KilledByEnergyField(field);
            }
        }
    }

    private void KilledByEnergyField(EnergyField field)
    {
        killByEnergyFieldParticle.SetActive(true);
     
        if (!isInvincible)
        {
            rb.isKinematic = true;

            healthAmount = 0;
            UpdateHealthBar();
            OnSkullyDiedEvent?.Invoke();

            skullyVisual.transform.DOShakeRotation(0.5f, 50).OnComplete(() =>
            {
                float x = Random.Range(-90f, 90f);
                float y = Random.Range(-90f, 90f);
                float z = Random.Range(-90f, 90f);

                skullyVisual.transform.DORotate(new Vector3(x, y, z), 0.5f);
            });

        }
    }



    private void HitByBullet(EnemyBulletBase bullet)
    {
        bullet.OnHitPlayer();
        if (!isInvincible)
        {
            TakeDamage(bullet.Damage);
        }
    }

    private void HitByBlob(Blob blob)
    {
        blob.OnHitPlayer();
        blob.gameObject.SetActive(false);
        hitByBlobParticle.Play();
        splatterManager.Show(blob.CoverScreenDuration);
    }

    public void HitByLightningStrike()
    {
        KilledByEnergyField(null);
    }

    public void HitBySniper()
    {
        if (!isInvincible)
        {
            maxHealth -= 50;
            TakeDamage(50);
            if (healthAmount >= 50)
            {
                OnHitByMeteorEvent?.Invoke();
            }
        }
    }

    public void TakeDamage(int damage)
    {
        healthAmount -= damage;
        healthAmount = Mathf.Clamp(healthAmount, 0, maxHealth);
        UpdateHealthBar();

        if (healthAmount <= 0f)
        {
            Vector3 randomAngularVelocity = GetRandomAngularVelocity();
            rb.freezeRotation = false;
            skullyMovement.StopRunning();
            rb.angularVelocity = randomAngularVelocity;

            DOVirtual.DelayedCall(3f, () =>
            {
                OnSkullyDiedEvent?.Invoke();
            });
        }
        else
        {
            skullyVisual.transform.DOShakeRotation(0.1f, 10);
        }
    }

    private static Vector3 GetRandomAngularVelocity()
    {
        float x = Random.Range(-90f, 90f);
        float y = Random.Range(-90f, 90f);
        float z = Random.Range(-90f, 90f);
        Vector3 randomAngularVelocity = new Vector3(x, y, z);
        return randomAngularVelocity;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void CollectSpeedBoost(SpeedBoost speedBoost)
    {
        speedBoostAttachment.SetActive(true);
        DOVirtual.DelayedCall(speedBoost.GetSpeedUpDuration(), () => speedBoostAttachment.SetActive(false));
        skullyMovement.SpeedBoost(speedBoost);
    }

    public void CollectDefensiveBoost(DefensiveBoost defensiveBoost)
    {
        defensiveBoostAttachment.SetActive(true);
        isInvincible = true;

        DOVirtual.DelayedCall(defensiveBoost.GetDefensiveDuration(), () =>
        {
            defensiveBoostAttachment.SetActive(false);

            isInvincible = false;
        });
    }

    public void CollectJetpackFuel(JetpackFuelItem jetpackFuelItem)
    {
        OnCollectItemEvent?.Invoke(jetpackFuelItem);
    }

    public void CollectHealItem(HealItem healItem)
    {
        Heal(healItem.HealAmount);
    }

    public void Heal(int healingAmount)
    {
        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, maxHealth);

        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthBar.DOFillAmount(healthAmount / (float)maxHealth, 0.5f);
        healthText.text = healthAmount.ToString() + " / " + maxHealth.ToString();
    }

    public void SetMaxSpeedFactor(float speedFactor)
    {
        skullyMovement.SetMaxSpeedFactor(speedFactor);
    }

    public void AttackByAlienJumpAttack(HangingAlien hangingAlien)
    {
        rb.isKinematic = true;
        skullyMovement.StopRunning();

        int targetHealth = healthAmount - hangingAlien.JumpAttackDamage;

        targetHealth = Mathf.Clamp(targetHealth, 0, maxHealth);
        skullyVisual.transform.DOShakeRotation(hangingAlien.JumpAttckBiteDuration, 10);
        DOVirtual.Int(healthAmount, targetHealth, hangingAlien.JumpAttckBiteDuration, (v) =>
        {
            healthAmount = v;
            healthBar.fillAmount = healthAmount / (float)maxHealth;
            healthText.text = healthAmount.ToString() + " / " + maxHealth.ToString();
        }).OnComplete(() =>
        {
            OnHitByMeteorEvent?.Invoke();
            rb.isKinematic = false;
            if (healthAmount <= 0)
            {
                Vector3 randomAngularVelocity = GetRandomAngularVelocity();
                rb.freezeRotation = false;
                skullyMovement.StopRunning();
                rb.angularVelocity = randomAngularVelocity;

                DOVirtual.DelayedCall(3f, () =>
                {
                    OnSkullyDiedEvent?.Invoke();
                });
            }
            else
            {
                rb.isKinematic = false;
                skullyMovement.StartRunning();
            }
        });
    }
}
