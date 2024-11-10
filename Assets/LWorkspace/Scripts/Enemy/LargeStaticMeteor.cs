using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeStaticMeteor : MonoBehaviour
{
    [SerializeField] private GameObject explodeParticle;

    [SerializeField] private int damage = 999;
    public int Damage { get => damage; }

    [SerializeField] private bool doDamage = false;
    public bool DoDamage => doDamage;

    public event OnMissileHit OnMissileHitEvent;
    public delegate void OnMissileHit(LargeStaticMeteor obstacle);

    [SerializeField] private float explodeParticleScale;

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
        else if (other.tag.Equals(GameConstants.METEOR))
        {
            Debug.Log("on hit by meteor");
            other.gameObject.SetActive(false);
            if (transform.position.z > LevelManager.Instance.Skully.transform.position.z && other.GetComponent<MeteorMovement>().CanCauseExplode)
            {
                Explode();
            }
        }
    }

    [SerializeField] private int spawnCount = 10;
    [SerializeField] private int spawnCountVariation = 1;

    [SerializeField] private List<GameObject> obstaclesList;
    [SerializeField] private float radius = 10;

    private void Explode()
    {
        GameObject particle = Instantiate(explodeParticle);
        particle.transform.localScale = explodeParticleScale * Vector3.one;
        particle.transform.position = transform.position;
        DOVirtual.DelayedCall(3f, () => Destroy(particle));
        gameObject.SetActive(false);

        int count = spawnCount;

        for (int i = 0; i < count; i++)
        {
            RandomHelper.GetRandomItem(obstaclesList, out GameObject prefab);
            GameObject go = Instantiate(prefab);
            Vector3 ran = Random.insideUnitSphere * radius + transform.position;
            ran.z = transform.position.z;
            go.transform.position = ran;
            go.GetComponent<MeteorMovement>().Init(transform.position + Random.onUnitSphere * 150f);
        }
    }
}
