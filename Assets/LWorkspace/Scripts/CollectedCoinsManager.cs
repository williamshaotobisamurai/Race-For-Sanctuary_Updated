using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectedCoinsManager : MonoBehaviour
{
    private static int coinsCollected = 0;
    public static int CoinsCollected
    {
        get => coinsCollected;
        set => coinsCollected = value;
    }

    private Skully skully;

    public void Init(Skully skully)
    {
        this.skully = skully;
        skully.OnCollectCoinEvent += Skully_OnCollectCoinEvent;
    }

    private void OnDestroy()
    {
        if (skully != null)
        {
            skully.OnCollectCoinEvent -= Skully_OnCollectCoinEvent;
        }
    }

    private void Skully_OnCollectCoinEvent(Coin coin)
    {
        coinsCollected++;
    }
}
