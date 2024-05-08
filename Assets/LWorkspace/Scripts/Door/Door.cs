using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;

    private bool isOpened = false;

    public void Open()
    {
        isOpened = true; 
        doorAnimator.Play("Open");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Skully>() != null && !isOpened)
        {
            Open();
        }
    }
}
