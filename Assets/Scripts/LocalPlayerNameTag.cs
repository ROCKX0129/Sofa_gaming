using UnityEngine;
using TMPro;

public class LocalPlayerNameTag : MonoBehaviour
{
    public TextMeshPro textMesh;
    public bool isPlayerOne;

    void Start()
    {
        if (isPlayerOne)
            textMesh.text = PlayerPrefs.GetString("PlayerOneName", "Player 1");
        else
            textMesh.text = PlayerPrefs.GetString("PlayerTwoName", "Player 2");
    }

    void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }
}