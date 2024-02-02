using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectedCoinsManager : MonoBehaviour
{
    [SerializeField] private Text collectedCoinsLabel;
    [SerializeField] private Skully skully;

    private int collectedCoins = 0;

    public void Init()
    {
        collectedCoins = 0;
        collectedCoinsLabel.text = collectedCoins.ToString();
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
        collectedCoins++;
        collectedCoinsLabel.text = collectedCoins.ToString();
    }
}
