using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public static PortalManager Instance;

    public GameObject leftPortal;
    public GameObject rightPortal;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        leftPortal.SetActive(false);
        rightPortal.SetActive(false);
    }

    public void ActivateLeftPortal()
    {
        leftPortal.SetActive(true);
    }

    public void ActivateRightPortal()
    {
        rightPortal.SetActive(true);
    }
}