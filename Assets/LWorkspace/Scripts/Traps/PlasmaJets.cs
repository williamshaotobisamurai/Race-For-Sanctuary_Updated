using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaJets : MonoBehaviour
{
    [SerializeField] private float castInterval;
    [SerializeField] private float damageInterval = 0.5f;
    [SerializeField] private int maxCastTime;

    private float lastCastTimestamp = 0;

    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private GameObject particlePrefab;

    [SerializeField] private int damage;

    private bool isDamaging = false;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(Random.Range(0, 5f));
        StartCoroutine(StartCasting());
    }

    private IEnumerator StartCasting()
    {
        while (true)
        {
            int castTime = 0;

            while (castTime < maxCastTime)
            {
                castTime++;

                EnableDamage();
                GameObject particleInstance = Instantiate(particlePrefab, transform);
                particleInstance.transform.localPosition = Vector3.zero;
                yield return new WaitForSeconds(0.3f);

                DisableDamage();

                yield return new WaitForSeconds(0.3f);
            }

            yield return new WaitForSeconds(castInterval);
        }
    }

    public void EnableDamage()
    {
        isDamaging = true;
        meshRenderer.material.color = Color.red;
    }

    public void DisableDamage()
    {
        isDamaging = false;
        meshRenderer.material.color = Color.white;
    }

    private float lastDamageTimestamp = 0f;

    private void OnTriggerStay(Collider other)
    {
        if (IsReadyToDamage() && GameHelper.IsSkully(other, out Skully skully))
        {
            lastDamageTimestamp = Time.time;
            skully.TakeDamage(damage);
        }
    }

    private bool IsReadyToDamage()
    {
        return isDamaging && Time.time > lastDamageTimestamp + damageInterval;
    }
    private void Update()
    {
        if (Time.time > lastCastTimestamp + castInterval)
        {

        }
    }
}
