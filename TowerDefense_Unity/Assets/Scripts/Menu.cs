using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void playGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void showHowToPlay()
    {
        SceneManager.LoadScene("HowToPlay");
    }

    public void showStats()
    {
        SceneManager.LoadScene("Data");
    }

    public void quitGame()
    {
        Application.Quit();
    }

}
