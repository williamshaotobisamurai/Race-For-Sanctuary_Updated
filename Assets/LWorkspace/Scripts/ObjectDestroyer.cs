using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(GameConstants.BOOST_ITEM) ||
            other.tag.Equals(GameConstants.BULLLET) ||
            other.tag.Equals(GameConstants.BLOB) ||
            other.tag.Equals(GameConstants.TRAP) ||
            other.tag.Equals(GameConstants.OBSTACLE) ||
            other.tag.Equals(GameConstants.METEOR))
        {
            other.gameObject.SetActive(false);
        }
    }
}
