using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : ItemBase
{
    public enum EWeaponType
    { 
        NONE,
        PISTOL,
        MACHINE_GUN,
        MISSILE,
    }

    [SerializeField] private EWeaponType weaponType;
    public EWeaponType WeaponType { get { return weaponType; } }
}
