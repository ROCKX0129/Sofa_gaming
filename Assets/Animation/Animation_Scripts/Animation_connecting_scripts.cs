using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Animation_connecting_scripts : MonoBehaviour
{
    public GameObject Character_prefab;
    private Animator mAnimator;
    private bool ismoving;
    private PlayerController playerController;

    public InputAction Attack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponent<PlayerController>();

        mAnimator = GetComponentInChildren<Animator>();

        Debug.Log(mAnimator);
    }

    private void OnEnable()
    {
        Attack.Enable();
        Attack.performed += OnAttack;
        ItemCharacterManager.OnPickupEvent += OnPickup;
    }

    public void OnPickup(GameObject CurrentPlayer)
    {
        if (gameObject.name == CurrentPlayer.name)
            mAnimator.SetTrigger("OnPickup");
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            mAnimator.SetTrigger("OnMelee");
        }

    }

    public void PlayDeath()
    {
        mAnimator.SetTrigger("OnDead");
    }

    
    void Update()
    {
        if (playerController.moveInput != null)
        {
            if (playerController.moveInput.x != 0.0f)
            {
                ismoving = true;
            }
            else
            {
                ismoving = false;
            }


            mAnimator.SetBool("OnMoving", ismoving);

        }
    }
}
