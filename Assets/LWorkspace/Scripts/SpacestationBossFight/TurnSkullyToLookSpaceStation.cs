using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSkullyToLookSpaceStation : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private Transform targetPos;
    [SerializeField] private FollowPlayer cameraFollowPlayer;

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
}
