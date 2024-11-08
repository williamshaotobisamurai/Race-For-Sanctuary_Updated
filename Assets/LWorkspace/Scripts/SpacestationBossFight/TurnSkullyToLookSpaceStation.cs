using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSkullyToLookSpaceStation : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private Transform targetPos;
    [SerializeField] private FollowPlayer cameraFollowPlayer;
    [SerializeField] private Skully skully;

    private void OnTriggerEnter(Collider other)
    {
        if (GameHelper.IsSkully(other.gameObject, out Skully skully))
        {
            skully.DisableControl();
            skully.transform.DOMove(targetPos.position, duration);
            skully.transform.DORotate(targetPos.eulerAngles, duration);
            cameraFollowPlayer.transform.DORotate(targetPos.eulerAngles, duration).OnComplete(() =>
            {
                skully.EnableControl();
                skully.EnableXYControl();
                skully.SetMaxForwardSpeedFactor(0);
                skully.IsLookingBack = true;
            });
        }
    }

    public void TurnLookForward()
    {
        skully.DisableControl();
        skully.transform.DORotate(Vector3.zero, duration);
        cameraFollowPlayer.transform.DORotate(Vector3.zero, duration).OnComplete(() =>
        {
            skully.EnableControl();
            skully.EnableXYControl();
            skully.SetMaxForwardSpeedFactor(1);
            skully.IsLookingBack = false;
        });
    }
}
