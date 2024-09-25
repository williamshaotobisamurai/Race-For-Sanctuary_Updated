using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class WallTurret : MonoBehaviour
{
    [SerializeField] private Transform crosshairRoot;

    [SerializeField] private GameObject crosshairPrefab;

    [SerializeField] private WallTurretCrosshair crosshairUIInstance;

    [SerializeField] private Skully skully;

    [SerializeField] private Transform wallTurretMuzzleRoot;
    [SerializeField] private Transform turretMuzzle;
    [SerializeField] private float rotateSpeed = 10f;

    [SerializeField] private GameObject crosshairObj;
    [SerializeField] private float crosshairFollowSpeed = 50f;

    [SerializeField] private float interval = 2f;
    [SerializeField] private float shootDuration = 2f;

    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] PlayerDetector playerDetector;

    [SerializeField] private ParticleSystem muzzleParticle;

    private void Start()
    {
        playerDetector.OnPlayeEnterEvent += OnPlayerDetected;
        playerDetector.OnPlayerLeftEvent += OnPlayerLeft;
    }

    private void OnPlayerLeft(Skully skully)
    {
        this.skully = null;
        StopShooting();
        Destroy(crosshairUIInstance.gameObject);
    }

    private void OnPlayerDetected(Skully skully)
    {
        Debug.Log("create a new crosshair");
        crosshairUIInstance = Instantiate(crosshairPrefab, crosshairRoot).GetComponent<WallTurretCrosshair>();
        this.skully = skully;
        skully.SetMaxSpeedFactor(0.5f);

        StartShooting();        
    }


    private void LateUpdate()
    {
        if (skully != null)
        {
            crosshairObj.transform.position = Vector3.MoveTowards(crosshairObj.transform.position, skully.transform.position + Vector3.up, Time.deltaTime * crosshairFollowSpeed);

            float x = crosshairObj.transform.position.x;
            float y = crosshairObj.transform.position.y;

            crosshairObj.transform.position = new Vector3(x, y, skully.transform.position.z);

            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, crosshairObj.transform.position);
            crosshairUIInstance.GetComponent<RectTransform>().position = screenPoint;

            wallTurretMuzzleRoot.LookAt(crosshairObj.transform.position);

            Ray ray = new Ray(wallTurretMuzzleRoot.position, wallTurretMuzzleRoot.forward);

            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green);
        }
    }

    private bool isShooting = false;

    private Coroutine shootingCoroutine;

    private void StartShooting()
    {
        isShooting = true;

        StopShootingCoroutine();

        shootingCoroutine = StartCoroutine(Shooting());
    }

    private void StopShootingCoroutine()
    {
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
    }

    private IEnumerator Shooting()
    {
        while (true)
        {
            if (IsSkullyInFront())
            {
                float shootingTime = 0f;
                while (isShooting && shootingTime < shootDuration)
                {
                    Debug.Log("shooting " + Time.time);
                    GameObject bulletInstance = Instantiate(bulletPrefab);
                    bulletInstance.transform.position = turretMuzzle.position;
                    bulletInstance.transform.LookAt(crosshairObj.transform.position);
                    shootingTime += 0.05f;
                    crosshairUIInstance.SetFiringColor();
                    muzzleParticle.Play();
                    yield return new WaitForSeconds(0.05f);
                    crosshairUIInstance.SetAimmingColor();
                    muzzleParticle.Stop();

                }
            }
            crosshairUIInstance.SetAimmingColor();
            yield return new WaitForSeconds(2f);
        }
    }

    private bool IsSkullyInFront()
    {
        if (skully != null)
        {
            return skully.transform.position.z < transform.position.z;
        }
        return false;
    }

    private void StopShooting()
    {
        isShooting = false;
        muzzleParticle.Stop();
        StopShootingCoroutine();
    } 
}
