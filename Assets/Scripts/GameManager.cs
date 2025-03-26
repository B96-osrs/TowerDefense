using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; 
    public int level = 1;
    public int hitpoints = 20;
    public int money = 500;
    public TextMeshProUGUI moneyDisplay;
    public TextMeshProUGUI hitpointsDisplay;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    private void Update()
    {
        moneyDisplay.text = "Coins: " + money;
        hitpointsDisplay.text = hitpoints + "";
    }
}
