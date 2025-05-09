using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    private int amountOfEnemies;
    public int maxEnemies;
    public float spawnRate;
    private float timer = 0;
    private Tilemap tilemap;

    void Start()
    {     
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        spawnEnemy();
        amountOfEnemies = 1;
    }

    private void Update()
    {

        if (timer < spawnRate)
        {
            timer = timer + Time.deltaTime;

        }
        else if (amountOfEnemies < maxEnemies)
        {
            spawnEnemy();
            timer = 0;
            amountOfEnemies++;

        }

    }

    private void spawnEnemy()
    {
        Debug.Log("Enemy Spawned at: " + transform.position);
        GameObject newEnemy = Instantiate(enemyPrefab, new Vector3Int(-9, 3, 0) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
        Enemy_BFS enemyScript = newEnemy.GetComponent<Enemy_BFS>();
        if (enemyScript != null)
        {
            Debug.Log("Enemy script found");
            enemyScript.tilemap = tilemap;
        }
        Enemy_Left_Only enemyScriptLeftOnly = newEnemy.GetComponent<Enemy_Left_Only>();
        if (enemyScriptLeftOnly != null)
        {
            Debug.Log("Enemy_Left_Only script found");
            enemyScriptLeftOnly.tilemap = tilemap;
        }
        Enemy_Right_Only enemyScriptRightOnly = newEnemy.GetComponent<Enemy_Right_Only>();
        if (enemyScriptRightOnly != null)
        {
            Debug.Log("Enemy_Right_Only script found");
            enemyScriptRightOnly.tilemap = tilemap;
        }
        Enemy_Random enemyScriptRandom = newEnemy.GetComponent<Enemy_Random>();
        if (enemyScriptRandom != null)
        {
            Debug.Log("Enemy_Random script found");
            enemyScriptRandom.tilemap = tilemap;
        }
        Enemy_A_Star enemyScriptAStar = newEnemy.GetComponent<Enemy_A_Star>();
        if (enemyScriptAStar != null)
        {
            Debug.Log("Enemy_A_Star script found");
            enemyScriptAStar.tilemap = tilemap;
        }
        Enemy_AI enemyScriptAI = newEnemy.GetComponent<Enemy_AI>();
        if (enemyScriptAI != null)
        {
            Debug.Log("Enemy_AI script found");
            enemyScriptAI.tilemap = tilemap;
        }

    }


}
