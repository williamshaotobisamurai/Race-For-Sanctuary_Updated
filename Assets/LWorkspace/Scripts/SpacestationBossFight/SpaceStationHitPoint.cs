using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStationHitPoint : MonoBehaviour
{
    [SerializeField] private int maxHealth = 500;
    [SerializeField] private int health = 500;

    [SerializeField] private bool isDestroyed = false;
    public bool IsDestroyed { get => isDestroyed; }

    [SerializeField] private GameObject destroyedParticle;

    public event OnDestroyed OnDestroyedEvent;
    public delegate void OnDestroyed(SpaceStationHitPoint hitPoint);

    [SerializeField] private MeshRenderer flashMr_1;
    [SerializeField] private MeshRenderer flashMr_2;

    [SerializeField] private Material energyBallMaterial_1;
    [SerializeField] private Material energyBallMaterial_2;

    [SerializeField] private Transform explosionTrans;

    private void Start()
    {
        energyBallMaterial_1 = flashMr_1.material;
        energyBallMaterial_2 = flashMr_2.material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(GameConstants.SKILL_SPHERE))
        {
            TakeDamage(other.GetComponentInParent<Missile>().Damage);
            other.GetComponentInParent<Missile>().HitEnemy();
        }
        else if (other.tag.Equals(GameConstants.SKULLY_BULLET))
        {
            SkullyBulletBase bullet = other.GetComponent<SkullyBulletBase>();
            TakeDamage(bullet.Damage);
            bullet.HitEnemy();
        }
    }

    private Vector2 texOffset = Vector2.zero;
    [SerializeField] private float texOffsetSpeed = 1f;

    private void Update()
    {
        if (!isDestroyed)
        {
            texOffset.y = Time.time * texOffsetSpeed;
            energyBallMaterial_1.SetTextureOffset("_FadeBurnTex", texOffset);
            energyBallMaterial_2.SetTextureOffset("_FadeBurnTex", -texOffset);
        }
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        health = maxHealth;
    }
    private bool isFlashing = false;

    private void TakeDamage(int damage)
    {
        if (!isDestroyed)
        {
            health -= damage;

            if (!isFlashing)
            {
                isFlashing = true;
                Sequence seq = DOTween.Sequence();
                seq.AppendCallback(() =>
                {
                    energyBallMaterial_1.SetFloat("_HitEffectBlend", 1f);
                    energyBallMaterial_2.SetFloat("_HitEffectBlend", 1f);
                });
                seq.AppendInterval(0.1f);
                seq.AppendCallback(() =>
                {
                    energyBallMaterial_1.SetFloat("_HitEffectBlend", 0f);
                    energyBallMaterial_2.SetFloat("_HitEffectBlend", 0f);

                });
                seq.OnComplete(() => isFlashing = false);
            }

            if (health <= 0 && !isDestroyed)
            {
                isDestroyed = true;
                StartCoroutine(Explode());
            }
        }
    }

    private IEnumerator Explode()
    {
        energyBallMaterial_1.SetFloat("_FadeBurnGlow", 5);
        energyBallMaterial_2.SetFloat("_FadeBurnGlow", 5);
        for (int i = 0; i < 5; i++)
        {
            GameObject particleInstance = Instantiate(destroyedParticle);

            particleInstance.transform.position = explosionTrans.position + Random.onUnitSphere * 3f;
            particleInstance.transform.localScale = Vector3.one * Random.Range(5, 10f);

            yield return new WaitForSeconds(0.3f);
        }

        OnDestroyedEvent?.Invoke(this);
        gameObject.SetActive(false);
    }    
}