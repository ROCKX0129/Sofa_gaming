using UnityEngine;

[CreateAssetMenu(fileName = "Item_SO", menuName = "Item/Item_SO")]
public class Item_SO : ScriptableObject
{
    public int itemID;
    public string itemName;
    public bool isPlaceable;
    public int Durability;
    public Sprite itemSprite;
}
