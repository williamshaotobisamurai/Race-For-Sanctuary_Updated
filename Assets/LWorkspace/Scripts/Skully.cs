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
    [SerializeField] private float healthAmount = 100f;

    public event OnSkullyDied OnSkullyDiedEvent;
    public delegate void OnSkullyDied();

    public event OnCollectCoin OnCollectCoinEvent;
    public delegate void OnCollectCoin(Coin coin);

    [SerializeField] private GameObject speedBoostAttachment;
    [SerializeField] private GameObject defensiveBoostAttachment;

    [SerializeField] private bool isInvincible = false;

    [SerializeField] private Rigidbody rb;
    public Rigidbody Rigidbody { get { return rb; } }

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
        if (collisionInfo.collider.tag.Equals(GameConstants.OBSTACLE))
        {
            collisionSound.Play();
            if (isInvincible)
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
                collisionInfo.transform.DOMove(target, 2).OnUpdate(() => Debug.Log(collisionInfo.transform.position));
            }
            else
            {
                skullyMovement.StopRunning();
                OnSkullyDiedEvent?.Invoke();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(GameConstants.SPEED_BOOST))
        {
            CollectSpeedBoost(other.GetComponent<SpeedBoost>());
        }
        else if (other.tag.Equals(GameConstants.DEFENSIVE_BOOST))
        {
            CollectDefensiveBoost(other.GetComponent<DefensiveBoost>());
        }
        else if (other.tag.Equals(GameConstants.BULLLET))
        {
            HitByBullet(other.GetComponent<Bullet>());
        }
    }

    private void HitByBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        if (!isInvincible)
        { 
            TakeDamage(bullet.Damage);        
        }
    }

    public void TakeDamage(int damage)
    {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / 100f;

        transform.DOShakeRotation(0.1f,10);

        if (healthAmount <= 0f)
        {
            OnSkullyDiedEvent?.Invoke();
        }
    }

    public void Heal(float healingAmount)
    {
        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);

        healthBar.fillAmount = healthAmount / 100f;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void CollectSpeedBoost(SpeedBoost speedBoost)
    {
        speedBoost.gameObject.SetActive(false);
        speedBoostAttachment.SetActive(true);
        DOVirtual.DelayedCall(speedBoost.GetSpeedUpDuration(), () => speedBoostAttachment.SetActive(false));
        skullyMovement.SpeedBoost(speedBoost);
    }

    public void CollectDefensiveBoost(DefensiveBoost defensiveBoost)
    {
        defensiveBoost.gameObject.SetActive(false);
        defensiveBoostAttachment.SetActive(true);
        isInvincible = true;
        rb.freezeRotation = true;

        DOVirtual.DelayedCall(defensiveBoost.GetDefensiveDuration(), () =>
        {
            rb.freezeRotation = false;

            defensiveBoostAttachment.SetActive(false);

            isInvincible = false;
        });
    }
}
