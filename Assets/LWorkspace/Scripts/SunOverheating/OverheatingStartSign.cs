using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OverheatingStartSign : MonoBehaviour
{
    [SerializeField] private string text;

    public event OnSkullyEnter OnSkullyEnterEvent;
    public delegate void OnSkullyEnter();

    private void OnTriggerEnter(Collider other)
    {
        Skully skully = other.GetComponent<Skully>();
        if (skully != null)
        {
            InstructionManager.ShowText(text,3f,()=>
            {
                OnSkullyEnterEvent?.Invoke();
            });
        }
    }
}
 