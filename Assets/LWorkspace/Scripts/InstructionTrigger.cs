using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionTrigger : MonoBehaviour
{
    [SerializeField] private string text;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Skully>() != null)
        {
            InstructionManager.ShowText(text);
        }
    }
}
