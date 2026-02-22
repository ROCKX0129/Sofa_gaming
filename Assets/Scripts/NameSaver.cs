using UnityEngine;
using TMPro;

public class NameSaver : MonoBehaviour
{
    public TMP_InputField nameInputField;

    private string playerNameKey = "PlayerName";

    void Start()
    {
        // check if PlayerPrefs already has a saved name
        if (PlayerPrefs.HasKey(playerNameKey))
        {
            // Load the saved name
            string savedName = PlayerPrefs.GetString(playerNameKey);

            nameInputField.text = savedName;
        }
    }

    public void SaveName()
    {
        string playerName = nameInputField.text;

        PlayerPrefs.SetString(playerNameKey, playerName);

        PlayerPrefs.Save();

        Debug.Log("Name saved: " + playerName);
    }
}