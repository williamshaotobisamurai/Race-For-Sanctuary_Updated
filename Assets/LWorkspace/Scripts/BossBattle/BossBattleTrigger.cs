using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleTrigger : MonoBehaviour
{
    [SerializeField] private BossBattleManager battleManager;

    private void OnTriggerEnter(Collider other)
    {
        if (GameHelper.IsSkully(other, out Skully skully))
        {
            battleManager.StartBossBattle();
            gameObject.SetActive(false);
        }
    }
}
