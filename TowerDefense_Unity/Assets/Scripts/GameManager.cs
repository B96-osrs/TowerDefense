using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    // Game variables
    public int level = 0;
    public int hitpoints = 20;
    public int money = 500;
    public int blockTilesAvailable = 2;
    private bool gameIsOver = false;

    private float playedTime = 0;
    [HideInInspector]
    public int moneySpent = 0;

    [HideInInspector]
    private int currentLevel = 0;  
    [HideInInspector]
    public int enemiesKilled = 0;

    // Enemy spawner
    public static GameManager Instance;
    public GameObject enemySpawnerPurple;
    public GameObject enemySpawnerYellow;
    public GameObject enemySpawnerRed;
    public GameObject enemySpawnerGreen;
    public GameObject enemySpawnerOrange;

    // UI elements
    private EnemySpawner enemySpawner;
    public TextMeshProUGUI moneyDisplay;
    public TextMeshProUGUI blockTilesAvailableDisplay;
    public TextMeshProUGUI hitpointsDisplay;
    public TextMeshProUGUI levelDisplay;
    public TextMeshProUGUI enemiesKilledDisplay;
    public GameObject gameOverPanel;


    void Awake()
    {
        Time.timeScale = 1f;
        if (Instance == null)
        {
            Instance = this;
        }
    }


    private void Update()
    {
        playedTime += Time.deltaTime;
        moneyDisplay.text = money + "";
        hitpointsDisplay.text = hitpoints + "";
        levelDisplay.text = currentLevel + "";
        enemiesKilledDisplay.text = enemiesKilled + "";
        blockTilesAvailableDisplay.text = blockTilesAvailable + "";

        if (hitpoints <= 0 && !gameIsOver)
        {
            Debug.Log("Game Over cause hp 0");
            gameOver();
        }
    }

    public void StartGame()
    {
        switch (currentLevel)
        {
            case 0:
                enemySpawner = Instantiate(enemySpawnerPurple).GetComponent<EnemySpawner>();
                currentLevel++;
                break;
            case 1:
                enemySpawner = Instantiate(enemySpawnerYellow).GetComponent<EnemySpawner>();
                currentLevel++;
                break;
            case 2:
                enemySpawner = Instantiate(enemySpawnerRed).GetComponent<EnemySpawner>();
                currentLevel++;
                break;
            case 3:
                enemySpawner = Instantiate(enemySpawnerGreen).GetComponent<EnemySpawner>();
                currentLevel++;
                break;
            case 4:
                enemySpawner = Instantiate(enemySpawnerOrange).GetComponent<EnemySpawner>();
                currentLevel++;
                break;

            default:
                enemySpawner = Instantiate(enemySpawnerOrange).GetComponent<EnemySpawner>();
                enemySpawner = Instantiate(enemySpawnerYellow).GetComponent<EnemySpawner>();
                enemySpawner = Instantiate(enemySpawnerGreen).GetComponent<EnemySpawner>();
                currentLevel++;
                break;
        }
    }


    public void goToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void gameOver()
    {
        gameIsOver = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        StartCoroutine(this.GetComponent<Post_Data>().Upload((int)playedTime, moneySpent, currentLevel, enemiesKilled));
    }

    public void newGame()
    {
        SceneManager.LoadScene("Game");
    }

}
