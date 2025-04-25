using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FogSpawner : MonoBehaviour
{
    public RectTransform spawnArea; // RectTransform defining the spawning area
    public FogObjectPool fogObjectPool; // Reference to the object pool
    public int maxFogInstances = 10; // Maximum number of fog instances
    public float spawnInterval = 0.5f; // Time between spawns
    public Vector2 fogSpeedRange = new Vector2(50, 100); // Speed range of fog movement
    public List<string> fogTypes; // Types of fog to spawn

    private List<GameObject> activeFog = new List<GameObject>();

    void Start()
    {
        if (spawnArea == null || fogObjectPool == null || fogTypes == null || fogTypes.Count == 0)
        {
            Debug.LogError("Please assign a spawnArea, fogObjectPool, and at least one fog type.");
            return;
        }

        StartCoroutine(SpawnFog());
        StartCoroutine(InitFog());
    }
    private IEnumerator  InitFog()
    {
        int spawnTime =0;

        while (spawnTime < 10)
        {
            spawnTime++;
            if (activeFog.Count < maxFogInstances)
            {
                // Randomly pick a fog type
                string randomFogType = fogTypes[Random.Range(0, fogTypes.Count)];

                // Get a fog instance from the pool
                GameObject fog = fogObjectPool.GetFromPool(randomFogType);
                if (fog != null)
                {
                    fog.transform.SetParent(spawnArea);
                    fog.transform.localPosition = GetRandomPosition(-1000f,1000f);

                    // Randomize size and speed
                    float randomScale = Random.Range(0.5f, 1.5f);
                    fog.transform.localScale = new Vector3(randomScale, randomScale, 1);

                    float speed = Random.Range(fogSpeedRange.x, fogSpeedRange.y);

                    // Assign movement script
                    FogMovement movement = fog.GetComponent<FogMovement>();
                    if (movement == null)
                    {
                        movement = fog.AddComponent<FogMovement>();
                    }

                    movement.speed = speed;
                    movement.spawner = this;
                    movement.fogType = randomFogType;

                    activeFog.Add(fog);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    private IEnumerator SpawnFog()
    {
        while (true)
        {
            if (activeFog.Count < maxFogInstances)
            {
                // Randomly pick a fog type
                string randomFogType = fogTypes[Random.Range(0, fogTypes.Count)];

                // Get a fog instance from the pool
                GameObject fog = fogObjectPool.GetFromPool(randomFogType);
                if (fog != null)
                {
                    fog.transform.SetParent(spawnArea);
                    fog.transform.localPosition = GetRandomPosition(800f,1000f);

                    // Randomize size and speed
                    float randomScale = Random.Range(0.5f, 1.5f);
                    fog.transform.localScale = new Vector3(randomScale, randomScale, 1);

                    float speed = Random.Range(fogSpeedRange.x, fogSpeedRange.y);

                    // Assign movement script
                    FogMovement movement = fog.GetComponent<FogMovement>();
                    if (movement == null)
                    {
                        movement = fog.AddComponent<FogMovement>();
                    }
                    movement.speed = speed;
                    movement.spawner = this;
                    movement.fogType = randomFogType;

                    activeFog.Add(fog);
                }
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private Vector3 GetRandomPosition(float min,float max)
    {
        float x =  Random.Range(min, max);
        float y = Random.Range(-100f, -550f); 
        return new Vector3(x, y, 0);
    }

    public void ReturnFogToPool(GameObject fog, string fogType)
    {
        activeFog.Remove(fog);
        fogObjectPool.ReturnToPool(fogType, fog);
    }
}


