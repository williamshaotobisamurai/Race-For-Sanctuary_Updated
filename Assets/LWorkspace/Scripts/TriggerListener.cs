using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerListener : MonoBehaviour
{
    [SerializeField] private List<GameObject> activeObjects;
    [SerializeField] private List<GameObject> deactiveObjects;

    private void OnTriggerEnter(Collider other)
    {
        if (GameHelper.IsSkully(other, out Skully skully))
        {
            activeObjects.ForEach(t => t.SetActive(true));

            deactiveObjects.ForEach(t => t.SetActive(false));
        }
    }
}
