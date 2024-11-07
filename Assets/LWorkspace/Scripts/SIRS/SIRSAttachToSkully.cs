using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SIRSAttachToSkully : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (GameHelper.IsSkully(other,out Skully skully)) 
        {
            
        }
    }
}
