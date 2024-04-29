using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class RandomPlace : MonoBehaviour
{
    [MenuItem("LeoTools/RandomPlace")]
    public static void RandomPlaceObjects()
    {
        foreach (Object obj in Selection.objects)
        {
            obj.GetComponent<Transform>().localPosition = Random.insideUnitSphere * 1f;
            obj.GetComponent<Transform>().localEulerAngles = new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));
        }
    }

    [MenuItem("LeoTools/RandomPlaceObstacles")]
    public static void RandomPlaceObstacles()
    {
        for (int i = 0; i < Selection.objects.Length; i++)
        {
            Selection.objects[i].GetComponent<Transform>().localPosition +=
                new Vector3(Random.Range(-165f,165f), Random.Range(-165f, 165f), 0);
        }
    }

    [MenuItem("LeoTools/PlaceCoins")]
    public static void PlaceCoins()
    {
        Vector3 pos = Vector3.zero;
        for (int i = 0; i < Selection.objects.Length; i ++)
        {
            Selection.objects[i].GetComponent<Transform>().localPosition = Vector3.forward * i * 65;
        }
    }
}
