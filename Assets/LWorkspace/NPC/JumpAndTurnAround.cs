using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAndTurnAround : MonoBehaviour
{
    [SerializeField] private Transform NPCVisual;
    public void DoAction()
    {
        NPCVisual.transform.DOLocalJump(Vector3.zero, 1f, 1, 0.5f).
            Join(NPCVisual.transform.DOLocalRotate(new Vector3(0, 180, 0), 0.5f));
    }
}
