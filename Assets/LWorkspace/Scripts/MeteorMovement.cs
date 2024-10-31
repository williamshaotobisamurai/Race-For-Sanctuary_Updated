using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorMovement : MonoBehaviour
{
    // Start is called before the first frame update

    private Vector3 targetPos;

    [SerializeField] private GameObject particle;
    [SerializeField] private float minSpeed = 800f;
    [SerializeField] private float maxSpeed = 1600f;


    private float speed = 800f;

    public void Init(Vector3 position)
    {
        transform.localEulerAngles = Random.insideUnitSphere * 360f;
        transform.localScale = transform.localScale * Random.Range(0.2f, 3f);
        this.targetPos = position;
  

        speed = Random.Range(minSpeed, maxSpeed);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
        particle.transform.LookAt(targetPos);
        //transform.Translate(direction * Time.deltaTime * 200f,Space.World);
        if (Vector3.Distance(transform.position, targetPos) < 1f)
        {
            Destroy(gameObject);
        }
    }
}
