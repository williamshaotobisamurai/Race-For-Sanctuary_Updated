using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private GameObject explodeParticle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(GameConstants.SKILL_SPHERE))
        {
            GameObject particle = Instantiate(explodeParticle);
            particle.transform.position = transform.position;
            DOVirtual.DelayedCall(3f, () => Destroy(particle));
            gameObject.SetActive(false);
            other.GetComponentInParent<Missile>().Explode();
        }
    }
}
