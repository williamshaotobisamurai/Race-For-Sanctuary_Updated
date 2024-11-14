using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Dialogue
{
    public float intervalBeforeSpeaking;
    public string text;
    public float duration;

    public UnityEvent beforeSpeaking;
    public UnityEvent afterSpeaking;

    public float intervalAfterSpeaking;
    public NPCDialogue npcDialogue;
}

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private List<Dialogue> dialogueList;
    

    [SerializeField] private float playerSpeedFactor = 0f;
    [SerializeField] private float interval = 0.5f;
    [SerializeField] private bool isPlayed = false;
    [SerializeField] private bool recoverSpeedFactor = true;

    public void OnTriggerEnter(Collider other)
    {
        if (GameHelper.IsSkully(other, out Skully skully))
        {
            StartDialogue(skully);
        }
    }

    private void StartDialogue(Skully skully)
    {
        if (!isPlayed)
        {
            isPlayed = true;
            Debug.Log("start dialogue ");

            float playerOrigionalSpeedFactor = skully.GetMaxSpeedFactor();
            skully.SetMaxForwardSpeedFactor(playerSpeedFactor);
            PlayDialogue(dialogueList, () =>
            {
                Debug.Log("dialogue finish ");
                if (recoverSpeedFactor)
                {
                    skully.SetMaxForwardSpeedFactor(playerOrigionalSpeedFactor);
                }
            });
        }
    }

    public void PlayDialogue(List<Dialogue> dialogueList, Action OnComplete)
    {
        Sequence seq = DOTween.Sequence();

        for (int i = 0; i < dialogueList.Count; i++)
        {
            Dialogue dialogue = dialogueList[i];
            NPCDialogue npcDialogue = dialogue.npcDialogue;
            seq.AppendCallback(() =>
            {
                dialogue.beforeSpeaking?.Invoke();
            });
            seq.AppendInterval(dialogue.intervalBeforeSpeaking);
            seq.AppendCallback(() =>
            {
                npcDialogue.Show(dialogue.text);
            });

            seq.AppendInterval(interval + dialogue.duration);
            seq.AppendCallback(() =>
            {
                npcDialogue.Hide();
                dialogue.afterSpeaking?.Invoke();
            });

            seq.AppendInterval(dialogue.intervalAfterSpeaking);
        }
        seq.OnComplete(() =>
        {
            OnComplete?.Invoke();
        });
    }

    public void Play(Action OnComplete)
    {
        PlayDialogue(this.dialogueList, OnComplete);
    }
}
