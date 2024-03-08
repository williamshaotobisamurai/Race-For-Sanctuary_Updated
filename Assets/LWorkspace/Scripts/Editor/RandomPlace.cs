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
}
