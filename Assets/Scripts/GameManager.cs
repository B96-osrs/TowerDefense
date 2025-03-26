using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; 
    public int level = 1;
    public int hitpoints = 100;
    public int money = 500;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
