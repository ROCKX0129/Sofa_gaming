using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaRainSpawner2D : MonoBehaviour
{
    [System.Serializable]
    public class SpawnableItem
    {
        public GameObject prefab;
        public int weight = 1;
    }

    [Header("Spawn Items")]
    public List<SpawnableItem> spawnableItems = new List<SpawnableItem>();

    [Header("Spawn Timing")]
    public float spawnInterval = 2f;
    public int maxItemsInScene = 30;

    [Header("Spawn Area (Box)")]
    public Vector2 boxSize = new Vector2(10f, 5f);

    [Header("Freeze Spaend items")]
    public bool freezeOnSpawn = true;

    private int currentItemCount = 0;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (currentItemCount < maxItemsInScene)
            {
                SpawnItem();
            }
        }
    }

    void SpawnItem()
    {
        if (spawnableItems.Count == 0)
            return;

        GameObject prefabToSpawn = GetWeightedRandomItem();

        
        float randomX = Random.Range(
            transform.position.x - boxSize.x / 2,
            transform.position.x + boxSize.x / 2
        );

        float topY = transform.position.y + boxSize.y / 2;

        Vector2 spawnPosition = new Vector2(randomX, topY);

        GameObject spawnedItem = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        if ( freezeOnSpawn)
        {
            Rigidbody2D rb = spawnedItem.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
        

        currentItemCount++;
    }

    GameObject GetWeightedRandomItem()
    {
        int totalWeight = 0;

        foreach (var item in spawnableItems)
        {
            totalWeight += item.weight;
        }

        int randomNumber = Random.Range(0, totalWeight);

        int currentWeight = 0;

        foreach (var item in spawnableItems)
        {
            currentWeight += item.weight;

            if (randomNumber < currentWeight)
            {
                return item.prefab;
            }
        }

        return spawnableItems[0].prefab;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}
