using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MachineGunSoldier : EnemyBase
{
    [SerializeField] private Animator animator;
    [SerializeField] private LookAtConstraint lookAtConstraint;

    protected void Start()
    {
        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = LevelManager.Instance.Skully.transform;
        source.weight = 1f;
        lookAtConstraint.AddSource(source);
        lookAtConstraint.roll = 90f;
        lookAtConstraint.rotationOffset = new Vector3(90, 0, 0f);
    }

    private void LateUpdate()
    {
        if (IsBlocked(LevelManager.Instance.Skully))
        {
            Debug.DrawRay(lookAtConstraint.transform.position, lookAtConstraint.transform.up * 300f, Color.red);
        }
        else
        {
            Debug.DrawRay(lookAtConstraint.transform.position, lookAtConstraint.transform.up * 300f, Color.green);
        }
    }

    protected override GameObject Shoot()
    {
        GameObject bulletInstance = GameObject.Instantiate(bullet);
        bulletInstance.transform.position = muzzle.position;

        Skully skully = LevelManager.Instance.Skully;
        Vector3 advance = Vector3.forward * advanceDistance * skully.GetCurrentVelocity().normalized.z;

        bulletInstance.transform.LookAt(
            skully.transform.position +
            advance +
            Random.insideUnitSphere * bulletSpread);
        return bulletInstance;
    }

}
