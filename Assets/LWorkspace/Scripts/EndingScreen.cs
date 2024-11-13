using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndingScreen : MonoBehaviour
{
    [SerializeField] private List<Ending> endingList;
    [SerializeField] private TMP_Text endingText;

    private void Start()
    {
        ShowEndingScreen();
    }

    public void ShowEndingScreen()
    {
        Debug.Log("ending screen " + CollectedCoinsManager.CoinsCollected);
        Ending ending = GetEnding(CollectedCoinsManager.CoinsCollected);
    }

    private Ending GetEnding(int coins)
    {
        for (int i = endingList.Count - 1; i >= 0; i--)
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
