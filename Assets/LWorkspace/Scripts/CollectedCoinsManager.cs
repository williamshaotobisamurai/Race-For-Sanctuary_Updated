using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectedCoinsManager : MonoBehaviour
{
    [SerializeField] private Text collectedCoinsLabel;

    private static int collectedCoinsInCurrentLevel = 0;
    public static int CollectedCoinsInCurrentLevel 
    {
        get => collectedCoinsInCurrentLevel;
        set => collectedCoinsInCurrentLevel = value;
    }

    private static int coinsInAllLevels = 0;
    public static int CoinsInAllLevels
    {
        get => coinsInAllLevels;
        set => coinsInAllLevels = value;
    }

    private Skully skully;

    public void Init(Skully skully)
    {
        this.skully = skully;
        skully.OnCollectCoinEvent += Skully_OnCollectCoinEvent;
        collectedCoinsInCurrentLevel = 0;
      //  collectedCoinsLabel.text = collectedCoinsInCurrentLevel.ToString();
    }

    private void OnDestroy()
    {
      //  skully.OnCollectCoinEvent -= Skully_OnCollectCoinEvent;
    }

    private void Skully_OnCollectCoinEvent(Coin coin)
    {
        collectedCoinsInCurrentLevel++;
        collectedCoinsLabel.text = collectedCoinsInCurrentLevel.ToString();
    }

    public void AddLevelCoinsToTotal()
    {
        coinsInAllLevels += collectedCoinsInCurrentLevel;
        collectedCoinsInCurrentLevel = 0;
    }
}
