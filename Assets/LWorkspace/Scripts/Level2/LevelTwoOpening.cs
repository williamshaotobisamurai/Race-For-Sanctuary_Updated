using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTwoOpening : MonoBehaviour
{
    [SerializeField] private FollowPlayer followPlayer;    
    
    public void RotateCamera()
    {
        followPlayer.transform.DORotate(Vector3.zero, 1f);


    }
}
