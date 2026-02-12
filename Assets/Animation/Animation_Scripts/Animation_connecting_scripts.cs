using UnityEngine;

public class Animation_connecting_scripts : MonoBehaviour
{
    public GameObject Character_prefab;
    private Animator mAnimator;
    private bool ismoving;
    private PlayerController playerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponent<PlayerController>();

        mAnimator = GetComponentInChildren<Animator>();

        Debug.Log(mAnimator);
    }

    // Update is called once per frame
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
