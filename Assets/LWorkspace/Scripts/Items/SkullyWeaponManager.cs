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


    [SerializeField] private SkullyMachineGun machineGun;
    [SerializeField] private SkullyMissile skullyMissile;

    private static SkullyWeaponManager instance;
    public static SkullyWeaponManager Instance { get => instance; }

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

    public void ShowCrosshair()
    {
        crosshair.Show();
    }

    public void HideCrosshair()
    {
        crosshair.Hide();
    }

    public void SetupWeapon(WeaponItem weaponItem)
    {
        ShowCrosshair();

        switch (weaponItem.WeaponType)
        {
            case WeaponItem.EWeaponType.MACHINE_GUN:
                TurnOffMissile();
                MachineGunPopup();
                break;
            case WeaponItem.EWeaponType.MISSILE:
                TurnOffMachineGun();
                MissilePopup();
                break;
            default:
                break;
    }        }


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
}
