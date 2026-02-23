using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NameSaver : MonoBehaviour
{
    [Header("Player Name Fields")]
    public TMP_InputField playerOneInputField;
    public TMP_InputField playerTwoInputField;

    [Header("Scene To Load")]
    public string gameSceneName = "GameScene"; 

    private string playerOneKey = "PlayerOneName";
    private string playerTwoKey = "PlayerTwoName";

    void Start()
    {
        if (PlayerPrefs.HasKey(playerOneKey))
            playerOneInputField.text = PlayerPrefs.GetString(playerOneKey);

        if (PlayerPrefs.HasKey(playerTwoKey))
            playerTwoInputField.text = PlayerPrefs.GetString(playerTwoKey);
    }

    public void SaveNamesAndStartGame()
    {
        string playerOneName = playerOneInputField.text;
        string playerTwoName = playerTwoInputField.text;

        PlayerPrefs.SetString(playerOneKey, playerOneName);
        PlayerPrefs.SetString(playerTwoKey, playerTwoName);
        PlayerPrefs.Save();

        Debug.Log("Names Saved. Starting Game...");

        SceneManager.LoadScene(gameSceneName);
    }
}