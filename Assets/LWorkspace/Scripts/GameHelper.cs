using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHelper : MonoBehaviour
{
    private static GameHelper instance;
    public static GameHelper Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public static bool IsSkully(GameObject go, out Skully skully)
    {
        if (go != null)
        {
            skully = go.GetComponent<Skully>();
            if (skully != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            skully = null;
            return false;
        }
    }

    public static bool IsSkully(Collider collider, out Skully skully)
    {
        return IsSkully(collider.gameObject, out skully);
    }
}

