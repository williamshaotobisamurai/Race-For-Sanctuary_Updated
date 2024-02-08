using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    public enum EItemType
    {
        SPEED_BOOST,
        DEFENSIVE_BOOST,
        JETPACK_FUEL,
        HEAL_ITEM,
    }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Collider m_collider;
    [SerializeField] private EItemType itemType;
    [SerializeField] private GameObject itemModel;

    public EItemType ItemType => itemType;

    public void Collect()
    {
        if (audioSource.clip != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }

        m_collider.enabled = false;
        itemModel.SetActive(false);
    }
}
