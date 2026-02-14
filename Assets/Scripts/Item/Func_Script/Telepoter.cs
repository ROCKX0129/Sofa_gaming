using Unity.VisualScripting;
using UnityEngine;

public class Telepoter : MonoBehaviour ,  IItem
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Vector2 Stored_position;
    private bool Storing_position = true;
    private GameObject TargetObject;
    public  Item_SO itemSO;
    public Item_SO ItemData => itemSO;
    private void OnEnable()
    {
        ItemCharacterManager.OnItemUsingPosition += Stored_telepote;
        ItemCharacterManager.OnCurrentPlayerCalling += getObject;
    }
    private void OnDisable()
    {
        ItemCharacterManager.OnItemUsingPosition -= Stored_telepote;
        ItemCharacterManager.OnCurrentPlayerCalling -= getObject;
    }


    

    private void Stored_telepote(Vector2 getPosition)
    {
        if (Stored_position != null && !Storing_position)
        {

            TargetObject.transform.position = Stored_position;

            Destroy(gameObject);
        }

        if (Storing_position)
        {
            Stored_position = getPosition;
            Storing_position = false;
        }
    }
    private void getObject(GameObject CurrentPlayer)
    {
        if (Stored_position != null)
        {

            
            TargetObject = CurrentPlayer;
        }
    }

}
