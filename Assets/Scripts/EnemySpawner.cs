using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    private int amountOfEnemies = 0;
    public int maxEnemies;
    public float spawnRate;
    private float timer = 0;
    private Tilemap tilemap;

    void Start()
    {
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
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
        Debug.Log("Enemy Spawned at: " + transform.position);
        GameObject newEnemy = Instantiate(enemyPrefab, new Vector3Int(-9, 3, 0) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.tilemap = tilemap;
        }
    }


}
