using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveBoost : MonoBehaviour
{
    [SerializeField] private float defensiveDuration;

    public float GetDefensiveDuration()
    {
        return defensiveDuration;
    }
}
