using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyField : MonoBehaviour
{
    [SerializeField] private GameObject particleEffect;
    [SerializeField] private GameObject particleEffect1;

    [SerializeField] private float interval = 0.5f;
    [SerializeField] private BoxCollider boxCollider;
    

    private void Start()
    {
        StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        int i = 0;
        while (true)
        {
            i++;
            GameObject particle = Instantiate(particleEffect, transform);
            particle.transform.localPosition = RandomPointInBox(boxCollider.center, boxCollider.size);
            particle.transform.localScale = Vector3.one * Random.Range(1, 3f);

            GameObject particle1 = Instantiate(particleEffect1, transform);
            particle1.transform.localPosition = RandomPointInBox(boxCollider.center, boxCollider.size);
            particle1.transform.localScale = Vector3.one * Random.Range(1, 3f);

            yield return new WaitForSeconds(interval);

            DOVirtual.DelayedCall(interval, () => { Destroy(particle); });
            DOVirtual.DelayedCall(interval, () => { Destroy(particle1); });
        }
    }

    private static Vector3 RandomPointInBox(Vector3 center, Vector3 size)
    {
        return center + new Vector3(
           (Random.value - 0.5f) * size.x,
           (Random.value - 0.5f) * size.y,
           (Random.value - 0.5f) * size.z
        );
    }
}
