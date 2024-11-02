using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullyWeaponManager : MonoBehaviour
{
    [SerializeField] private Crosshair crosshair;
    [SerializeField] private LayerMask targetsLayer;
    public LayerMask TargetsLayer { get => targetsLayer; }

    [SerializeField] private SkullyPistolGun skullyPistol;
    [SerializeField] private SkullyMachineGun machineGun;
    [SerializeField] private SkullyMissile skullyMissile;

    private static SkullyWeaponManager instance;
    public static SkullyWeaponManager Instance { get => instance; }

    private WeaponItem.EWeaponType currentWeaponType = WeaponItem.EWeaponType.NONE;
    public WeaponItem.EWeaponType CurrentWeapon { get => currentWeaponType; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance.gameObject);
        }
    }

    private void Start()
    {
        skullyPistol.OnShootBulletEvent += SkullyPistol_OnShootBulletEvent;
        machineGun.OnShootBulletEvent += MachineGun_OnShootBulletEvent;
        skullyMissile.OnLaunchMissileEvent += SkullyMissile_OnLaunchMissileEvent;
    }

    private void SkullyMissile_OnLaunchMissileEvent(Missile missile)
    {
        missile.OnHitEnemyEvent += crosshair.OnHitEnemy;
    }

    private void MachineGun_OnShootBulletEvent(SkullyBulletBase bulletBase)
    {
        bulletBase.OnHitEnemyEvent += crosshair.OnHitEnemy;
    }

    private void SkullyPistol_OnShootBulletEvent(SkullyBulletBase bulletBase)
    {
        bulletBase.OnHitEnemyEvent += crosshair.OnHitEnemy;
    }

    public void ShowCrosshair()
    {
        crosshair.Show();
    }

    public void HideCrosshair()
    {
        crosshair.Hide();
    }

    public void SetupWeapon(WeaponItem.EWeaponType weaponType)
    {
        if (currentWeaponType != weaponType)
        {
            switch (currentWeaponType)
            {
                case WeaponItem.EWeaponType.NONE:
                    break;

                case WeaponItem.EWeaponType.PISTOL:
                    TurnOffPistol();
                    break;

                case WeaponItem.EWeaponType.MACHINE_GUN:
                    TurnOffMachineGun();
                    break;

                case WeaponItem.EWeaponType.MISSILE:
                    TurnOffMissile();
                    break;

                default:
                    break;
            }

            currentWeaponType = weaponType;
            switch (weaponType)
            {
                case WeaponItem.EWeaponType.PISTOL:
                    PistolPopup();
                    ShowCrosshair();
                    break;

                case WeaponItem.EWeaponType.MACHINE_GUN:
                    MachineGunPopup();
                    ShowCrosshair();
                    break;

                case WeaponItem.EWeaponType.MISSILE:
                    MissilePopup();
                    ShowCrosshair();
                    break;

                default:
                    break;
            }
        }
    }

    private void PistolPopup()
    {
        if (!skullyPistol.isActiveAndEnabled)
        {
            skullyPistol.gameObject.SetActive(true);
            skullyPistol.transform.localScale = Vector3.zero;

            skullyPistol.enabled = false;
            skullyPistol.transform.DOScale(Vector3.one, 0.3f).OnComplete(() =>
            {
                skullyPistol.enabled = true;
            });
        }
    }

    private void MachineGunPopup()
    {
        if (!machineGun.isActiveAndEnabled)
        {
            machineGun.gameObject.SetActive(true);
            machineGun.transform.localScale = Vector3.zero;

            machineGun.enabled = false;
            machineGun.transform.DOScale(Vector3.one, 0.3f).OnComplete(() =>
            {
                machineGun.enabled = true;
            });
        }
    }

    private void MissilePopup()
    {
        if (!skullyMissile.isActiveAndEnabled)
        {
            skullyMissile.gameObject.SetActive(true);
            skullyMissile.PrepareMissile();
        }
    }

    private void TurnOffMachineGun()
    {
        if (machineGun.isActiveAndEnabled)
        {
            machineGun.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
            {
                machineGun.enabled = false;
                machineGun.gameObject.SetActive(false);
            });
        }
    }

    private void TurnOffMissile()
    {
        if (skullyMissile.isActiveAndEnabled)
        {
            skullyMissile.TurnOff();
        }
    }

    private void TurnOffPistol()
    {
        if (skullyPistol.isActiveAndEnabled)
        {
            skullyPistol.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
            {
                skullyPistol.enabled = false;
                skullyPistol.gameObject.SetActive(false);
            });
        }
    }
}
