using DG.Tweening;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Boss : EnemyBase
{
    [SerializeField] private BossMachineGun bossMachineGun;
    [SerializeField] private BossMissileGun bossMissileGun;
    [SerializeField] private BossUltimateMissileManager bossUltimateMissileManager;

    [SerializeField] private Transform bossDebris;

    [SerializeField] private protected Skully skully;
    [SerializeField] protected float debrisSpread = 20f;
    [SerializeField] private float debrisSpeed = 10f;
    [SerializeField] private GameObject chargeParticle;
    [SerializeField] private GameObject expolodeParticle;

    [SerializeField] private BehaviourTreeOwner behaviourTreeOwner;
    [SerializeField] private float explodeCharge = 3f;

    [SerializeField] private GameObject bossVisual;

    [SerializeField] private LookAtConstraint chestLookAt;
    [SerializeField] private LookAtConstraint machineGunLookAt;
    [SerializeField] private LookAtConstraint missileGunLookAt;

    [SerializeField] private GameObject bossHealthBar;

    [SerializeField] private Image healthBar;
    [SerializeField] private Text healthText;

    [SerializeField] private Material flashMat;
    [SerializeField] private List<MeshRenderer> flashRenderers;

    public event OnExplode OnExplodeEvent;
    public delegate void OnExplode();

    private void Start()
    {
        skully = LevelManager.Instance.Skully;
        flashRenderers.ForEach(r => { r.material = flashMat; });
    }

    public void StartFighting()
    {
        behaviourTreeOwner.enabled = true;
        bossHealthBar.SetActive(true);
        health = maxHealth;
        UpdateHealthBar();
    }

    public void MachineGunFire()
    {
        machineGunLookAt.weight = 1f;
        bossMachineGun.StartFiring();
    }

    public void MachineGunStopFire()
    {
        machineGunLookAt.weight = 0f;
        bossMachineGun.StopFiring();
    }

    public void MissileFire()
    {
        missileGunLookAt.weight = 1f;
        bossMissileGun.StartFiring();
    }

    public void MissileStopFire()
    {
        missileGunLookAt.weight = 0f;
        bossMissileGun.StopFiring();
    }

    public void CastUltimate()
    {
        missileGunLookAt.weight = 0f;
        machineGunLookAt.weight = 0f;
        bossUltimateMissileManager.Cast();
    }

    public void Explode()
    {
        bossVisual.SetActive(false);
        expolodeParticle.SetActive(true);
        bossDebris.transform.position = transform.position;
        bossDebris.gameObject.SetActive(true);
        Transform[] debrisArr = bossDebris.GetComponentsInChildren<Transform>();
        foreach (var debris in debrisArr)
        {
            if (debris != bossDebris.transform)
            {
                debris.GetComponent<BossDebris>().Speed = Random.Range(debrisSpeed * 0.8f, debrisSpeed * 1.2f);
                debris.GetComponent<BossDebris>().FlyToPosition(skully.transform.position + new Vector3(Random.insideUnitCircle.x, Random.insideUnitCircle.y, 0) * debrisSpread);
            }
        }

        OnExplodeEvent?.Invoke();

    } 

    public override void Kill()
    {
        if (!isKilled)
        {
            isKilled = true;
            behaviourTreeOwner.enabled = false;
            chargeParticle.gameObject.SetActive(true);
            GetComponent<Animator>().enabled = false;
            transform.DOShakeRotation(explodeCharge, 10f);
            bossHealthBar.SetActive(false);      

            DOVirtual.DelayedCall(explodeCharge, () =>
            {
                Explode();
                chargeParticle.SetActive(false);
            });
            MachineGunStopFire();
            MissileStopFire();
            machineGunLookAt.weight = 0.5f;
            missileGunLookAt.weight = 0.3f;
        }
    }


    private Tween flashTween = null;

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
      //  UpdateHealthBar();

        if (flashTween != null)
        {
            flashTween.Kill();
            flashTween = null;
        }

        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() =>flashMat.EnableKeyword("_EMISSION"));
        seq.AppendInterval(0.1f);
        seq.AppendCallback(() => flashMat.DisableKeyword("_EMISSION"));        
        seq.Play();
        flashTween = seq;

    }

    protected override void UpdateHealthBar()
    {
        healthBar.DOFillAmount(health / (float)maxHealth, 0.5f);
        healthText.text = health.ToString() + " / " + maxHealth.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Kill();
        }
    }
}
