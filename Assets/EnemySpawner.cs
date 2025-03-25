using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Tilemap tilemap;
    private int amountOfEnemies = 0;
    private int maxEnemies = 10;
    public float spawnRate;
    private float timer = 0;

    void Start()
    {
        spawnEnemy();
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
        else
        {

        }
    }

    private void spawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemyPrefab, new Vector3Int(-9, 3, 0) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.tilemap = tilemap;
        }
    }


}
