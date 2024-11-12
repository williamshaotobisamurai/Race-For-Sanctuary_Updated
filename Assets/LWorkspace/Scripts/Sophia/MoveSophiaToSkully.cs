using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSophiaToSkully : MonoBehaviour
{
    [SerializeField] private Transform skully;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float followingSpeed = 30f;

    private bool isFollowingSkully = false;

    public void MoveToSkully()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMove(skully.transform.position + offset, 4f));
        seq.OnComplete(() => isFollowingSkully = true);
        seq.Play();            
    }

    private void LateUpdate()
    {
        transform.LookAt(skully.transform.position);
        if(isFollowingSkully) 
        {
            transform.position = skully.transform.position + offset;
        }
    }
}
