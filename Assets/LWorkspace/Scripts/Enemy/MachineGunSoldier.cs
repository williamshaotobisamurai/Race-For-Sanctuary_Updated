using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MachineGunSoldier : EnemyBase
{
    [SerializeField] private Animator animator;
    [SerializeField] private LookAtConstraint lookAtConstraint;

    [SerializeField] private float bulletSpread = 2f;

    protected override void Start()
    {
        base.Start();
        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = GameManager.Instance.Skully.transform;
        source.weight = 1f;
        lookAtConstraint.AddSource(source);
    }

    private void LateUpdate()
    {
        Debug.DrawRay(lookAtConstraint.transform.position, lookAtConstraint.transform.up * 300f, Color.green);
    }

    protected override GameObject Shoot()
    {
        GameObject bulletInstance = GameObject.Instantiate(bullet);
        bulletInstance.transform.position = muzzle.position;

        Skully skully = GameManager.Instance.Skully;
        Vector3 advance = Vector3.forward * advanceDistance * skully.GetCurrentVelocity().normalized.z;

        bulletInstance.transform.LookAt(
            skully.transform.position +
            advance +
            Random.insideUnitSphere * bulletSpread);
        return bulletInstance;
    }

}
