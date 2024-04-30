using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialPhaseBase : MonoBehaviour
{
    public string instructionText;
    public string successText;
    public string failText;

    public virtual void Prepare()
    {
        endTrigger.OnSkullyEnterEvent += EndTrigger_OnSkullyEnterEvent;
    }
    public virtual void StartPhase()
    {
        isStarted = true;
    }

    public abstract bool IsSuccess();

    protected bool isStarted = false;

    public Action<bool> OnReachEndTrigger;

    [SerializeField] protected TutorialPhaseEndTrigger endTrigger;

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
}
