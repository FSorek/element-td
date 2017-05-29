using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour {

public void exitGame()
    {
        Application.Quit();
    }

    public void restartGame()
    {
        SceneManager.LoadScene("MainScene");
    }
}
