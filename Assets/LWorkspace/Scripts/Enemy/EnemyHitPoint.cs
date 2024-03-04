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
            other.GetComponentInParent<Missile>().Explode();
            enemy.Kill();
        }
        else if (other.tag.Equals(GameConstants.SKULLY_BULLET))
        {
            enemy.TakeDamage(other.GetComponent<SkullyBulletBase>().Damage);
        }
    }
}
