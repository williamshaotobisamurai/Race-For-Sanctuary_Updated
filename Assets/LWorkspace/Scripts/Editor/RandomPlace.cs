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
                new Vector3(Random.Range(-105f,105f), Random.Range(-105f, 105f), 0);
        }
    }  

    [MenuItem("LeoTools/PlaceCoins")] 
    public static void PlaceCoins()
    {
        Vector3 pos = Vector3.zero;
        for (int i = 0; i < Selection.objects.Length; i ++)
        {
            Selection.objects[i].GetComponent<Transform>().localPosition = Vector3.forward * i * 15;
        }
    }

    [MenuItem("LeoTools/RandomScale")]
    public static void RandomScale()
    {
        Vector3 pos = Vector3.zero;
        for (int i = 0; i < Selection.objects.Length; i++)
        {
            Selection.objects[i].GetComponent<Transform>().localScale = Vector3.one * Random.Range(1,3f) * 10f;
        }
    }
}
