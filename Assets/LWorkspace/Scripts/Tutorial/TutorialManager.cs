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
    [SerializeField] private TutorialPhaseBase currentPhasePrefab;


    public event OnAllTutorialPassed OnAllTutorialPassedEvent;
    public delegate void OnAllTutorialPassed();

    public void StartRunningTutorial()
    {
        StartNewTutorialPhase(tutorialPhaseList[0]);
    }

    private void StartNewTutorialPhase(TutorialPhaseBase tutorialPhasePrefab)
    {
        if (currentPhase != null)
        {
            currentPhase.CleanPhase();
        }
        TutorialPhaseBase tutorialInstance = Instantiate(tutorialPhasePrefab, transform);
        tutorialInstance.gameObject.transform.position = new Vector3(0, 0, LevelManager.Instance.Skully.transform.position.z + tutorialInstance.TransitionDistance);
        tutorialInstance.gameObject.SetActive(true);
        tutorialInstance.Prepare();

        tutorialInstance.OnReachEndTrigger += OnSkullyReachEnd;
        ShowTutorialInstruction(tutorialInstance);
        currentPhase = tutorialInstance;
        currentPhasePrefab = tutorialPhasePrefab;
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
            Sequence seq = DOTween.Sequence();
            seq.AppendCallback(() =>
            {
                instructionLabel.text = currentPhase.failText;
            });
            seq.Append(tutorialCanvasGroup.DOFade(1, 1f));
            seq.AppendInterval(3f);
            seq.Append(tutorialCanvasGroup.DOFade(0, 1f));
            seq.AppendCallback(() =>
            {
                FadeManager.Instance.Transition(() => StartNewTutorialPhase(currentPhasePrefab), null);
            });
            seq.Play();
        }
    }

    private void CurrentPhaseFinished()
    {
        TutorialPhaseBase nextPhase = GetNextPhase();
        if (nextPhase != null)
        {
            StartNewTutorialPhase(nextPhase);
        }
        else
        {
            OnAllTutorialPassedEvent?.Invoke();
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
