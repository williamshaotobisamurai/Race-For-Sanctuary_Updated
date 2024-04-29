using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPhaseEndTrigger : MonoBehaviour
{
    public event OnSkullyEnter OnSkullyEnterEvent;
    public delegate void OnSkullyEnter();

    private void OnTriggerEnter(Collider other)
    {
        Skully skully = other.GetComponent<Skully>();
        if (skully != null)
        {
            OnSkullyEnterEvent?.Invoke();
        }
    }
}
