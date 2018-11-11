using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonsLostScene : MonoBehaviour {

    public void Retry()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Menu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
