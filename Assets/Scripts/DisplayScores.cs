using UnityEngine;
using UnityEngine.UI;

public class DisplayScores : MonoBehaviour {

    [SerializeField] Text score;
    [SerializeField] Text highestScore;

    void Start () {
        if (PlayerPrefs.HasKey("score"))
        {
            score.text = PlayerPrefs.GetInt("score").ToString();
        }
        else
        {
            score.text = "0";
        }

        if (PlayerPrefs.HasKey("highestScore"))
        {
            highestScore.text = PlayerPrefs.GetInt("highestScore").ToString();
        }
        else
        {
            highestScore.text = "0";
        }
    }
}
