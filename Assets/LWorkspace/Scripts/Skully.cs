using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Skully : MonoBehaviour
{
    [SerializeField] private SkullyMovement skullyMovement;
    [SerializeField] private SIRS sirs;
    [SerializeField] private AudioSource collisionSound;
    [SerializeField] private Image healthBar;

    [SerializeField] private SkullyBounce skullyBounce;

    [SerializeField] private SIRSMount sirsMount;
    public SIRSMount SIRSMount { get => sirsMount; }

    private int maxHealth = 100;
    private int healthAmount = 100;
    public int HealthAmount
    {
        get => healthAmount; 
        set
        {
            healthAmount = value;
            UpdateHealthBar();
        }
    }

    [SerializeField] private Transform skullyVisual;

    public event OnRequestCopMoveForwardEvent OnRequestCopMoveForward;
    public delegate void OnRequestCopMoveForwardEvent();

    public event OnRequestCameraShakeEvent OnRequestCameraShake;
    public delegate void OnRequestCameraShakeEvent();

    public event OnSkullyDied OnSkullyDiedEvent;
    public delegate void OnSkullyDied();

    public event OnCollectCoin OnCollectCoinEvent;
    public delegate void OnCollectCoin(Coin coin);

    public event OnCollectItem OnCollectItemEvent;
    public delegate void OnCollectItem(ItemBase item);

    [SerializeField] private GameObject speedBoostAttachment;
    [SerializeField] private GameObject defensiveBoostAttachment;

    [SerializeField] private bool isInvincible = false;
    public bool IsInvincible { get => isInvincible; set => isInvincible = value; }

    [SerializeField] private Rigidbody rb;
    public Rigidbody Rigidbody { get { return rb; } }

    [SerializeField] private AudioSource boostAudioSource;

    [SerializeField] private GameObject killByEnergyFieldParticle;
    [SerializeField] private ParticleSystem hitByBlobParticle;

    [SerializeField] private SplatterManager splatterManager;

    [SerializeField] private Text healthText;

    [SerializeField] private SkullyWeaponManager skullyWeaponManager;
    public SkullyWeaponManager WeaponManager { get => skullyWeaponManager; }

    [SerializeField] private bool isLookingBack = false;
    public bool IsLookingBack { get => isLookingBack; set => isLookingBack = value; }

    [SerializeField] private SkullyOverheating skullyOverheating;

    [SerializeField] private SpeedUIManager speedUIManager;

    private bool isDead = false;

    private void Start()
    {
        skullyMovement.Init();
        sirs.OnCollectCoinEvent += Sirs_OnCollectCoinEvent;
        skullyOverheating.OnOverheatEvent += SkullyOverheating_OnOverheatEvent;
    }

    private void OnDestroy()
    {
        sirs.OnCollectCoinEvent -= Sirs_OnCollectCoinEvent;
    }

    private void LateUpdate()
    {
        speedUIManager.UpdateUI();
    }

    private void Sirs_OnCollectCoinEvent(Coin coin)
    {
        OnCollectCoinEvent?.Invoke(coin);
    }

    private void ReflectMeteor(LargeStaticMeteor obstacle, float strength)
    {
        Vector3 forceDirection = obstacle.transform.position - transform.position;
        forceDirection.z = 0;

        obstacle.GetComponent<Collider>().enabled = false;
        obstacle.GetComponent<Rigidbody>().isKinematic = true;
        RandomMovement randomMovement = obstacle.GetComponent<RandomMovement>();
        randomMovement?.StopMoving();

        Vector3 randomDirection = Random.onUnitSphere;

        float minDistance = 20 * strength;
        float maxDistance = 30 * strength;

        Vector3 offset = new Vector3(randomDirection.x * Random.Range(minDistance, maxDistance), randomDirection.y * Random.Range(minDistance, maxDistance), 50);
        Vector3 currentPos = obstacle.transform.position;
        Vector3 target = offset + currentPos;
        obstacle.transform.DOMove(target, 2);
    }

    private void HitByMeteroCollider(LargeStaticMeteor obstacle)
    {
        Debug.Log("hit by obstacle " + obstacle.name + " " + obstacle.DoDamage);
        if (obstacle.DoDamage)
        {
            TakeDamage(obstacle.Damage);
        }
        else
        {
            skullyVisual.transform.DOShakeRotation(0.1f, 10);
            OnRequestCopMoveForward?.Invoke();
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

                case ItemBase.EItemType.COOLING_ITEM:
                    CollectCoolingItem(itemBase as CoolingItem);
                    break;


                case ItemBase.EItemType.WEAPON_ITEM:
                    CollectWeaponItem(itemBase as WeaponItem);
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
        else if (other.tag.Equals(GameConstants.OBSTACLE))
        {
            Debug.Log("on hit by obstcle");

            collisionSound.Play();
            if (!IsInvincible)
            {
                ReflectMeteor(other.GetComponent<LargeStaticMeteor>(), 1f);
                HitByMeteroCollider(other.GetComponent<LargeStaticMeteor>());
            }
            else
            {
                ReflectMeteor(other.GetComponent<LargeStaticMeteor>(), 10f);

            }
        }
    }

    private void KilledByEnergyField(EnergyField field)
    {
        killByEnergyFieldParticle.SetActive(true);

        if (!IsInvincible && !isDead)
        {
            rb.isKinematic = true;

            healthAmount = 0;
            UpdateHealthBar();
            isDead = true;
            OnSkullyDiedEvent?.Invoke();
            OnRequestCameraShake?.Invoke();

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
        bullet.OnHit();
        if (!IsInvincible)
        {
            TakeDamage(bullet.Damage);
        }
    }

    private void HitByBlob(Blob blob)
    {
        blob.OnHit();
        blob.gameObject.SetActive(false);
        hitByBlobParticle.Play();
        splatterManager.Show(blob.CoverScreenDuration);
        OnRequestCameraShake?.Invoke();
    }

    public void HitByLaser(int damage)
    {
        if (!IsInvincible)
        {
            TakeDamage(damage);
        }
    }

    public void HitByLightningStrike()
    {
        KilledByEnergyField(null);
    }

    public void HitBySniper()
    {
        if (!IsInvincible)
        {
            maxHealth -= 50;
            TakeDamage(50);
            if (healthAmount >= 50)
            {
                OnRequestCopMoveForward?.Invoke();
            }
        }
    }

    public void TakeDamage(int damage)
    {
        healthAmount -= damage;
        healthAmount = Mathf.Clamp(healthAmount, 0, maxHealth);
        UpdateHealthBar();

        if (!isDead)
        {
            if (healthAmount <= 0f)
            {
                Vector3 randomAngularVelocity = GetRandomAngularVelocity();
                rb.freezeRotation = false;
                skullyMovement.StopRunning();
                rb.angularVelocity = randomAngularVelocity;
                isDead = true;
                DOVirtual.DelayedCall(3f, () =>
                {
                    OnSkullyDiedEvent?.Invoke();
                });
            }
            else
            {
                OnRequestCameraShake?.Invoke();
                skullyVisual.transform.DOShakeRotation(0.1f, 10);
            }
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
        IsInvincible = true;

        float flashTimeStamp = defensiveBoost.GetDefensiveDuration() - 3;
        flashTimeStamp = Mathf.Clamp(flashTimeStamp, 0, flashTimeStamp);
        DOVirtual.DelayedCall(flashTimeStamp, () =>
        {

            defensiveBoostAttachment.GetComponent<DefensiveAttachment>().StartFlashingForSeconds(defensiveBoost.GetDefensiveDuration());
        });

        DOVirtual.DelayedCall(defensiveBoost.GetDefensiveDuration(), () =>
        {
            defensiveBoostAttachment.SetActive(false);

            IsInvincible = false;
        });
    }

    public void CollectJetpackFuel(JetpackFuelItem jetpackFuelItem)
    {
        OnCollectItemEvent?.Invoke(jetpackFuelItem);
    }

    public void CollectCoolingItem(CoolingItem item)
    {
        OnCollectItemEvent?.Invoke(item);
        skullyOverheating.ReduceOverheatingProgress(item.CoolingAmount);
    }

    public void CollectHealItem(HealItem healItem)
    {
        healthAmount += healItem.HealAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, maxHealth);

        UpdateHealthBar();
    }

    private void CollectWeaponItem(WeaponItem weaponItem)
    {
        skullyWeaponManager.SetupWeapon(weaponItem.WeaponType);
    }

    private void UpdateHealthBar()
    {
        healthBar.DOFillAmount(healthAmount / (float)maxHealth, 0.5f);
        healthText.text = healthAmount.ToString() + " / " + maxHealth.ToString();
    }

    public void SetMaxForwardSpeedFactor(float speedFactor)
    {
        Debug.Log("set max forward speed factor ");
        skullyMovement.SetMaxForwardSpeedFactor(speedFactor);
    }

    public float GetMaxSpeedFactor()
    {
        return skullyMovement.GetMaxSpeedFactor();
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
            OnRequestCopMoveForward?.Invoke();
            rb.isKinematic = false;
            if (healthAmount <= 0 && !isDead)
            {
                Vector3 randomAngularVelocity = GetRandomAngularVelocity();
                rb.freezeRotation = false;
                skullyMovement.StopRunning();
                rb.angularVelocity = randomAngularVelocity;
                isDead = true;

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

    public void Revive()
    {
        Debug.Log("revive");
        healthAmount = maxHealth;
        healthBar.fillAmount = healthAmount / (float)maxHealth;
        UpdateHealthBar();
        rb.freezeRotation = false;
        skullyMovement.StartRunning();
        isDead = false;
    }

    public Vector3 GetCurrentVelocity()
    {
        return skullyMovement.GetCurrentVelocity();
    }

    public void DisableControl()
    {
        skullyMovement.enabled = false;
    }

    public void EnableControl()
    {
        skullyMovement.enabled = true;
    }

    public void EnableXYControl()
    {
        skullyMovement.EnableXYControl();
    }

    public void DisableXYControl()
    {
        skullyMovement.DisableXYControl();
    }

    public void EnterBossMode()
    {
        skullyMovement.SetMaxForwardSpeedFactor(0);
    }

    public void ExitBossMode()
    {
        skullyMovement.SetMaxForwardSpeedFactor(1);
    }

    public void StartOverheating()
    {
        skullyOverheating.StartOverheating();
    }

    public void StopOverheating()
    {
        skullyOverheating.StopOverheating();
    }

    private void SkullyOverheating_OnOverheatEvent()
    {
        isDead = true;
        DisableControl();
        InstructionManager.ShowText("I'm burned out", 2f, () =>
        {
            OnSkullyDiedEvent?.Invoke();
        });
    }

    public void SkullyHitSpaceStationWall()
    {
        skullyMovement.StopRunning();
        skullyBounce.Bounce(skullyMovement.GetCurrentVelocity());
        DOVirtual.DelayedCall(3f, () =>
        {
            skullyMovement.StartRunning();
        });
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag.Equals(GameConstants.SPACE_STATION))
        {
            skullyMovement.StopRunning();
            rb.velocity = Vector3.zero;
            skullyBounce.Bounce(hit.normal);
            collisionSound.Play();
            DOVirtual.DelayedCall(3f, () =>
            {
                if (!isDead)
                {
                    skullyMovement.StartRunning();
                }
            });
            Debug.Log("on controller hit " + hit.collider);
        }
    }

    public void AddExternalSpeed(Vector3 externalSpeed)
    {
        skullyMovement.AddExternalSpeed(externalSpeed);
    }

    public void StopExternalSpeed(float decay)
    {
        skullyMovement.StopExternalSpeed(decay);
    }

    public void ActiveSIRS()
    {
        sirs.gameObject.SetActive(true);
    }

    public void DisableSIRS()
    { 
        sirs.gameObject.SetActive(false);
    }

    public int SIRSActivated()
    {
        return sirs.isActiveAndEnabled ? 1 : 0;
    }
}
