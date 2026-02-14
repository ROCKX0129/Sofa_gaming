using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemCharacterManager : MonoBehaviour
{
    public Item_SO Item_SO;

    private Vector2 PlayerCurrentPosition;
    private GameObject CurrentPlayer;

    public List<GameObject> items;
    public List<GameObject> PlayerHasItem;

    public static event Action<Vector2> OnItemUsingPosition;
    public static event Action<GameObject> OnCurrentPlayerCalling;

    public int CurrentChooseing = 0;

    private bool isplaceing = false;
    public void Start()
    {
        TestingP();
    }

    private void TestingP() 
    {
        PlayerHasItem = items;
    }

    public void CurrentPlayerUsing()
    {

        if (PlayerHasItem[CurrentChooseing] != null)
        {
            var item = PlayerHasItem[CurrentChooseing].GetComponent<IItem>();
            if (item.ItemData != null)
            {
                Debug.Log($"Player is using item: {item.ItemData.itemName}");
                if (item.ItemData.isPlaceable)
                {
                    if (!isplaceing)
                    {
                        UsingPlaceable();
                        isplaceing = true;
                    }

                    if (isplaceing)
                    {
                        CallingPlaceable();
                    }
                }

            }
            else
            {
                Debug.LogWarning("The item does not have valid ItemData.");
            }
        }
    }

    private void UsingPlaceable()
    {
        OnItemUsingPosition?.Invoke(PlayerCurrentPosition);
        OnCurrentPlayerCalling?.Invoke(CurrentPlayer);
        Instantiate(PlayerHasItem[CurrentChooseing], PlayerCurrentPosition, Quaternion.identity);
    }

    private void CallingPlaceable()
    {
        OnItemUsingPosition?.Invoke(PlayerCurrentPosition);
        OnCurrentPlayerCalling?.Invoke(CurrentPlayer);
    }

    private void OnEnable()
    {
        PlayerUsing.OnPlayerUsing += HandlePlayerUsing;
        PlayerUsing.OnUseEvent += HandleUseEvent;
    }

    private void OnDisable()
    {
        PlayerUsing.OnPlayerUsing -= HandlePlayerUsing;
        PlayerUsing.OnUseEvent -= HandleUseEvent;
    }
    private void HandlePlayerUsing(GameObject player)
    {
        CurrentPlayer = player;
        CurrentPlayerUsing(); 
    }

    private void HandleUseEvent(Vector2 position)
    {
        PlayerCurrentPosition = position;
    }
}
