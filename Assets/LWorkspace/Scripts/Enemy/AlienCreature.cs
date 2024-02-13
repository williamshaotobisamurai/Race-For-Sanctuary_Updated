using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienCreature : MonoBehaviour
{
    [SerializeField] private GameObject blobPrefab;

    [SerializeField] private Transform mouth;
    [SerializeField] private float shootInterval = 3f;

    [SerializeField] private Animator animator;

    private float lastTimeShootTimeStamp = 0f;

    private void Update()
    {
        transform.LookAt(GameManager.Instance.Skully.transform);

        if (transform.position.z < GameManager.Instance.Skully.transform.position.z - 5f)
        {
            gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Shoot();
        }
    }


    private void OnTriggerStay(Collider other)
    {
        Skully skully = other.GetComponent<Skully>();

        if (skully != null)
        {
            if (Time.time > lastTimeShootTimeStamp + shootInterval)
            {
                Shoot();
                lastTimeShootTimeStamp = Time.time;
            }
        }
    }

    private void Shoot()
    {
        animator.Play("Fly Fireball Shoot");
        DOVirtual.DelayedCall(0.6f, () =>
        {
            GameObject blobInstance = GameObject.Instantiate(blobPrefab);
            blobInstance.transform.position = mouth.position;
            blobInstance.transform.rotation = mouth.rotation;
            blobInstance.transform.LookAt(GameManager.Instance.Skully.transform);
        });
    }
}
