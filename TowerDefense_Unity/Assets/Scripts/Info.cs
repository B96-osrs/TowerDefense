using UnityEngine;
using UnityEngine.SceneManagement;

public class Info : MonoBehaviour
{
    public void backToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

}
