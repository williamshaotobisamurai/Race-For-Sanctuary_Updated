using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MissileSoldier : EnemyBase
{
    [SerializeField] private Animator animator;
    [SerializeField] private LookAtConstraint lookAtConstraint;

    public event OnShoot OnShootEvent;
    public delegate void OnShoot(EnemyMissile enemyMissile);

    private void Start()
    {
        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = GameManager.Instance.Skully.transform;
        source.weight = 1f;
        lookAtConstraint.AddSource(source);
    }

    protected override void LookAtSkully()
    {
         
    }

    protected override GameObject Shoot()
    {
        animator.Play("ShootSecondaryMissileLauncher");
        GameObject missileInstance = base.Shoot();
        OnShootEvent?.Invoke(missileInstance.GetComponent<EnemyMissile>());
        return missileInstance;
    }
}
