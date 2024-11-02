using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoritesCollide : MonoBehaviour
{
    [SerializeField] private Transform endPoint;

    [SerializeField] private CollidingMeteorite meteoriteA;
    [SerializeField] private CollidingMeteorite meteoriteB;

    [SerializeField] private GameObject fireBallPrefab;


    [SerializeField] private float startRadius = 5f;
    [SerializeField] private float endRadius = 30f;

    [SerializeField] private int fireBallCount = 15;

    [SerializeField] private List<GameObject> fireBallList;

    [SerializeField] private Transform fireballRoot;
    [SerializeField] private float hitPlayerTime = 2f;

    [SerializeField] private float lifeTime = 3f;

    [SerializeField] private ParticleSystem explodeParticle;

    private void Start()
    {
        meteoriteA.OnHitByOtherMeteoriteEvent += MeteoriteA_OnHitByOtherMeteoriteEvent;
        fireballRoot.transform.position = endPoint.transform.position;
        for (int i = 0; i < fireBallCount; i++)
        {
            GameObject fireball = Instantiate(fireBallPrefab, fireballRoot);
            fireball.transform.localPosition = Random.insideUnitSphere * startRadius;
            fireball.transform.localScale = Vector3.one * Random.Range(3f, 10f);
            fireball.gameObject.SetActive(false);
            fireBallList.Add(fireball);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartPlaying();
        }
    }

    private void OnDestroy()
    {
        meteoriteA.OnHitByOtherMeteoriteEvent -= MeteoriteA_OnHitByOtherMeteoriteEvent;
    }

    private void MeteoriteA_OnHitByOtherMeteoriteEvent(CollidingMeteorite a, CollidingMeteorite b)
    {
        explodeParticle.Play();
        a.gameObject.SetActive(false);
        b.gameObject.SetActive(false);

        fireBallList.ForEach(t =>
        {
            t.SetActive(true);
            t.transform.LookAt(LevelManager.Instance.Skully.transform);
            Vector2 targetPosition = Random.insideUnitCircle * endRadius;
            t.transform.DOMove(LevelManager.Instance.Skully.transform.position + Vector3.back * 2 + new Vector3(targetPosition.x, targetPosition.y, 0f), hitPlayerTime * t.transform.localScale.x * 0.2f).SetEase(Ease.Linear);
        });

        DOVirtual.DelayedCall(lifeTime, () =>
        {
            Destroy(gameObject);
        });
    }

    public void StartPlaying()
    {
        meteoriteA.transform.DOMove(endPoint.transform.position, 2f);
        meteoriteB.transform.DOMove(endPoint.transform.position, 2f);
    }
}
