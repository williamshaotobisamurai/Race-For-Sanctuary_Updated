using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUltimateMissileManager : MonoBehaviour
{
    [SerializeField] private GameObject missilePrefab;

    [SerializeField] private Transform muzzleTrans;

    [SerializeField] private float lauchDelay = 2f;

    [SerializeField] private float initSpeed = 20f;

    [SerializeField] private float missileInterval = 0.1f;

    public void Cast()
    {
        StartCoroutine(CastCoroutine());
    }

    private IEnumerator CastCoroutine()
    {
        Transform[] muzzles = muzzleTrans.GetComponentsInChildren<Transform>();

        for (int i = 0; i < muzzles.Length; i++)
        {
            Transform muzzleForMissile = muzzles[i];
            if (muzzleForMissile == muzzleTrans)
            {
                continue;
            }

            GameObject missileInstance = Instantiate(missilePrefab, muzzles[i]);

            missileInstance.transform.position = muzzleForMissile.transform.position;

            missileInstance.transform.rotation = muzzleForMissile.transform.rotation;

            EnemyMissile enemyMissile = missileInstance.GetComponent<EnemyMissile>();

            float targetSpeed = enemyMissile.Speed;
            float targetRotateRate = enemyMissile.RotateRate;

            enemyMissile.Speed = targetSpeed * 0.25f;
            enemyMissile.RotateRate = 0f;

            missileInstance.transform.SetParent(null);
            missileInstance.transform.localScale = Vector3.one * 2.5f;

            DOVirtual.DelayedCall(lauchDelay + Random.Range(-1, 1), () =>
            {
                enemyMissile.Speed = targetSpeed * Random.Range(0.8f, 1.2f);
                enemyMissile.RotateRate = targetRotateRate * Random.Range(3f, 4f);

                DOVirtual.DelayedCall(0.5f, () =>
                {
                    enemyMissile.RotateRate = targetRotateRate * Random.Range(0.8f, 1f);

                });
            });

            yield return new WaitForSeconds(missileInterval);
        }
    }

    // Update is called once per frame
  
}

