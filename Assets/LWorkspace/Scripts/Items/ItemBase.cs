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
        COOLING_ITEM,
        WEAPON_ITEM,
    }

    [SerializeField] private Collider m_collider;
    [SerializeField] private EItemType itemType;
    [SerializeField] private GameObject itemModel;

    public EItemType ItemType => itemType;

    private bool isCollected = false;
    public bool IsCollected { get => isCollected; }

    public void Collect()
    {
        isCollected = true;
        m_collider.enabled = false;
        itemModel.SetActive(false);
    }
}
