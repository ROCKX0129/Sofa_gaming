using UnityEngine;
using TMPro;

public class EndSceneManager : MonoBehaviour
{
    public TextMeshProUGUI winnerText;

    void Start()
    {
        int winner = PlayerPrefs.GetInt("Winner", 0);

        if (winner == 1)
        {
            string name = PlayerPrefs.GetString("PlayerOneName", "Player 1");
            winnerText.text = name + " WINS!!!";
        }
        else if (winner == 2)
        {
            string name = PlayerPrefs.GetString("PlayerTwoName", "Player 2");
            winnerText.text = name + " WINS!!!";
        }
        else
        {
            winnerText.text = "Game Over";
        }
    }
}