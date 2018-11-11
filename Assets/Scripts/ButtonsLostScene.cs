using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonsLostScene : MonoBehaviour {

    [SerializeField] int menuSceneIndex = 0;
    [SerializeField] int gameSceneIndex = 1;

    public void Retry()
    {
        SceneManager.LoadScene(gameSceneIndex);
    }

    public void Menu()
    {
        SceneManager.LoadScene(menuSceneIndex);
    }
}
