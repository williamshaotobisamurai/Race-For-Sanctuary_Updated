using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectedCoinsManager : MonoBehaviour
{
    [SerializeField] private Text collectedCoinsLabel;
    [SerializeField] private Skully skully;

    private static int collectedCoinsInLevel = 0;
    private int coinsInAllLevels = 0;

    public void Init()
    {
        collectedCoinsInLevel = 0;
        collectedCoinsLabel.text = collectedCoinsInLevel.ToString();
    }

    private void Start()
    {
        skully.OnCollectCoinEvent += Skully_OnCollectCoinEvent;
    }

    private void OnDestroy()
    {
        skully.OnCollectCoinEvent -= Skully_OnCollectCoinEvent;
    }

    private void Skully_OnCollectCoinEvent(Coin coin)
    {
        collectedCoinsInLevel++;
        collectedCoinsLabel.text = collectedCoinsInLevel.ToString();
    }

    public void AddLevelCoinsToTotal()
    {
        GameDataManager.CoinsCollectedInAllLevels += collectedCoinsInLevel;
    }
}
