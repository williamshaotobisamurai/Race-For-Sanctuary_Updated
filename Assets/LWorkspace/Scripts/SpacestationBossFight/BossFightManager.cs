using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightManager : MonoBehaviour
{
    [SerializeField] private SpaceStationBoss spaceStationBoss;
    [SerializeField] private TurnSkullyToLookSpaceStation turnSkully;
    [SerializeField] private LevelManager levelManager;

    void Start()
    {
        spaceStationBoss.OnDestroyedEvent += SpaceStationBoss_OnDestroyedEvent;        
    }

    private void OnDestroy()
    {
        spaceStationBoss.OnDestroyedEvent -= SpaceStationBoss_OnDestroyedEvent;
    }

    private void SpaceStationBoss_OnDestroyedEvent()
    {
        turnSkully.TurnLookForward();        
    }

    public void LevelComplete()
    {
        levelManager.CompleteLevel();
    }    
}
