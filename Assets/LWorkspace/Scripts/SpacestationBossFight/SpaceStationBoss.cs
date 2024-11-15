using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpaceStationBoss : MonoBehaviour
{
    [SerializeField] private SpaceStationHitPoint hitPoint_1;
    [SerializeField] private SpaceStationHitPoint hitPoint_2;
    [SerializeField] private SpaceStationHitPoint hitPoint_3;

    [SerializeField] private List<SpaceStationHitPoint> hitPointList;

    [SerializeField] private List<SpaceStationTurret> spaceStationTurretList;

    public event OnDestroyed OnDestroyedEvent;
    public delegate void OnDestroyed();

    private bool isKilled = false;

    private SpaceStationTurret T0 { get => spaceStationTurretList[0]; }
    private SpaceStationTurret T1 { get => spaceStationTurretList[1]; }
    private SpaceStationTurret T2 { get => spaceStationTurretList[2]; }
    private SpaceStationTurret T3 { get => spaceStationTurretList[3]; }
    private SpaceStationTurret T4 { get => spaceStationTurretList[4]; }
    private SpaceStationTurret T5 { get => spaceStationTurretList[5]; }
    private SpaceStationTurret T6 { get => spaceStationTurretList[6]; }
    private SpaceStationTurret T7 { get => spaceStationTurretList[7]; }

    private void Start()
    {
        hitPointList.Add(hitPoint_1);
        hitPointList.Add(hitPoint_2);
        hitPointList.Add(hitPoint_3);

        hitPoint_1.OnDestroyedEvent += HitPoint_OnDetroyedEvent;
        hitPoint_2.OnDestroyedEvent += HitPoint_OnDetroyedEvent;
        hitPoint_3.OnDestroyedEvent += HitPoint_OnDetroyedEvent;
    }

    private void HitPoint_OnDetroyedEvent(SpaceStationHitPoint hitPoint)
    {
        hitPointList.ForEach((h) =>
        {
            if (!h.IsDestroyed)
            {
                h.IncreaseMaxHealth(200);
            }
        });

        List<SpaceStationHitPoint> list = hitPointList.FindAll(t => !t.IsDestroyed);

        if (list.Count == 2)
        {
            EnterFirstPhase();
            DOVirtual.DelayedCall(2f, () => enableBattleMode = true);
        }
        else if (list.Count == 1)
        {
            ReleaseDrones();
        }
        else if (list.Count == 0)
        {
            BossDefeated();
        }
    }

    private void BossDefeated()
    {
        StopFiring(new List<SpaceStationTurret>() { T0, T1, T2, T3, T4, T5, T6, T7 });
        isKilled = true;
        DestroyAllDrones();
        PlayDestroyedParticles(() =>
        {
            OnDestroyedEvent?.Invoke();
        });
    }

    private bool enableBattleMode;

    private float nextSwitchPatternTimeStamp;

    private void StopCurrentPattern()
    {
        patternTweenList.ForEach(p =>
        {
            if (p != null)
            {
                p.Kill();
            }
        });

        patternTweenList.Clear();
    }

    private void Update()
    {
        if (enableBattleMode && !isKilled)
        {
            if (Time.time > nextSwitchPatternTimeStamp)
            {
                nextSwitchPatternTimeStamp = Time.time + 3f;
                StopCurrentPattern();
                int ran = UnityEngine.Random.Range(0, 8);
                switch (ran)
                {
                    case 0:
                        TurretsPlayPattern_1();
                        break;
                    case 1:
                        TurretsPlayPattern_2();
                        break;

                    case 2:
                        TurretsPlayPattern_3();
                        break;

                    case 3:
                        TurretsPlayPattern_4();
                        break;

                    case 4:
                        TurretsPlayPattern_5();
                        break;

                    default:
                        TurretsPlayPattern_2();
                        break;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            BossDefeated();
        }
    }

    public void EnterFirstPhase()
    {
        spaceStationTurretList.ForEach(t =>
        {
            t.MoveOut();
        });
    }

    private List<Tween> patternTweenList = new List<Tween>();

    private void TurretsPlayPattern_1()
    {
        Sequence seq_Top = DOTween.Sequence();
        seq_Top.AppendCallback(() =>
        {
            StartFiring(new List<SpaceStationTurret>() { T0, T1, T2, T3 });
            T0.TurretTrans.DOLocalRotate(new Vector3(0f, -45f, 0f), 0.5f);
            T2.TurretTrans.DOLocalRotate(new Vector3(0f, -45f, 0f), 0.5f);
            T1.TurretTrans.DOLocalRotate(new Vector3(0f, 45f, 0f), 0.5f);
            T3.TurretTrans.DOLocalRotate(new Vector3(0f, 45f, 0f), 0.5f);
        });
        seq_Top.AppendInterval(0.5f);
        seq_Top.AppendCallback(() =>
        {
            T0.TurretTrans.DOLocalRotate(new Vector3(0f, 45f, 0f), 1f);
            T2.TurretTrans.DOLocalRotate(new Vector3(0f, 45f, 0f), 1f);
            T1.TurretTrans.DOLocalRotate(new Vector3(0f, -45f, 0f), 1f);
            T3.TurretTrans.DOLocalRotate(new Vector3(0f, -45f, 0f), 1f);
        });
        seq_Top.AppendInterval(1f);
        seq_Top.AppendCallback(() => StopFiring(new List<SpaceStationTurret>() { T0, T1, T2, T3 }));
        seq_Top.Play();

        Sequence seq_bottom = DOTween.Sequence();
        seq_bottom.AppendInterval(1f);
        seq_bottom.AppendCallback(() =>
        {
            StartFiring(new List<SpaceStationTurret>() { T4, T5, T6, T7 });
            T4.TurretTrans.DOLocalRotate(new Vector3(0f, -45f, 0f), 0.5f);
            T6.TurretTrans.DOLocalRotate(new Vector3(0f, -45f, 0f), 0.5f);
            T5.TurretTrans.DOLocalRotate(new Vector3(0f, 45f, 0f), 0.5f);
            T7.TurretTrans.DOLocalRotate(new Vector3(0f, 45f, 0f), 0.5f);
        });
        seq_bottom.AppendInterval(0.5f);
        seq_bottom.AppendCallback(() =>
        {
            T4.TurretTrans.DOLocalRotate(new Vector3(0f, 45f, 0f), 1f);
            T6.TurretTrans.DOLocalRotate(new Vector3(0f, 45f, 0f), 1f);
            T5.TurretTrans.DOLocalRotate(new Vector3(0f, -45f, 0f), 1f);
            T7.TurretTrans.DOLocalRotate(new Vector3(0f, -45f, 0f), 1f);
        });
        seq_bottom.AppendInterval(1f);

        seq_bottom.AppendCallback(() => StopFiring(new List<SpaceStationTurret>() { T4, T6, T5, T7 }));
        seq_bottom.Play();

        patternTweenList.Add(seq_Top);
        patternTweenList.Add(seq_bottom);
    }

    private void TurretsPlayPattern_2()
    {
        List<SpaceStationTurret> autoAimList = new List<SpaceStationTurret>() { T2, T3, T4, T5 };
        EnableAutoAim(autoAimList);

        List<SpaceStationTurret> rotatingTurretList = new List<SpaceStationTurret>() { T0, T1, T6, T7 };
        Sequence seq_out = DOTween.Sequence();
        seq_out.AppendCallback(() =>
        {
            StartFiring(rotatingTurretList);
            T0.TurretTrans.DOLocalRotate(new Vector3(0f, 45f, 0f), 0.5f);
            T1.TurretTrans.DOLocalRotate(new Vector3(0f, -45f, 0f), 0.5f);
            T6.TurretTrans.DOLocalRotate(new Vector3(0f, 45f, 0f), 0.5f);
            T7.TurretTrans.DOLocalRotate(new Vector3(0f, -45f, 0f), 0.5f);
        });

        seq_out.AppendInterval(1f);
        seq_out.AppendCallback(() =>
        {
            StartFiring(new List<SpaceStationTurret>() { T0, T1, T6, T7 });
            T0.TurretTrans.DOLocalRotate(Vector3.zero, 0.5f);
            T1.TurretTrans.DOLocalRotate(Vector3.zero, 0.5f);
            T6.TurretTrans.DOLocalRotate(Vector3.zero, 0.5f);
            T7.TurretTrans.DOLocalRotate(Vector3.zero, 0.5f);

        });
        seq_out.AppendInterval(1f);
        seq_out.AppendCallback(() =>
        {
            DisableAutoAim(autoAimList);
            StopFiring(rotatingTurretList);
        });

        patternTweenList.Add(seq_out);
    }

    private void TurretsPlayPattern_3()
    {
        List<SpaceStationTurret> left = new List<SpaceStationTurret>() { T0, T2, T4, T6 };
        List<SpaceStationTurret> right = new List<SpaceStationTurret>() { T1, T3, T5, T7 };

        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() =>
        {
            StartFiring(left);
            StartFiring(right);
            left.ForEach(t =>
            {
                t.TurretTrans.DOLocalRotate(new Vector3(0, 60f, 0f), 0.5f);
            });

            right.ForEach(t =>
            {
                t.TurretTrans.DOLocalRotate(new Vector3(0, -60f, 0f), 0.5f);
            });
        });
        seq.AppendInterval(0.5f);
        seq.AppendCallback(() =>
        {
            StartFiring(left);
            left.ForEach(t =>
            {
                t.TurretTrans.DOLocalRotate(new Vector3(0, -60f, 0f), 1f);
            });

            right.ForEach(t =>
            {
                t.TurretTrans.DOLocalRotate(new Vector3(0, 60f, 0f), 1f);
            });
        });
        seq.AppendInterval(1f);
        seq.SetLoops(2);
        seq.OnComplete(() =>
            {
                StopFiring(left);
                StopFiring(right);
            });

        patternTweenList.Add(seq);
    }

    private void TurretsPlayPattern_4()
    {
        List<SpaceStationTurret> turretList = new List<SpaceStationTurret>() { T0, T2, T4, T6, T1, T3, T5, T7 };

        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() =>
        {
            StartFiring(turretList);
            turretList.ForEach(t =>
            {
                t.TurretTrans.DOLocalRotate(new Vector3(0, 50f, 0f), 1f);
            });
        });

        seq.AppendInterval(1f);
        seq.AppendCallback(() =>
        {
            turretList.ForEach(t =>
            {
                t.TurretTrans.DOLocalRotate(new Vector3(0, -50f, 0f), 1f);
            });
        });

        seq.AppendInterval(1f);
        seq.SetLoops(2);

        patternTweenList.Add(seq);

    }

    private void TurretsPlayPattern_5()
    {
        List<SpaceStationTurret> turretList = new List<SpaceStationTurret>() { T0, T2, T4, T6, T1, T3, T5, T7 };

        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() =>
        {
            StartFiring(turretList);
            turretList.ForEach(t =>
            {
                t.TurretTrans.DOLocalRotate(new Vector3(50, 0f, 0f), 1f);
            });
        });

        seq.AppendInterval(1f);
        seq.AppendCallback(() =>
        {
            turretList.ForEach(t =>
            {
                t.TurretTrans.DOLocalRotate(new Vector3(-50, 0f, 0f), 1f);
            });
        });

        seq.AppendInterval(1f);
        seq.SetLoops(2);

        patternTweenList.Add(seq);
    }

    private void EnableAutoAim(List<SpaceStationTurret> list)
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() =>
        {
            StartFiring(list);
            list.ForEach(t => t.StartAutoAim());
        });

        patternTweenList.Add(seq);
    }

    private void DisableAutoAim(List<SpaceStationTurret> list)
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() =>
        {
            StartFiring(list);
            list.ForEach(t => t.StopAutoAim());
        });
        seq.AppendCallback(() =>
        {
            list.ForEach(t => t.TurretTrans.DOLocalRotate(Vector3.zero, 0.5f));
        });
        seq.AppendInterval(1f);
        seq.AppendCallback(() =>
        {
            StopFiring(list);
        });

        patternTweenList.Add(seq);
    }

    private void StartFiring(List<SpaceStationTurret> list)
    {
        list.ForEach(t => { t.StartFiring(); });
    }

    private void StopFiring(List<SpaceStationTurret> list)
    {
        list.ForEach(t => { t.StopFiring(); });
    }


    [SerializeField] private SpaceStationDroneSpawner droneSpawner;

    private void ReleaseDrones()
    {
        droneSpawner.StartSpawningDrone();
    }

    [SerializeField] private List<GameObject> destroyedParticleList;
    [SerializeField] private List<Transform> explosionTransList;

    private void PlayDestroyedParticles(Action OnComplete)
    {
        StartCoroutine(PlayDestroyedParticlesCoroutine(OnComplete));
    }

    private IEnumerator PlayDestroyedParticlesCoroutine(Action OnComplete)
    {
        int count = 0;
        while (count < 20)
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

    private void DestroyAllDrones()
    {
        droneSpawner.DestroyAllDrones();
        droneSpawner.StopSpawningDrone();
    }
}
