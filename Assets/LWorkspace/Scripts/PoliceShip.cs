using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceShip : MonoBehaviour
{
    [SerializeField] private float distance = 20f;

    private Vector3 targetPos;

    [SerializeField] private float speed = 100f; 

    private void Update()
    {
        targetPos = GameManager.Instance.Skully.transform.position - new Vector3(0, 0, distance);

        float x = Mathf.Lerp(transform.position.x, targetPos.x, Time.deltaTime * speed);
        float y = Mathf.Lerp(transform.position.y, targetPos.y, Time.deltaTime * speed);

        //transform.position = new Vector3(x, y, GameManager.Instance.Skully.transform.position.z  - distance);

        transform.position = targetPos;
    }

    public void MoveCloseToSkully()
    { 
    
    }
}
