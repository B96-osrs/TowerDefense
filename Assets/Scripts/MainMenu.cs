using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public void playGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void quitGame()
    {
        Application.Quit();
    }

}
