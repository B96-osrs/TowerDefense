using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public void playGame()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void quitGame()
    {
        Application.Quit();
    }



}
