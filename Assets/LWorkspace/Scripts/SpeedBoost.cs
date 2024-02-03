using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    [SerializeField] private float speedUpDuration;
    [SerializeField] private float speedBoostFactor;
    private AudioSource collisionSound;

    void Start() {
        collisionSound = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Skully>() != null)
        {
            collisionSound.Play();
        }
    }
    public float GetSpeedUpDuration()
    {
        return speedUpDuration;
    }

    public float GetSpeedBoostFactor()
    {
        return speedBoostFactor;
    }
   
}
