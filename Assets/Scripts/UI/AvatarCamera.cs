using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AvatarCamera : MonoBehaviour
{
    public GameObject Player1Camera;
    public GameObject Player2Camera;
    public static AvatarCamera Instance { get; private set; }



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
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnscenleLoaded;
        
    }

    private void OnscenleLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {

        StartCoroutine(WaitAndInitialize());




    }

    IEnumerator WaitAndInitialize()
    {
        yield return null;

        InitCanvasCamera();
    }

    private void InitCanvasCamera()
    {
        GameObject player1 = GameObject.FindGameObjectWithTag("Player1");
        GameObject player2 = GameObject.FindGameObjectWithTag("Player2");
        Player1Camera.GetComponent<AvatarCameraFallow>().Player = player1;
        Player2Camera.GetComponent<AvatarCameraFallow>().Player = player2;

        GameObject headAnchorObj = GameObject.FindGameObjectWithTag("Player1Avatar");
        GameObject headAnchorObj2 = GameObject.FindGameObjectWithTag("Player2Avatar");

        if (headAnchorObj != null)
        {
            Player1Camera.GetComponent<AvatarCameraFallow>().headAnchor = headAnchorObj.transform;
            Player2Camera.GetComponent<AvatarCameraFallow>().headAnchor = headAnchorObj2.transform;
        }


    }
}