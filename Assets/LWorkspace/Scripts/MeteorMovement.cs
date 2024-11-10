using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorMovement : MonoBehaviour
{
    private Vector3 targetPos;
    private Vector3 direction;

    [SerializeField] private bool canCauseExplode = false;
    public bool CanCauseExplode { get => canCauseExplode;  }

    [SerializeField] private Transform particleRoot;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private float minSpeed = 800f;
    [SerializeField] private float maxSpeed = 1600f;

    [SerializeField] private int damage = 20;
    public int Damage { get => damage; }

    private float speed = 800f;

    private float lifeTime = 20f;


    public void Init(Vector3 position)
    {
        
        transform.localEulerAngles = Random.insideUnitSphere * 360f;
        transform.localScale = transform.localScale * Random.Range(0.2f, 3f);
        this.targetPos = position;

        direction = (this.targetPos - transform.position).normalized;    

        speed = Random.Range(minSpeed, maxSpeed);
    }

    private void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime < 0)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
            particleRoot.transform.LookAt(transform.position + direction);
        }
    }
}
