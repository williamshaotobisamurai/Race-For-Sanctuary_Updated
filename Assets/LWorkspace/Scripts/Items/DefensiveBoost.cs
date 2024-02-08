using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveBoost : ItemBase
{
    [SerializeField] private float defensiveDuration;

    public float GetDefensiveDuration()
    {
        return defensiveDuration;
    }
}
