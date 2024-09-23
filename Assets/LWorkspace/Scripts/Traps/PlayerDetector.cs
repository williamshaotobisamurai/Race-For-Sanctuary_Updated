using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public event OnPlayerEnter OnPlayeEnterEvent;
    public delegate void OnPlayerEnter(Skully skully);

    public event OnPlayerLeft OnPlayerLeftEvent;
    public delegate void OnPlayerLeft(Skully skully);

    public void OnTriggerEnter(Collider other)
    {
        if (GameHelper.IsSkully(other, out Skully skully))
        { 
            OnPlayeEnterEvent?.Invoke(skully);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (GameHelper.IsSkully(other, out Skully skully))
        {
            OnPlayerLeftEvent?.Invoke(skully);
        }
    }
}
