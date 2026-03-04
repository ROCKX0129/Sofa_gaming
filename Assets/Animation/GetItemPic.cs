using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class GetItemPic : MonoBehaviour
{
    [Header("Add Object itself")]
    public Image displayImage;
    [Header("Add Player Prefab in it")]
    public ItemCharacterManager targetObject;

    private GameObject lastEquippedItem;


    //I just Burrow the variallble "equippedItem" form ItemCharacterManager, to add object pic into This Image Display.
    //
    private void LateUpdate()
    {
        if (targetObject == null) return;

        if (targetObject.equippedItem == null || displayImage == null)
        {
            return;
        }

        GameObject currentItem = targetObject.equippedItem;
        if (currentItem != lastEquippedItem)
        {
            lastEquippedItem = currentItem;

            if (currentItem != null)
            {

                SpriteRenderer sRenderer = currentItem.GetComponentInChildren<SpriteRenderer>(true);
                if (sRenderer != null && sRenderer.sprite != null)
                {
                    displayImage.sprite = sRenderer.sprite;
                }
            }
            else
            {

                displayImage.sprite = null;
            }
        }
    }
}
