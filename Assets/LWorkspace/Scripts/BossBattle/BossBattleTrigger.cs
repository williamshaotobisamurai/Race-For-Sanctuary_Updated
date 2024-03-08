using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleTrigger : MonoBehaviour
{
    [SerializeField] private BossBattleManager battleManager;

    private void OnTriggerEnter(Collider other)
    {
        Skully skully = other.GetComponent<Skully>();
        if (skully != null)
        {
            battleManager.StartBossBattle();
            gameObject.SetActive(false);
        }
    }    
}
