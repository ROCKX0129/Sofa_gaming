using System;
using UnityEngine;

public class ItemCharacterManager : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private float pickupRadius = 1.5f;
    [SerializeField] private LayerMask itemLayer;

    [Header("Projectile Settings")]
    public GameObject icePrefab;
    public Transform firePoint;

    [Header("Thrown / Placeable Settings")]
    public float throwForce = 5f;

    private Vector2 playerCurrentPosition;
    private GameObject currentPlayer;

    // Teleporter tracking
    private Vector2 teleporterPosition;
    private bool hasTeleporter = false;

    private GameObject nearbyItem;
    public GameObject equippedItem;
    private bool hasItem = false;
    

    

    private void Awake()
    {
        currentPlayer = gameObject;
    }

    public void CurrentPlayerUsing()
    {
        Debug.Log(name + " pressed Use");
        if (!hasItem)
        {
            TryPickup();
            return;
        }

        UseItem();
    }

    private void TryPickup()
    {
        Debug.Log("Player2 position: " + transform.position + " Pickup radius: " + pickupRadius);

        Collider2D hit = Physics2D.OverlapCircle(transform.position, pickupRadius, itemLayer);
        if (hit != null)
        {
            nearbyItem = hit.gameObject;
            equippedItem = nearbyItem;
            hasItem = true;

            equippedItem.SetActive(false); // hide until used
            nearbyItem = null;

            Debug.Log("Picked up: " + equippedItem.name);
        }
        else
        {
            Debug.Log(name + " found no items in radius");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(gameObject.name + " collided with " + collision.name + " on layer " + collision.gameObject.layer);

        if (hasItem) return; // already holding an item

        // Check if collided object is in the itemLayer
        if (itemLayer == (itemLayer | (1 << collision.gameObject.layer)))
        {
            nearbyItem = collision.gameObject;
            equippedItem = nearbyItem;
            hasItem = true;

            equippedItem.SetActive(false); // hide until used
            nearbyItem = null;

            Debug.Log("Picked up via trigger: " + equippedItem.name);
        }
    }

  

    private void UseItem()
    {
        Debug.Log($"UseItem called. equippedItem={equippedItem?.name}, hasTeleporter={hasTeleporter}");

        if (equippedItem == null) return;

        var item = equippedItem.GetComponent<IItem>();
        if (item == null || item.ItemData == null)
        {
            Debug.LogWarning("Item missing IItem or ItemData.");
            return;
        }

        // -----------------------
        // TELEPORTER LOGIC (ENDER-PEARL STYLE)
        // -----------------------
        if (item.ItemData.itemName == "Teleporter")
        {
            Teleporter teleporterScript = equippedItem.GetComponent<Teleporter>();

            if (!hasTeleporter)
            {
                // Pick it up and hide
                teleporterPosition = equippedItem.transform.position;
                hasTeleporter = true;
                equippedItem.SetActive(false);

                Debug.Log("Teleporter picked up! Position saved.");
            }
            else
            {
                // Teleport player back
                transform.position = teleporterPosition;
                Debug.Log("Teleported back to pickup point!");

                // Destroy the teleporter object
                if (teleporterScript != null)
                    Destroy(teleporterScript.gameObject);

                equippedItem = null;
                hasItem = false;
                hasTeleporter = false;
            }

            return; // exit so no placeable throwing occurs
        }

        // -----------------------
        // THROWABLE PLACEABLES (e.g., Exploding Chicken)
        // -----------------------
        if (item.ItemData.isPlaceable)
        {
            // Spawn/throw at firePoint or player position
            Vector2 spawnPos = firePoint != null ? firePoint.position : (Vector2)transform.position;
            equippedItem.transform.position = spawnPos;
            equippedItem.SetActive(true);

            Rigidbody2D rb = equippedItem.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.gravityScale = 1f;
                rb.linearVelocity = Vector2.zero;
            }

            Vector2 throwDir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

            if (item.ItemData.itemName == "Exploding Chicken")
            {
                var chicken = equippedItem.GetComponent<ExplodingChicken>();
                if (chicken != null)
                    chicken.Throw(throwDir * throwForce);
            }
            else
            {
                if (rb != null)
                    rb.linearVelocity = throwDir * throwForce;
            }

            // Immediately clear so no extra presses are needed
            equippedItem = null;
            hasItem = false;
            return;
        }

        // -----------------------
        // INSTANT CONSUMABLE ITEMS (e.g., Speed Boost, Ghost Orb)
        // -----------------------
        if (!item.ItemData.isPlaceable && item.ItemData.itemType != ItemType.Projectile)
        {
            if (item.ItemData.itemName == "Speed Boost")
            {
                var speed = equippedItem.GetComponent<SpeedBoost>();
                if (speed != null)
                    speed.Use(gameObject);
            }
            else if (item.ItemData.itemName == "Ghost Orb")
            {
                var ghost = equippedItem.GetComponent<GhostOrb>();
                if (ghost != null)
                    ghost.Use(gameObject);
            }

            equippedItem = null;
            hasItem = false;

            return;
        }

        // -----------------------
        // PROJECTILE ITEMS (e.g., Ice)
        // -----------------------
        if (item.ItemData.itemType == ItemType.Projectile)
        {
            if (icePrefab != null && firePoint != null)
            {
                Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
                ShootProjectile(direction);
            }

            equippedItem = null;
            hasItem = false;
        }
    }


    private void ShootProjectile(Vector2 direction)
    {
        GameObject proj = Instantiate(icePrefab, firePoint.position, Quaternion.identity);
        var projectileScript = proj.GetComponent<Freeze>();
        if (projectileScript != null)
            projectileScript.Shoot(direction);
    }

    public void UpdateUsePosition(Vector2 position)
    {
        playerCurrentPosition = position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}


