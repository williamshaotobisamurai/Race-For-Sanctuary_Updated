using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private List<TutorialPhaseBase> tutorialPhaseList;

    [SerializeField] private TMP_Text instructionLabel;

    [SerializeField] private CanvasGroup tutorialCanvasGroup;

    [SerializeField] private int currentTutorialIndex = 0;

    [SerializeField] private TutorialPhaseBase currentPhase;

    void Start()
    {
        StartNewTutorialPhase(tutorialPhaseList[0]);
    }

    private void StartNewTutorialPhase(TutorialPhaseBase tutorialPhase)
    {
        if (currentPhase != null)
        {
            currentPhase.CleanPhase();
        }

        TutorialPhaseBase tutorialInstance = Instantiate(tutorialPhase, transform);
        tutorialInstance.gameObject.transform.position = new Vector3(0, 0, GameManager.Instance.Skully.transform.position.z);
        tutorialInstance.gameObject.SetActive(true);
        tutorialInstance.Prepare();
        tutorialInstance.OnReachEndTrigger += OnSkullyReachEnd;
        ShowTutorialInstruction(tutorialInstance);
        currentPhase = tutorialInstance;
    }

    private void ShowTutorialInstruction(TutorialPhaseBase tutorialPhase)
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() =>
        {
            instructionLabel.text = tutorialPhase.instructionText;
        });
        seq.Append(tutorialCanvasGroup.DOFade(1, 1f));
        seq.AppendInterval(3f);
        seq.Append(tutorialCanvasGroup.DOFade(0, 1f));
        seq.AppendCallback(() =>
        {
            tutorialPhase.StartPhase();
        });
        seq.Play();
    }

    private void OnSkullyReachEnd(bool success)
    {
        if (success)
        {
            CurrentPhaseFinished();
        }
        else
        {
            StartNewTutorialPhase(tutorialPhaseList[0]);
        }
    }


    private void CurrentPhaseFinished()
    {
        TutorialPhaseBase nextPhase = GetNextPhase();
        if (nextPhase != null)
        {
            StartNewTutorialPhase(nextPhase);
        }
    }

    private TutorialPhaseBase GetNextPhase()
    {
        currentTutorialIndex++;
        if (currentTutorialIndex >= tutorialPhaseList.Count)
        {
            return null;
        }
        else
        {
            return tutorialPhaseList[currentTutorialIndex];
        }
    }
}
