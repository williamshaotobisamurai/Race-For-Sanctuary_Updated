using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skully : MonoBehaviour
{
    [SerializeField] private SkullyMovement skullyMovement;
    [SerializeField] private AudioSource collisionSound;
    [SerializeField] private Image healthBar;
    [SerializeField] private float healthAmount = 100f;

    public event OnSkullyDied OnSkullyDiedEvent;
    public delegate void OnSkullyDied();


    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Obstacle")
        {
            collisionSound.Play();

            skullyMovement.StopRunning();
            OnSkullyDiedEvent?.Invoke();
        }
    }

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / 100f;

    }
    public void Heal(float healingAmount)
    {
        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);

        healthBar.fillAmount = healthAmount / 100f;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
