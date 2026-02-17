using System;
using UnityEngine;

public enum ItemType { Placeable, Projectile, Other }

public class ItemCharacterManager : MonoBehaviour
{
    private Vector2 playerCurrentPosition;
    private GameObject currentPlayer;

    private GameObject nearbyItem;     // item on ground
    private GameObject equippedItem;   // item player is holding
    private bool hasItem = false;

    [Header("Projectile Settings")]
    public GameObject icePrefab;
    public Transform firePoint;

    public static event Action<Vector2> OnItemUsingPosition;
    public static event Action<GameObject> OnCurrentPlayerCalling;

    private bool isPlacing = false;

    private void Awake()
    {
        currentPlayer = gameObject;
    }

    // This should be called when pressing E
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
        if (nearbyItem == null) return;

        equippedItem = nearbyItem;
        hasItem = true;

        equippedItem.SetActive(false); // hide item in world
        nearbyItem = null;

        Debug.Log("Picked up item!");
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

        Debug.Log("Using: " + item.ItemData.itemName);

        
        if (item.ItemData.isPlaceable)
        {
            if (!isPlacing)
            {
                OnItemUsingPosition?.Invoke(playerCurrentPosition);
                OnCurrentPlayerCalling?.Invoke(currentPlayer);

                Instantiate(equippedItem, playerCurrentPosition, Quaternion.identity);
                isPlacing = true;
            }

            Destroy(equippedItem);
            equippedItem = null;
            hasItem = false;
            isPlacing = false;

            return;
        }

        
        if (item.ItemData.itemType == ItemType.Projectile)
        {
            if (icePrefab != null && firePoint != null)
            {
                Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
                ShootIce(direction);
            }
        }

        // After use → remove item
        Destroy(equippedItem);
        equippedItem = null;
        hasItem = false;
    }

    private void ShootIce(Vector2 direction)
    {
        if (equippedItem == null) return;

        // Move the prefab to the firePoint and activate it
        equippedItem.transform.position = firePoint.position;
        equippedItem.SetActive(true);

        // Get the IceItem script and shoot
        Freeze ice = equippedItem.GetComponent<Freeze>();
        if (ice != null)
            ice.Shoot(direction);

        // Clear the equipped item (used)
        equippedItem = null;
        hasItem = false;
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            nearbyItem = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            if (nearbyItem == collision.gameObject)
                nearbyItem = null;
        }
    }

    // This should be called from your player input system
    public void UpdateUsePosition(Vector2 position)
    {
        playerCurrentPosition = position;
    }
}

