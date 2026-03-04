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

    public static event Action OnPickupEvent;


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

        var item = equippedItem?.GetComponent<IItem>();
        if (item == null) return;

        //  Projectile: 2 presses only
        if (item.ItemData.itemType == ItemType.Projectile)
        {
            UseItem();
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

            OnPickupEvent?.Invoke();
            //Debug.Log("Picked up: " + equippedItem.name);
        }
        else
        {
            Debug.Log(name + " found no items in radius");
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

        // TELEPORTER LOGIC (3 presses: pick up → throw → teleport)
        // TELEPORTER LOGIC (3 presses: pick up → throw → teleport)
        // TELEPORTER LOGIC (working 3 presses: pick up → throw → teleport)
        if (item.ItemData.itemName == "Teleporter")
        {
            Teleporter teleporterScript = equippedItem.GetComponent<Teleporter>();
            if (teleporterScript == null) return;

            // --- Press 1: Pick up ---
            if (!hasTeleporter && !teleporterScript.isPickedUp && !teleporterScript.isPlaced)
            {
                teleporterScript.PickUp(gameObject);      // sets ownerPlayer & isPickedUp = true
                equippedItem = teleporterScript.gameObject; // immediately assign to prevent double pickup
                hasTeleporter = true;
                Debug.Log("Teleporter picked up!");
            }
            // --- Press 2: Throw forward ---
            else if (hasTeleporter && teleporterScript.isPickedUp && !teleporterScript.isPlaced)
            {
                teleporterScript.Throw();                  // throws naturally from player's current position
                Debug.Log("Teleporter thrown!");
            }
            // --- Press 3: Teleport to thrown teleporter ---
            else if (teleporterScript.isPlaced)
            {
                teleporterScript.TeleportOwner();          // teleport player
                equippedItem = null;
                hasItem = false;
                hasTeleporter = false;
                Debug.Log("Teleported to teleporter!");
            }

            return; // exit UseItem for Teleporter
        }

        // THROWABLE PLACEABLES 
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

        // INSTANT CONSUMABLE ITEMS (e.g., Speed Boost, Ghost Orb)
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

        // PROJECTILE ITEMS (e.g., Ice)
        if (item.ItemData.itemType == ItemType.Projectile)
        {
            // Second press = shoot
            if (icePrefab != null && firePoint != null)
            {
                Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
                ShootProjectile(direction);
            }

            equippedItem = null;
            hasItem = false;
            return;
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


