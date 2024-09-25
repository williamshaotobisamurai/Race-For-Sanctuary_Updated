using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPangMovement : MonoBehaviour
{

    [SerializeField] private float speed = 10f;

    private float t;

    Vector3 start;
    Vector3 end;

    private void Start()
    {
        start = new Vector3(0, 0, Random.Range(0, 3f));
        end = new Vector3(0, 0, Random.Range(-3, 0f));

        start.x = transform.localPosition.x;
        start.y = transform.localPosition.y;
         
        end.x = transform.localPosition.x;
        end.y = transform.localPosition.y;
    }

    private void Update()
    {
        t = Mathf.PingPong(Time.time * speed, 1);
        transform.localPosition = Vector3.Lerp(start, end, t);
    }
}

 