using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MachineGunBullet : SkullyBulletBase
{
    [SerializeField] private float delayDestroy = 5f;

    private bool isFlying = false;
    public bool IsFlying => isFlying;

    [SerializeField] private float lifeTime = 3f;

    private void Start()
    {
        DOVirtual.DelayedCall(lifeTime, () =>
        {
            Destroy(gameObject);
        });
    }

    private void Update()
    {
        Fly();
    }

    protected virtual void Fly()
    {
        isFlying = true;
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals(GameConstants.OBSTACLE))
        {
            gameObject.SetActive(false);
            GameObject instance = Instantiate(hitParticle);
            instance.transform.position = transform.position;
        }
        else if (other.gameObject.tag.Equals(GameConstants.ENEMY_HITPOINT))
        {
            gameObject.SetActive(false);
            GameObject instance = Instantiate(hitParticle);
            instance.transform.position = transform.position;
            Vector3 normal = transform.position - other.transform.position;
            instance.transform.up = normal;
        }
        else if (other.gameObject.tag.Equals(GameConstants.SPACE_STATION))
        {
            gameObject.SetActive(false);
            GameObject instance = Instantiate(hitParticle);
            instance.transform.position = transform.position;
        }
    }
}
