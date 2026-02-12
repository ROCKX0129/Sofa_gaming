using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUsing : MonoBehaviour
{

    [SerializeField] private PlayerController playerController;
    public static event Action<Vector2> OnUseEvent;
    public static event Action<GameObject> OnPlayerUsing;
    public GameObject telepoterTesting;
    public GameObject PlayerTarget;
    private bool canUsing = true;
    private bool telepoterUsing = true;
    public float ResetCooldown = 3.0f;

    private void Start()
    {
        PlayerTarget = gameObject;
    }
    public void OnUsing(InputAction.CallbackContext ctx)
    {
        
        if (ctx.performed && canUsing)  
        {
            UsingTelepoter();
        }
    }

    private void UsingTelepoter()
    {
        Vector2 myPosition = gameObject.transform.position;
        if (telepoterUsing)
        {
            Instantiate(telepoterTesting, myPosition, Quaternion.identity);
            telepoterUsing=!telepoterUsing;
            Invoke(nameof(ResetTelep), ResetCooldown);
        }
        OnUseEvent?.Invoke(myPosition);
        OnPlayerUsing?.Invoke(PlayerTarget);
    }

    void ResetTelep()
    {
        telepoterUsing = true;
    }
}
