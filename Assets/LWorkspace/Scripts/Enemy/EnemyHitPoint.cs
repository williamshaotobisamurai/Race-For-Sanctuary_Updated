using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitPoint : MonoBehaviour
{
    [SerializeField] private EnemyBase enemy;
    [SerializeField] private GameObject explodeParticle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(GameConstants.SKILL_SPHERE))
        {
            if (explodeParticle != null)
            {
                GameObject particle = Instantiate(explodeParticle);
                particle.transform.position = transform.position;
                DOVirtual.DelayedCall(3f, () => Destroy(particle));
            }
            enemy.TakeDamage(other.GetComponentInParent<Missile>().Damage);            
            other.GetComponentInParent<Missile>().HitEnemy();
        }
        else if (other.tag.Equals(GameConstants.SKULLY_BULLET))
        {
            SkullyBulletBase bullet = other.GetComponent<SkullyBulletBase>();
            enemy.TakeDamage(bullet.Damage);
            bullet.HitEnemy();
        }
    }
}
