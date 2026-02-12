using Unity.VisualScripting;
using UnityEngine;

public class Telepoter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Vector2 Stored_position;
    private bool Storing_position = true;
    private GameObject TargetObject;
    private void OnEnable()
    {
        //EventManager.OnPlayerEvent += Stored_telepote;
        //EventManager.OnPlayerEvent += telepote;
        PlayerUsing.OnUseEvent += Stored_telepote;
        PlayerUsing.OnPlayerUsing += getObject;
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

    private void OnDisable()
    {
        PlayerUsing.OnUseEvent -= Stored_telepote;
        PlayerUsing.OnPlayerUsing -= getObject;
    }
}
