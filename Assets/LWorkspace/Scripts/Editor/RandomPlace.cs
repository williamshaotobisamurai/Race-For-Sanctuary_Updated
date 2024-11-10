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
            Selection.objects[i].GetComponent<Transform>().localPosition =
                new Vector3(Random.Range(-250f,250f), Random.Range(-250f, 250f), i * 40);

            GameObject go = Instantiate(Selection.objects[i]) as GameObject;
            go.transform.localPosition = new Vector3(Random.Range(-150f, 150f), Random.Range(-150f, 150f), i * 40);
        }
    }  

    [MenuItem("LeoTools/PlaceCoins")] 
    public static void PlaceCoins()
    {
        Vector3 pos = Vector3.zero;
        for (int i = 0; i < Selection.objects.Length; i ++)
        {
            Selection.objects[i].GetComponent<Transform>().localPosition = Vector3.forward * i * 55;
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

    [MenuItem("LeoTools/RandomDelete")]
    public static void RandomDelete()
    {
        for (int i = 0; i < Selection.objects.Length; i++)
        {
            if (Random.Range(0f, 1f) > 0.5f)
            {
                DestroyImmediate(Selection.objects[i]);
            }
        }
    }
}
