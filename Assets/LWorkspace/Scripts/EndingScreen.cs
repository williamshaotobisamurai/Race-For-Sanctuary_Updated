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
        Debug.Log("ending screen " + GameDataManager.CoinsCollectedInAllLevels);
        Ending ending = GetEnding(GameDataManager.CoinsCollectedInAllLevels);
        endingText.text = ending.endingText;        
    }

    private Ending GetEnding(int coins)
    {
        return endingList.Find(t =>
          coins >= t.minCoins
        && coins <= t.maxCoins);
    }
}

[Serializable]
public class Ending
{
    public int minCoins;
    public int maxCoins;
    public string endingText;
}