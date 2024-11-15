using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SophiaComeEventManager : MonoBehaviour
{
    [SerializeField] private PoliceshipBoss policeshipBoss;
    [SerializeField] private GameObject sophia;
    [SerializeField] private Skully skully;
    [SerializeField] private TimerManager timerManager;
    [SerializeField] private Transform eventStartPos;

    [SerializeField] private EndingManager endingManager; 

    private void Start()
    {
        policeshipBoss.OnKilledEvent += PoliceshipBoss_OnKilledEvent;
    }

    private void OnDestroy()
    {
        policeshipBoss.OnKilledEvent -= PoliceshipBoss_OnKilledEvent;
    }

    public void MoveSkullyToEventStartPos()
    {
        skully.DisableControl();
        StartCoroutine(MoveSkullyCotoutine(skully, eventStartPos.position, 20));
    }

    public void EnableSkullyXYControl()
    {
        skully.SetMaxForwardSpeedFactor(0);
        skully.EnableControl();
        skully.EnableXYControl();
    }
    [SerializeField] private SkullyBoss skullyBoss;

    private void PoliceshipBoss_OnKilledEvent()
    {
        skully.DisableControl();
        sophia.gameObject.SetActive(true);
        sophia.gameObject.transform.position = policeshipBoss.transform.position + Vector3.forward + Vector3.right;
        sophia.transform.LookAt(skully.transform);

        skullyBoss.gameObject.SetActive(true);

        skullyBoss.transform.position = policeshipBoss.transform.position;
        skullyBoss.transform.LookAt(skully.transform.position);

        StartCoroutine(PlayEnding());
    }

    private IEnumerator PlayEnding()
    {
        yield return MoveSkullyCotoutine(skully, skullyBoss.transform.position + Vector3.back * 3f, 20f);

        yield return new WaitForSeconds(1f);

        bool dialogueFinished = false;
        skullyBoss.SayDialogue(() => dialogueFinished = true);
        
        while (!dialogueFinished)
        {
            yield return new WaitForEndOfFrame();              
        }

        yield return new WaitForSeconds(0.2f);

        skullyBoss.StartFloating();

        yield return MoveSkullyCotoutine(skully, sophia.transform.position + Vector3.right + Vector3.back, 20f);

        PlayDefeatBossDialogue();
    }

    IEnumerator MoveSkullyCotoutine(Skully skully, Vector3 target, float speed, Action OnComplete = null)
    {
        float distance = Vector3.Distance(skully.transform.position, target);
        while (distance >= 3f)
        {
            skully.transform.position = Vector3.MoveTowards(skully.transform.position, target, Time.deltaTime * speed);
            skully.transform.LookAt(target);
            distance = Vector3.Distance(skully.transform.position, target);
            yield return new WaitForEndOfFrame();
        }

        OnComplete?.Invoke();
    }

    [SerializeField] private DialogueManager endingDialogueManager;

    [SerializeField] private List<Dialogue> defeateBossDialogue;

    private void PlayDefeatBossDialogue()
    {
        endingDialogueManager.PlayDialogue(defeateBossDialogue, () =>
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(sophia.transform.DOLookAt(sophia.transform.position + Vector3.forward, 0.8f));
            seq.Join(skully.transform.DOLookAt(skully.transform.position + Vector3.forward, 0.8f));

            seq.AppendCallback(() =>
            {
                MoveSkullyAndSophiaForward();
                skully.DisableControl();
            });

            seq.AppendInterval(2.5f);
            seq.AppendCallback(() =>
            {
                PlayEndingText();
            });
        });
    }

    private bool moveSSFoward = false;

    private void MoveSkullyAndSophiaForward()
    {
        moveSSFoward = true;
    }

    private void Update()
    {
        if (moveSSFoward)
        {
            skully.transform.Translate(Vector3.forward * Time.deltaTime * 300f, Space.World);
            sophia.transform.Translate(Vector3.forward * Time.deltaTime * 300f, Space.World);
        }
    }

    private void PlayEndingText()
    {
        Ending ending = endingManager.GetEnding(CollectedCoinsManager.CoinsCollected);
        endingDialogueManager.PlayDialogue(ending.dialogueList, () =>
        {

        });
    }
}
