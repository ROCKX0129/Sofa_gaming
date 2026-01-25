using UnityEngine;
using UnityEngine.InputSystem;
public class LocalMultiplayer : MonoBehaviour
{
    public PlayerInputManager playerInputManager;

    private bool keyboard1Joined;
    private bool keyboard2Joined;

    void Update()
    {
        if (Keyboard.current != null)
            return;

        if (!keyboard1Joined && Keyboard.current.spaceKey.wasPressedThisFrame )
        {
            playerInputManager.JoinPlayer(playerIndex: 0, controlScheme: "Keyboard1", pairWithDevice: null);
            keyboard1Joined = true;
        }
        
        if (!keyboard2Joined && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            playerInputManager.JoinPlayer(playerIndex: 1, controlScheme: "Keyboard2", pairWithDevice: null);
            keyboard2Joined = true;
        }
    } 
}
