using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolingItem : ItemBase
{
    [SerializeField] private float coolingAmount;
    public float CoolingAmount => coolingAmount;
}
