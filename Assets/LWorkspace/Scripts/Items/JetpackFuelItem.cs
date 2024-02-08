using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackFuelItem : ItemBase
{
    [SerializeField] private float increaseAmount;
    public float IncreaseAmount => increaseAmount;
}

