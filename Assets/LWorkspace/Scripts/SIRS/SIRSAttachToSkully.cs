using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SIRSAttachToSkully : MonoBehaviour
{
    public void AttachToSkully()
    {
        StartCoroutine(MoveToSkully());
    }

    [SerializeField] private float speed = 50f;

    private IEnumerator MoveToSkully()
    {
        Skully skully = LevelManager.Instance.Skully;

        float distance = 10;
        while (distance > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, skully.SIRSMount.AttachPoint.position, Time.deltaTime * speed);
            distance = Vector3.Distance(transform.position, skully.SIRSMount.AttachPoint.position);
            yield return new WaitForEndOfFrame();
        }

        transform.parent = skully.SIRSMount.AttachPoint;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        this.enabled = false;
        Destroy(this);
    }
}
