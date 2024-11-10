using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{

    [SerializeField] private float radius = 100f;

    [SerializeField] private List<GameObject> obstaclesList;
    // Start is called before the first frame update

    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnVariation = 1f;

    [SerializeField] private int spawnCount = 3;
    [SerializeField] private int spawnCountVariation = 1;

    private float nextSpawnTime = 0f;
    [SerializeField] private float inAdvanceOfSkully = 5000;


    private void Update()
    {
        Debug.DrawLine(transform.position, LevelManager.Instance.Skully.transform.position);
        transform.position = new Vector3(0, 0, LevelManager.Instance.Skully.transform.position.z + inAdvanceOfSkully);
        if (Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + spawnInterval + Random.Range(0, spawnVariation);

            int count = Random.Range(spawnCount, spawnCount + spawnCountVariation);
            Skully skully = LevelManager.Instance.Skully;

            for (int i = 0; i < count; i++)
            {
                RandomHelper.GetRandomItem(obstaclesList, out GameObject prefab);
                GameObject go = Instantiate(prefab, transform);
                Vector3 ran = Random.insideUnitSphere * radius + transform.position;
                ran.z = transform.position.z;
                go.transform.position = ran;
                go.GetComponent<MeteorMovement>().Init(skully.transform.position + Random.insideUnitSphere * 1.5f);
            }
        }
    }
}

