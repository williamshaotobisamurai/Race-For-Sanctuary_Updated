using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialogue
{
    public string text;
    public float duration;
    public NPCDialogue npcDialogue;
}

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private List<Dialogue> dialogueList;

    [SerializeField] private float playerSpeedFactor = 0f;
    [SerializeField] private float interval = 0.5f;
    [SerializeField] private bool isPlayed = false;

    public void OnTriggerEnter(Collider other)
    {
        if (GameHelper.IsSkully(other, out Skully skully))
        {
            if (!isPlayed)
            {
                isPlayed = true;
                Debug.Log("start dialogue ");

                float playerOrigionalSpeedFactor = skully.GetMaxSpeedFactor();
                skully.SetMaxSpeedFactor(playerSpeedFactor);
                PlayDialogue(() =>
                {
                    Debug.Log("dialogue finish ");
                    skully.SetMaxSpeedFactor(playerOrigionalSpeedFactor);
                });
            }
        }
    }

    private void PlayDialogue(Action OnComplete)
    {
        Sequence seq = DOTween.Sequence();

        for (int i = 0; i < dialogueList.Count; i++)
        {
            Dialogue dialogue = dialogueList[i];
            NPCDialogue npcDialogue = dialogue.npcDialogue;
            seq.AppendCallback(() =>
            {
                npcDialogue.Show(dialogue.text);
            });
            seq.AppendInterval(interval + dialogue.duration);
            seq.AppendCallback(() =>
            {
                npcDialogue.Hide();
            });
        }
        seq.OnComplete(() =>
        {
            OnComplete?.Invoke();
        });
    }
}
