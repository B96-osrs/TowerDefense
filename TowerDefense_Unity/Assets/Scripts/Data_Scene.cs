using UnityEngine;
using UnityEngine.SceneManagement;

public class Data_Scene : MonoBehaviour
{
    public void backToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
