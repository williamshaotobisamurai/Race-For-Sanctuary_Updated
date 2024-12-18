using DG.Tweening;
using NodeCanvas.BehaviourTrees;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PoliceshipBoss : EnemyBase
{
    private bool isBattleMode = false;

    [SerializeField] private Transform missileLauncher_1;
    [SerializeField] private Transform missileLauncher_2;

    [SerializeField] private Transform machineGunBarrel;
    [SerializeField] private Transform machineGun;

    [SerializeField] private MeshRenderer flashRenderer;
    [SerializeField] private Material flashMat;

    [SerializeField] private Image healthBar;
    [SerializeField] private Text healthText;

    #region machine gun

    [SerializeField] private BossGunBase bossMachineGun;
    [SerializeField] private float machineGunShootDuration = 5f;
    [SerializeField] private float machineGunCooldown = 3f;

    private float nextMachineGunShootTime;
    private float stopMachineGunShootTime;

    #endregion

    #region missile launcher

    [SerializeField] protected BossGunBase bossMissileGun_1;
    [SerializeField] protected BossGunBase bossMissileGun_2;

    [SerializeField] private float missileDuration = 5f;
    [SerializeField] private float missileGunCooldown = 3f;
    private float nextLaunchMissileTime;
    private float stopLaunchMissileTime;

    #endregion

    #region drones

    [SerializeField] private SpaceStationDroneSpawner droneSpawner;

    #endregion

    public event OnKilled OnKilledEvent;
    public delegate void OnKilled();

    [SerializeField] private GameObject sophia = null;

    private void Start()
    {
        flashMat = flashRenderer.material;
    }

    private void Update()
    {
        if (isBattleMode && !isKilled)
        {
            transform.LookAt(LevelManager.Instance.Skully.transform);
            ProcessMachineGun();
            ProcessMissileLauncher();
            ProcessDrones();
        }
    }

    private void ProcessMachineGun()
    {
        machineGun.LookAt(LevelManager.Instance.Skully.transform);

        if (Time.time > nextMachineGunShootTime && !bossMachineGun.IsFiring)
        {
            bossMachineGun.StartFiring();

            stopMachineGunShootTime = Time.time + machineGunShootDuration;
        }

        if (Time.time > stopLaunchMissileTime && bossMissileGun_1.IsFiring)
        {
            bossMachineGun.StopFiring();
            nextMachineGunShootTime = Time.time + machineGunCooldown;
        }
    }

    private void ProcessMissileLauncher()
    {
        if (Time.time > nextLaunchMissileTime && !bossMissileGun_1.IsFiring)
        {
            bossMissileGun_1.StartFiring();
            bossMissileGun_2.StartFiring();

            stopLaunchMissileTime = Time.time + missileDuration;
        }

        if (Time.time > stopLaunchMissileTime && bossMissileGun_1.IsFiring)
        {
            bossMissileGun_1.StopFiring();
            bossMissileGun_2.StopFiring();
            nextLaunchMissileTime = Time.time + missileGunCooldown;
        }
    }

    private void ProcessDrones()
    {
        if (health <= maxHealth * 0.5f && !droneSpawner.IsSpawning)
        {
            droneSpawner.StartSpawningDrone();
        }
    }

    public void MoveAndStartBattle(Transform startPos)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(missileLauncher_1.DOLocalRotate(new Vector3(-30, 0, 0), 0.8f));
        seq.Join(missileLauncher_2.DOLocalRotate(new Vector3(-30, 0, 0), 0.8f));
        seq.Append(machineGunBarrel.DOLocalMove(new Vector3(0, 0, 0.8f), 0.8f));
        seq.Append(transform.DOMove(startPos.position, 1f));
        seq.Play();
        seq.OnComplete(() =>
        {
            isBattleMode = true;
            bossHealthBar.SetActive(true);
        });
    }

    private Tween flashTween = null;

    [SerializeField] private Texture2D emissionTex;
    [SerializeField] private Texture2D hit;

    public override void TakeDamage(int damage)
    {
        if (!isBattleMode)
        {
            return;
        }

        base.TakeDamage(damage);

        if (flashTween != null)
        {
            flashTween.Kill();
            flashTween = null;
        }

        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => flashMat.SetTexture("_EmissionMap", null));
        seq.AppendInterval(0.1f);
        seq.AppendCallback(() => flashMat.SetTexture("_EmissionMap", emissionTex));
        seq.Play();
        flashTween = seq;
    }

    [SerializeField] private BehaviourTreeOwner ai;

    public override void Kill()
    {
        if (!isKilled)
        {
            ai.behaviour.Stop();
            isKilled = true;
            bossHealthBar.SetActive(false);
            droneSpawner.StopSpawningDrone();
            bossMachineGun.StopFiring();
            bossMissileGun_1.StopFiring();
            bossMissileGun_2.StopFiring();
            DestroyAllDrones();

            StartCoroutine(PlayDestroyedParticlesCoroutine(() =>
            {
                DOVirtual.DelayedCall(1f, () =>
                {
                    dialogueManaer.Play();
                });
            }));
        }
    }

    [SerializeField] private List<GameObject> destroyedParticleList;
    [SerializeField] private List<Transform> explosionTransList;
    [SerializeField] private GameObject bossHealthBar;

    [SerializeField] private DialogueManager dialogueManaer;
    [SerializeField] private SkullyBoss skullyBoss;

    public void PlayDeathParticle()
    {
        StartCoroutine(PlayDestroyedParticlesCoroutine(() =>
        {
            gameObject.SetActive(false);
            skullyBoss.gameObject.SetActive(true);
            skullyBoss.transform.position = transform.position;

            sophia.gameObject.SetActive(true);
            sophia.gameObject.transform.position = transform.position + Vector3.forward * 2 + Vector3.right * 2;
            sophia.transform.LookAt(LevelManager.Instance.Skully.transform);

            skullyBoss.gameObject.SetActive(true);

            skullyBoss.transform.position = transform.position;
            skullyBoss.transform.LookAt(LevelManager.Instance.Skully.transform);
        }));
    }

    public void TriggerOnKilledEvent()
    {
        OnKilledEvent?.Invoke();
    }

    private IEnumerator PlayDestroyedParticlesCoroutine(Action OnComplete)
    {
        int count = 0;
        while (count < 5)
        {
            RandomHelper.GetRandomItem(destroyedParticleList, out GameObject particlePrefab);
            RandomHelper.GetRandomItem(explosionTransList, out Transform explosionTrans);

            GameObject particleInstance = Instantiate(particlePrefab);

            particleInstance.transform.position = explosionTrans.position + UnityEngine.Random.onUnitSphere * 3f;
            particleInstance.transform.localScale = Vector3.one * UnityEngine.Random.Range(5, 10f);

            count++;
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.3f));
        }

        OnComplete?.Invoke();
    }

    protected override void UpdateHealthBar()
    {
        healthBar.DOFillAmount(health / (float)maxHealth, 0.5f);
        healthText.text = health.ToString() + " / " + maxHealth.ToString();
    }

    private void DestroyAllDrones()
    {
        droneSpawner.DestroyAllDrones();
        droneSpawner.StopSpawningDrone();
    }
}
