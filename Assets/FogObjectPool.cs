using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FogObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class FogType
    {
        public string typeName; // Name of the fog type
        public GameObject fogPrefab; // Prefab for this type (UI Image)
        public int initialPoolSize = 20; // Initial pool size for this type
    }

    public List<FogType> fogTypes = new List<FogType>();

    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    void Start()
    {
        foreach (var fogType in fogTypes)
        {
            if (fogType.fogPrefab == null)
            {
                Debug.LogError($"Fog prefab for type {fogType.typeName} not assigned.");
                continue;
            }

            Queue<GameObject> pool = new Queue<GameObject>();
            for (int i = 0; i < fogType.initialPoolSize; i++)
            {
                GameObject obj = Instantiate(fogType.fogPrefab);
                obj.SetActive(false);

                // Ensure the fog is set up as a UI Rect Image
                RectTransform rectTransform = obj.GetComponent<RectTransform>();
                if (rectTransform == null)
                {
                    Debug.LogError($"Fog prefab {fogType.fogPrefab.name} must have a RectTransform component.");
                    continue;
                }

                pool.Enqueue(obj);
            }

            poolDictionary[fogType.typeName] = pool;
        }
    }

    public GameObject GetFromPool(string typeName)
    {
        if (!poolDictionary.ContainsKey(typeName))
        {
            Debug.LogError($"Fog type {typeName} not found in pool.");
            return null;
        }

        Queue<GameObject> pool = poolDictionary[typeName];

        if (pool.Count == 0)
        {
            ExpandPool(typeName);
        }

        GameObject obj = pool.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public void ReturnToPool(string typeName, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(typeName))
        {
            Debug.LogError($"Fog type {typeName} not found in pool.");
            return;
        }

        obj.SetActive(false);
        poolDictionary[typeName].Enqueue(obj);
    }

    private void ExpandPool(string typeName)
    {
        if (!poolDictionary.ContainsKey(typeName))
        {
            Debug.LogError($"Fog type {typeName} not found in pool.");
            return;
        }

        FogType fogType = fogTypes.Find(f => f.typeName == typeName);
        if (fogType == null || fogType.fogPrefab == null)
        {
            Debug.LogError($"Fog type {typeName} configuration is invalid.");
            return;
        }

        Queue<GameObject> pool = poolDictionary[typeName];
        for (int i = 0; i < fogType.initialPoolSize; i++)
        {
            GameObject obj = Instantiate(fogType.fogPrefab);
            obj.SetActive(false);

            // Ensure the fog is set up as a UI Rect Image
            RectTransform rectTransform = obj.GetComponent<RectTransform>();
            if (rectTransform == null)
            {
                Debug.LogError($"Fog prefab {fogType.fogPrefab.name} must have a RectTransform component.");
                continue;
            }

            pool.Enqueue(obj);
        }
    }
}