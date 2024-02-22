using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : ItemBase
{
    [SerializeField] private int healAmount;
    public int HealAmount => healAmount;
}
