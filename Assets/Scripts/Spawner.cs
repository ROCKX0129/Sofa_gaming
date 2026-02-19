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

    [Header("Freeze After Landing")]
    public LayerMask groundLayer; // Layer to detect landing

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

        Rigidbody2D rb = spawnedItem.GetComponent<Rigidbody2D>();
        Collider2D col = spawnedItem.GetComponent<Collider2D>();

        if (rb != null && col != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 1f;
            col.isTrigger = false;

            // Add a helper to freeze after touching ground
            spawnedItem.AddComponent<FreezeOnLanding>().groundLayer = groundLayer;
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


// Helper script to freeze item after landing
public class FreezeOnLanding : MonoBehaviour
{
    [HideInInspector] public LayerMask groundLayer;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            // Stop physics
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
