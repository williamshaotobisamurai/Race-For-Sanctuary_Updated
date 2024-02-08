using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : ItemBase
{
    [SerializeField] private float healAmount;
    public float HealAmount => healAmount;
}
