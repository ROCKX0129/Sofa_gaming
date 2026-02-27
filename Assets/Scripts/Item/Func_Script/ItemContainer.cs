using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemContainer : MonoBehaviour
{
    public List<GameObject> ItemList = new List<GameObject> ();
    public int RandomIntForItem = 0;
    public GameObject CurrentItem;
    public GameObject PicForDisplayObject;
    public Sprite Sprite;
    public GameObject Bubble;

    
    
    private void Start ()
    {
        RandomIntForItem = Random.Range (0, ItemList.Count);
        CurrentItem = ItemList[RandomIntForItem];
        Sprite = CurrentItem.GetComponent<IItem>().ItemData.itemSprite;
        PicForDisplayObject.GetComponent<SpriteRenderer>().sprite = Sprite;

        Vector3 SpriteSize = Sprite.bounds.size;
        Vector3 BubbleSize = Bubble.GetComponent<SpriteRenderer>().sprite.bounds.size;

        PicForDisplayObject.transform.localScale = new Vector3(
                (BubbleSize.x / SpriteSize.x),
                (BubbleSize.y / SpriteSize.y) - 0.25f,
                1f
            );


    }


}
