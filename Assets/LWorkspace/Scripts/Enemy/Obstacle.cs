using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private GameObject explodeParticle;

    [SerializeField] private int damage = 999;
    public int Damage { get => damage; }

    [SerializeField] private bool doDamage = false;
    public bool DoDamage => doDamage;

    public event OnMissileHit OnMissileHitEvent;
    public delegate void OnMissileHit(Obstacle obstacle);

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(GameConstants.SKILL_SPHERE))
        {
            GameObject particle = Instantiate(explodeParticle);
            particle.transform.position = transform.position;
            DOVirtual.DelayedCall(3f, () => Destroy(particle));
            gameObject.SetActive(false);
            other.GetComponentInParent<Missile>().Explode();
            OnMissileHitEvent?.Invoke(this);
        }
    }
}
 