using DG.Tweening;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering;

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

    private void Start()
    {
        skully = GameManager.Instance.Skully;
    }
    public void ShootBulletsForSeconds()
    {

    }

    public void MoveRandomly()
    {

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

        DOVirtual.DelayedCall(2f, () =>
        {
            Time.timeScale = 0.5f;
        });
    }

    public override void Kill()
    {
        behaviourTreeOwner.enabled = false;
        chargeParticle.gameObject.SetActive(true);
        transform.DOShakeRotation(explodeCharge,10f);

        DOVirtual.DelayedCall(1f, () =>
        {
            DOVirtual.Float(1, 0.5f, 1f, (f) =>
            {
                Time.timeScale = f;
            });
        });   

        DOVirtual.DelayedCall(explodeCharge, () =>
        {
            Explode();
            chargeParticle.SetActive(false);
        },ignoreTimeScale: true);
        chestLookAt.weight = 0f;
        MachineGunStopFire();
        MissileStopFire();
        machineGunLookAt.weight = 0.5f;
        missileGunLookAt.weight = 0.3f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Kill();
        }
    }
}
