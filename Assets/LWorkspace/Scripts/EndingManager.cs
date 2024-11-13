using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EndingManager : MonoBehaviour
{
    [SerializeField] private List<Ending> endingList;

    public Ending GetEnding(int coins)
    {
        for (int i = 0; i < endingList.Count; i++)
        {
            Ending ending = endingList[i];
            if (coins >= ending.minCoins)
            {
                return ending;
            }
        }
        return null;
    }
}



[Serializable]
public class Ending
{
    public int minCoins;
    public List<Dialogue> dialogueList;
}