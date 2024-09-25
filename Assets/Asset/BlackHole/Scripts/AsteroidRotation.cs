using UnityEngine;
using System.Collections;

public class AsteroidRotation : MonoBehaviour
{
	public Vector3 Rot;

    private void Start()
    {
        if (Rot == Vector3.zero)
        {
            Rot = Random.onUnitSphere * Random.Range(50f, 300f);
        }
    }

    void Update()
	{
		transform.Rotate(Rot * Time.deltaTime);
	}
}
