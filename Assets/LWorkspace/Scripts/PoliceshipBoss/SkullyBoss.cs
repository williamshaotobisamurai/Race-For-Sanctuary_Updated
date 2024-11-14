using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkullyBoss : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;

    public void SayDialogue(Action OnComplete)
    {
        dialogueManager.Play(OnComplete);
    }

    public void StartFloating()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOLocalRotate(new Vector3(90f, 0, 0), 5f));
        seq.Join(transform.DOMove(transform.position + Vector3.left * 30f, 10f));
        seq.Play();
    }
}


//Pilot: "Skully, why did you leave the station? You should have stayed where you belonged… where everything was comfortable and familiar."
//Skully: "If I’d stayed, I’d be trapped, never truly knowing myself. Growth happens in the unknown—out here, beyond comfort, is where I finally feel alive."