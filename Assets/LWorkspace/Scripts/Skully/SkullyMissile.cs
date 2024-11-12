using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class SkullyMissile : MonoBehaviour
{
    [SerializeField] private float coolDown = 3f;
    private const int rayLength = 1000;
    private float lastCastTimeStamp;

    [SerializeField] private float aimingDuration = 3f;
    [SerializeField] private SIRS SIRS;
    [SerializeField] private Missile missilePrefab;

    private Missile missileInstance;

    [SerializeField] private AudioSource reloadAudio;

    public event OnLaunchMissile OnLaunchMissileEvent;
    public delegate void OnLaunchMissile(Missile missile);


    private void OnEnable()
    {
        reloadAudio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= lastCastTimeStamp + coolDown)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (missileInstance == null)
            {
                PrepareMissile();              
            }
            else
            {
                missileInstance.transform.LookAt(ray.origin + ray.direction * rayLength);
                if (Input.GetMouseButton(0))
                {
                    lastCastTimeStamp = Time.time;
                    Shoot(ray);
                }
            }
        }
    }

    public void PrepareMissile()
    {
        if (missileInstance != null)
        {
            Destroy(missileInstance);
        }

        missileInstance = Instantiate(missilePrefab);
        missileInstance.transform.localScale = Vector3.zero;
        missileInstance.AttachTo(SIRS.MissileAttachedTrans);
        missileInstance.transform.DOScale(missilePrefab.transform.localScale,0.2f);
    }

    private void Shoot(Ray ray)
    {
        OnLaunchMissileEvent?.Invoke(missileInstance);
        missileInstance.transform.LookAt(ray.origin + ray.direction * rayLength);
        missileInstance.Launch();
        missileInstance = null;

    }

    public void TurnOff()
    {
        if (missileInstance != null)
        {
            missileInstance.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
            {
                Destroy(missileInstance.gameObject);
                gameObject.SetActive(false);
            });
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
