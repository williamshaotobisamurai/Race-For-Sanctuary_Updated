using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SIRS : MonoBehaviour
{
    [SerializeField] private Skully skully;

    public event OnCollectCoin OnCollectCoinEvent;
    public delegate void OnCollectCoin(Coin coin);

    [SerializeField] private Transform missileAttachedTrans;
    public Transform MissileAttachedTrans => missileAttachedTrans;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(GameConstants.COIN))
        {
            other.GetComponent<Coin>().MoveToSkully(skully, CollectOneCoin);
        }
    }

    private void CollectOneCoin(Coin coin)
    {
        OnCollectCoinEvent?.Invoke(coin);
    }
}
