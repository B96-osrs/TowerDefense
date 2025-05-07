using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void playGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void showInfo()
    {
        SceneManager.LoadScene("Info");
    }

    public void quitGame()
    {
        Application.Quit();
    }

}
