using DG.Tweening;
using System;
using UnityEngine;

public class SpaceStationTurret : MonoBehaviour
{
    [SerializeField] private Transform turretTrans;
    public Transform TurretTrans { get => turretTrans; }

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private ParticleSystem muzzleParticle;
    [SerializeField] protected Transform muzzle;

    [SerializeField] private float rotateSpeed = 10f;


    [SerializeField] protected float lastTimeShootTimeStamp = 0f;
    [SerializeField] protected float shootInterval = 0.5f;

    private bool isFiring = false;

    [SerializeField] private AudioSource audioSrc;
    [SerializeField] private AudioClip shootAudio;

    public void MoveOut()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(UnityEngine.Random.Range(0f, 0.5f));
        seq.Append(turretTrans.DOLocalMoveZ(0, 1f));
        seq.Play();
        audioSrc.Play();
    }

    private void Update()
    {
        if (isFiring)
        {
            if ((Time.time > lastTimeShootTimeStamp + shootInterval))
            {
                Shoot();
                lastTimeShootTimeStamp = Time.time;
            }
        }

        if (isAutoAiming)
        {
            LookAtSkully();
        }
    }



    private bool isAutoAiming = false;


    protected virtual void LookAtSkully()
    {
        Skully skully = LevelManager.Instance.Skully;
        Quaternion original = turretTrans.rotation;
        turretTrans.LookAt(skully.transform.position + Vector3.up * 0.5f);
        Quaternion desiredEuler = turretTrans.rotation;

        Quaternion eulerThisFrame = Quaternion.RotateTowards(original, desiredEuler, Time.deltaTime * rotateSpeed);

        turretTrans.rotation = eulerThisFrame;
    }

    protected virtual GameObject Shoot()
    {
        muzzleParticle.Play();

        audioSrc.clip = shootAudio;

        if (!audioSrc.isPlaying)
        {
            audioSrc.Play();
        }
        GameObject bulletInstance = GameObject.Instantiate(bulletPrefab);
        bulletInstance.transform.position = muzzle.position;
        bulletInstance.transform.rotation = muzzle.rotation;
        return bulletInstance;
    }

    public void StartAutoAim()
    {
        isAutoAiming = true;
    }

    public void StopAutoAim()
    {
        isAutoAiming = false;
    }

    public void StartFiring()
    {
        isFiring = true;
        lastTimeShootTimeStamp = 0f;
    }

    public void StopFiring()
    {
        isFiring = false;
    }
}
