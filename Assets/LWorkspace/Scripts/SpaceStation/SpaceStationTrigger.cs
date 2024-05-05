using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStationTrigger : MonoBehaviour
{
    public event OnSkullyEnter OnSkullyEnterEvent;
    public delegate void OnSkullyEnter(SpaceStationTrigger spaceStationTrigger);

    private void OnTriggerEnter(Collider other)
    {
        OnSkullyEnterEvent?.Invoke(this);
    }
}
