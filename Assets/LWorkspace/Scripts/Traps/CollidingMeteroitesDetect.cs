using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidingMeteroitesDetect : MonoBehaviour
{
    [SerializeField] private MeteoritesCollide meteoritesCollide;

    [SerializeField] private MeshRenderer mr;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Skully>() != null)
        {
            DOVirtual.Color(Color.white,Color.blue,1f,(c)=>
            {
                mr.material.color = c;            
            });
            meteoritesCollide.StartPlaying();
        }
    }
}
