using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerToPosition : MonoBehaviour
{
    [SerializeField] private Transform targetTrans;
    [SerializeField] private Collider myTrigger;
    [SerializeField] private bool autoMove = true;

    private void OnTriggerEnter(Collider other)
    {
        Skully skully = other.GetComponent<Skully>();
        if (skully != null && autoMove)
        {
            StartSkullyMovement(skully);
        }
    }

    IEnumerator MoveSkullyCotoutine(Skully skully, Transform target)
    { 
        float distance = Vector3.Distance(skully.transform.position, target.position);
        while (distance >= 3f)
        {
            skully.transform.position =  Vector3.MoveTowards(skully.transform.position, target.position, Time.deltaTime * 50f);
            skully.transform.LookAt(target.position);
            distance = Vector3.Distance(skully.transform.position, target.position);
            yield return new WaitForEndOfFrame();
        }

        skully.EnableControl();
    }

    public void StartSkullyMovement(Skully skully)
    {
        myTrigger.enabled = false;
        skully.DisableControl();
        StartCoroutine(MoveSkullyCotoutine(skully, targetTrans));
    }
}
