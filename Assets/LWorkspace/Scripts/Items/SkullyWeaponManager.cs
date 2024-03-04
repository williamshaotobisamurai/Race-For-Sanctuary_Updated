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
                machineGun.gameObject.SetActive(true);

                break;
            case WeaponItem.EWeaponType.MISSILE:
                break;
            default:
                break;
        }
    }
}
