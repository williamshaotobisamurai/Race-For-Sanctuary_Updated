using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialPhaseBase : MonoBehaviour
{
    public string instructionText;
    public string failText;    

    [SerializeField] private float transitionDistance = 200f;
    [SerializeField] private float timeDuration = 100f;

    public float TransitionDistance { get => transitionDistance;  }
    public float TimeDuration { get => timeDuration;  }

    public virtual void Prepare()
    {
        endTrigger.OnSkullyEnterEvent += EndTrigger_OnSkullyEnterEvent;
        GameManager.Instance.Skully.OnSkullyDiedEvent += Skully_OnSkullyDiedEvent;
        GameManager.Instance.PoliceShip.OnCaughtSkullyEvent += PoliceShip_OnCaughtSkullyEvent;
        GameManager.Instance.PoliceShip.MoveToOriginalPosition();
        GameManager.Instance.TimerManager.Init(TimeDuration);
    }
    public virtual void StartPhase()
    {
        isStarted = true;
    }

    public abstract bool IsSuccess();

    protected bool isStarted = false;

    public Action<bool> OnReachEndTrigger;

    [SerializeField] protected TutorialPhaseEndTrigger endTrigger;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            OnReachEndTrigger?.Invoke(false);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            OnReachEndTrigger?.Invoke(true);
        }
    }

    public virtual void EndTrigger_OnSkullyEnterEvent()
    {
        OnReachEndTrigger?.Invoke(IsSuccess());
    }

    public virtual void CleanPhase()
    {
        isStarted = false;
        Debug.Log (gameObject.name +  " clean phase " + gameObject.GetInstanceID());
        Destroy(gameObject);
    }

    protected void PoliceShip_OnCaughtSkullyEvent()
    {
        Debug.Log("caught skully");
        OnReachEndTrigger?.Invoke(false);
    }

    protected virtual void Skully_OnSkullyDiedEvent()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(1f);
        seq.AppendCallback(() =>
        {
            OnReachEndTrigger?.Invoke(false);
        });
        seq.AppendInterval(1f);
        seq.AppendCallback(() =>
        {
            GameManager.Instance.Skully.Revive();

        });
        seq.Play();
    }
}
