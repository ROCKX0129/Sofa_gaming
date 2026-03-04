using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUIContainer : MonoBehaviour
{ 

    public static PlayerUIContainer Instance { get; private set; }

    public GameObject Player1ItemPIC;
    public GameObject Player2ItemPIC;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnscenleLoaded;
    }
    private void OnscenleLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        Canvas canvas = GetComponent<Canvas>();

        canvas.worldCamera = Camera.main;
        Player1ItemPIC.GetComponent<GetItemPic>().targetObject = GameObject.FindGameObjectWithTag("Player1").GetComponent<ItemCharacterManager>();
        Player2ItemPIC.GetComponent<GetItemPic>().targetObject = GameObject.FindGameObjectWithTag("Player2").GetComponent<ItemCharacterManager>();

        // Player1ItemPIC.GetComponent

    }

}
