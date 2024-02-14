using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Sniper : EnemyBase
{
    [SerializeField] private Animator animator;
    [SerializeField] private LookAtConstraint lookAtConstraint;

    [SerializeField] private LineRenderer laser;
    [SerializeField] private GameObject hitParticle;

    [SerializeField] private GameObject crosshairObj;
    [SerializeField] private GameObject aimedObj;
    [SerializeField] private float crosshairFollowSpeed = 10f;

    [SerializeField] private float aimmingDurationBeforeShoot = 3f;
    [SerializeField] private float aimmingThreshold = 0.5f;

    [SerializeField] private float aimmingDuration = 0f;

    [SerializeField] private RandomMovement aimedObjRandomMovement;

    [SerializeField] private Color laserSearchingColor;
    [SerializeField] private Color laserAimmingColor;

    protected override void LookAtSkully()
    {

    }

    private void Start()
    {
        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = GameManager.Instance.Skully.transform;
        source.weight = 1f;
        lookAtConstraint.AddSource(source);
        laser.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Skully skully = other.GetComponent<Skully>();

        if (skully != null)
        {
            crosshairObj.transform.position = skully.transform.position;
        }
    }

    protected override void AimAtSkully(Skully skully)
    {
        laser.enabled = true;
        crosshairObj.transform.position = Vector3.MoveTowards(crosshairObj.transform.position, skully.transform.position, Time.deltaTime * crosshairFollowSpeed);

        float x = Mathf.MoveTowards(crosshairObj.transform.position.x, skully.transform.position.x, Time.deltaTime * crosshairFollowSpeed);
        float y = Mathf.MoveTowards(crosshairObj.transform.position.y, skully.transform.position.y, Time.deltaTime * crosshairFollowSpeed);

        crosshairObj.transform.position = new Vector3(x, y, skully.transform.position.z);

        if (Vector3.Distance(crosshairObj.transform.position, skully.transform.position) < aimmingThreshold)
        {
            aimmingDuration += Time.deltaTime;
        }
        else
        {
            aimmingDuration = 0f;
        }
        laser.SetPosition(0, muzzle.transform.position);
        laser.SetPosition(1, aimedObj.transform.position);

        float aimmingProgress = aimmingDuration / aimmingDurationBeforeShoot;
        aimmingProgress = Mathf.Clamp(aimmingProgress, 0f, 1f);
        aimedObjRandomMovement.Range = Mathf.Lerp(1.5f, 0.25f, aimmingProgress);
        Color laserColor = Color.Lerp(laserSearchingColor, laserAimmingColor, aimmingProgress);
        laser.material.color = laserColor;
        laser.material.SetColor("_EmissionColor", laserColor);
        laser.startColor = laserColor;
        laser.endColor = laserColor;

        if (ReadyToShoot(aimmingProgress))
        {
            lastTimeShootTimeStamp = Time.time;
            Shoot();
        }
    }

    private bool ReadyToShoot(float aimmingProgress)
    {
        return aimmingProgress >= 1f && (Time.time > lastTimeShootTimeStamp + shootInterval);
    }

    private void LateUpdate()
    {
        if (transform.position.z < GameManager.Instance.Skully.transform.position.z)
        {
            laser.enabled = false;
            lastTimeShootTimeStamp = float.MaxValue;
            shootInterval = float.MaxValue;
        }

        if (transform.position.z < GameManager.Instance.Skully.transform.position.z - 10f)
        {
            gameObject.SetActive(false);
        }
    }

    protected override GameObject Shoot()
    {
        GameManager.Instance.Skully.HitBySniper();
        GameObject hitP = Instantiate(hitParticle);
        hitP.transform.position = GameManager.Instance.Skully.transform.position;
        return null;
    }
}
