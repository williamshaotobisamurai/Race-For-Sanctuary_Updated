using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform respawnTrans;
    public Transform RespawnTrans { get { return respawnTrans; } }

    public event OnSkullyEntered OnSkullyEnteredEvent;
    public delegate void OnSkullyEntered(Checkpoint checkpoint, Skully skully);

    [SerializeField] private int id;
    public int ID { get => id; }    

    private void OnTriggerEnter(Collider other)
    {
        if (GameHelper.IsSkully(other, out Skully skully))
        {
            OnSkullyEnteredEvent?.Invoke(this, skully);
        }
    }
}
