using System;
using UnityEngine;

public enum ItemType { Placeable, Projectile, Other }

public class ItemCharacterManager : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private float pickupRadius = 1.5f;       // range to pick up items
    [SerializeField] private LayerMask itemLayer;             // only pickable items

    [Header("Projectile Settings")]
    public GameObject icePrefab;   // example projectile prefab
    public Transform firePoint;    // spawn point for projectiles

    private Vector2 playerCurrentPosition;
    private GameObject currentPlayer;

    private GameObject nearbyItem;     // item on ground
    private GameObject equippedItem;   // item currently held
    private bool hasItem = false;

    private bool isPlacing = false;

    public static event Action<Vector2> OnItemUsingPosition;
    public static event Action<GameObject> OnCurrentPlayerCalling;

    private void Awake()
    {
        currentPlayer = gameObject;
    }

    /// <summary>
    /// Called by InputAction "Using" when E is pressed
    /// </summary>
    public void CurrentPlayerUsing()
    {
        if (!hasItem)
        {
            TryPickup();
            return;
        }

        UseItem();
    }

    private void TryPickup()
    {
        // Detect item in range
        Collider2D hit = Physics2D.OverlapCircle(transform.position, pickupRadius, itemLayer);
        if (hit != null)
        {
            nearbyItem = hit.gameObject;

            equippedItem = nearbyItem;
            hasItem = true;

            // Hide item in world
            equippedItem.SetActive(false);
            nearbyItem = null;

            Debug.Log("Picked up: " + equippedItem.name);
        }
    }

    private void UseItem()
    {
        if (equippedItem == null) return;

        var item = equippedItem.GetComponent<IItem>();
        if (item == null || item.ItemData == null)
        {
            Debug.LogWarning("Item missing IItem or ItemData.");
            return;
        }

        Debug.Log("Using item: " + item.ItemData.itemName);

        // --- PLACEABLE ITEMS ---
        if (item.ItemData.isPlaceable)
        {
            if (!isPlacing)
            {
                OnItemUsingPosition?.Invoke(playerCurrentPosition);
                OnCurrentPlayerCalling?.Invoke(currentPlayer);

                Instantiate(equippedItem, playerCurrentPosition, Quaternion.identity);

                isPlacing = true;
            }

            // Remove from hand
            Destroy(equippedItem);
            equippedItem = null;
            hasItem = false;
            isPlacing = false;
            return;
        }

        // --- PROJECTILE ITEMS ---
        if (item.ItemData.itemType == ItemType.Projectile)
        {
            if (icePrefab != null && firePoint != null)
            {
                Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
                ShootProjectile(direction);
            }

            // Remove from hand
            Destroy(equippedItem);
            equippedItem = null;
            hasItem = false;
        }
    }

    private void ShootProjectile(Vector2 direction)
    {
        GameObject proj = Instantiate(icePrefab, firePoint.position, Quaternion.identity);
        var projectileScript = proj.GetComponent<Freeze>(); // your Ice/Freeze script
        if (projectileScript != null)
            projectileScript.Shoot(direction);
    }

    /// <summary>
    /// Update the player's current position (called from PlayerUsing)
    /// </summary>
    public void UpdateUsePosition(Vector2 position)
    {
        playerCurrentPosition = position;
    }

    // Optional: visualize pickup range in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}

