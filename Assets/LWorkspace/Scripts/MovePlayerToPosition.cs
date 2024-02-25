using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerToPosition : MonoBehaviour
{
    [SerializeField] private Transform targetTrans;
    [SerializeField] private Collider myTrigger;

    private void OnTriggerEnter(Collider other)
    {
        Skully skully = other.GetComponent<Skully>();
        if (skully != null)
        {
            myTrigger.enabled = false;

            skully.SetKinematic(true);

            StartCoroutine(MoveSkullyCotoutine(skully, targetTrans));
        }
    }

    IEnumerator MoveSkullyCotoutine(Skully skully, Transform target)
    { 
        float distance = Vector3.Distance(skully.transform.position, target.position);
        while (distance >= 3f)
        {
            skully.transform.position =  Vector3.MoveTowards(skully.transform.position, target.position, Time.deltaTime * 200f);
            distance = Vector3.Distance(skully.transform.position, target.position);
            yield return new WaitForEndOfFrame();
        }

        skully.SetKinematic(false);
    }
}
