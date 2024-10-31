using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomHelper 
{
    public static void GetRandomItem<T>(List<T> itemList, out T t)
    {
        if (itemList.Count > 0)
        {
            t = itemList[Random.Range(0, itemList.Count)];
        }
        else
        {
            t = default(T);
        }
    }
}
