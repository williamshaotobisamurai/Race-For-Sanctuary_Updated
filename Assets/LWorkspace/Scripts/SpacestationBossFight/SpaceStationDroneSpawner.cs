using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStationDroneSpawner : MonoBehaviour
{
    [SerializeField] private GameObject dronePrefab;
    [SerializeField] private List<Transform> spawnPointList;

    private int maxCount = 15;
    private int spawnInEachWave = 5;

    private bool isSpawning = false;

    private float lastWaveTimeStamp;
    private float waveInterval = 7f;

    private float lastSpawnTimeStamp;
    private float spawnInterval = 0.5f;

    [SerializeField] private List<Drone> spawnedDrones;

    private void Update()
    {
        if (!isSpawning) return;

        if (Time.time > lastWaveTimeStamp + waveInterval)
        {
            lastWaveTimeStamp = Time.time;
            StartCoroutine(SpawnWaveOfDrones());
        }
    }

    public void StartSpawningDrone()
    {
        isSpawning = true;
    }

    public void StopSpawningDrone()
    {
        isSpawning = false;
    }

    private IEnumerator SpawnWaveOfDrones()
    {
        int spawned = 0;
        while (spawned < spawnInEachWave && GetCurrentAliveDrones().Count < maxCount)
        {
            RandomHelper.GetRandomItem(spawnPointList, out Transform spawnPoint);
            SpawnOneDrone(spawnPoint);
            spawned++;
            yield return spawnInterval;   
        }
    }

    private void SpawnOneDrone(Transform spawnPoint)
    {
        GameObject instance = Instantiate(dronePrefab,spawnPoint);
        instance.transform.position = spawnPoint.position;
        Drone drone = instance.GetComponent<Drone>();

        drone.Init(spawnPoint);
        spawnedDrones.Add(drone);
    }

    private List<Drone> GetCurrentAliveDrones()
    {
        return spawnedDrones.FindAll(t => !t.IsKilled);
    }

    public void DestroyAllDrones()
    {
        GetCurrentAliveDrones().ForEach(d => d.Kill());
    }
}
