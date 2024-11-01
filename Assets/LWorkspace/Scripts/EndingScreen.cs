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

        //for (int i = 0; i < 100; i++)
        //{
        //    Debug.Log(i + " : " + GetEnding(i).endingText );
        //}
    }

    public void ShowEndingScreen()
    {
        Debug.Log("ending screen " + CollectedCoinsManager.CoinsInAllLevels);
        Ending ending = GetEnding(CollectedCoinsManager.CoinsInAllLevels);
        endingText.text = ending.endingText;        
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

[Serializable]
public class Ending
{
    public int minCoins;
    public string endingText;
}