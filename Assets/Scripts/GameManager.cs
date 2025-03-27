using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public int level = 1;
    public int hitpoints = 20;
    public int money = 500;
    private int currentLevel = 0;
    [HideInInspector]
    public int enemiesKilled = 0;

    public static GameManager Instance;
    public GameObject enemySpawnerPurple;
    public GameObject enemySpawnerYellow;
    public GameObject enemySpawnerRed;
    public GameObject enemySpawnerGreen;
    public GameObject enemySpawnerOrange;

    private EnemySpawner enemySpawner;
    public TextMeshProUGUI moneyDisplay;
    public TextMeshProUGUI hitpointsDisplay;
    public TextMeshProUGUI levelDisplay;
    public TextMeshProUGUI enemiesKilledDisplay;
    public GameObject gameOverPanel;
    

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
        levelDisplay.text = currentLevel + "";
        enemiesKilledDisplay.text = enemiesKilled + "";

        if(hitpoints <= 0)
        {
            gameOver();
        }
    }

    public void StartGame()
    {
        switch(currentLevel)
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
                break;
        }
    }


    public void goToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void gameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }


    public void newGame()
    {
        SceneManager.LoadScene("Game");
    }
}
