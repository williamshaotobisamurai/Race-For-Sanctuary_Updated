using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skully : MonoBehaviour
{
    [SerializeField] private SkullyMovement skullyMovement;
    [SerializeField] private AudioSource collisionSound;
    [SerializeField] private Image healthBar;
    [SerializeField] private float healthAmount = 100f;

    [SerializeField] private SIRS sirs;

    public event OnSkullyDied OnSkullyDiedEvent;
    public delegate void OnSkullyDied();

    public event OnCollectCoin OnCollectCoinEvent;
    public delegate void OnCollectCoin(Coin coin);

    [SerializeField] private GameObject speedBoostAttachment;

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
        if (collisionInfo.collider.tag == "Obstacle")
        {
            collisionSound.Play();

            skullyMovement.StopRunning();
            OnSkullyDiedEvent?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(GameConstants.SPEED_BOOST))
        {
            CollectSpeedBoost(other.GetComponent<SpeedBoost>());
        }
    }

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / 100f;

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
}
