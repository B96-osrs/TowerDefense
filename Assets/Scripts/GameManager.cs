using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject enemySpawnerPrefab;
    public int level = 1;
    public int hitpoints = 20;
    public int money = 500;
    private EnemySpawner enemySpawner;
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
        moneyDisplay.text = money + "";
        hitpointsDisplay.text = hitpoints + "";
    }

    public void StartGame()
    {
        Instantiate(enemySpawnerPrefab).GetComponent<EnemySpawner>();
    }
}
