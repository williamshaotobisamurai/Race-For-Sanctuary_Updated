using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatMovement : MonoBehaviour
{
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;

    [SerializeField] private Transform target;

    [SerializeField] private float speed = 10f;

    private float t;

    private void Update()
    {        
        t = Mathf.PingPong(Time.time * speed, 1) ;
        target.position = Vector3.Lerp(start.position, end.position, t );
    }
}
 