using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    // Start is called before the first frame update
    public int flyCount = 1;
    public int beetleCount = 1;
    public int wormCount = 1;
    public float flySpawnRate = 10;
    public float beetleSpawnRate = 10;
    public float wormSpawnRate = 10;
    public float flySpawnDist = 10;
    public float beetleSpawnDist = 10;
    public int wormSpawnDist = 10;
    public GameObject flyEnemyPrefab;
    public GameObject beetleEnemyPrefab;
    public GameObject wormEnemyPrefab;
    // public Camera cam;
    private bool levelActive = false;
    private float flySpawnTimer = 0;
    private float beetleSpawnTimer = 0;
    private float wormSpawnTimer = 0;
    void Start()
    {
        if (!levelActive)
        {
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!levelActive)
        {
            return;
        }
        if (GameManager.instance.gameState != GameState.PLAY)
        {
            return;
        }
        flySpawnTimer += Time.deltaTime;
        beetleSpawnTimer += Time.deltaTime;
        wormSpawnTimer += Time.deltaTime;
        if (flySpawnTimer > flySpawnRate)
        {
            spawnEnemy(flyEnemyPrefab, flySpawnDist);
            flySpawnTimer = 0;
        }
        if (beetleSpawnTimer > beetleSpawnRate)
        {
            spawnEnemy(beetleEnemyPrefab, beetleSpawnDist);
            beetleSpawnTimer = 0;
        }
        if (wormSpawnTimer > wormSpawnRate)
        {
            spawnEnemy(wormEnemyPrefab, wormSpawnDist);
            wormSpawnTimer = 0;
        }
    }

    public void startLevel()
    {
        levelActive = true;
        doFirstSpawnWave();
    }

    public void endLevel()
    {
        levelActive = false;
        flySpawnTimer = 0;
        beetleSpawnTimer = 0;
        wormSpawnTimer = 0;
        despawnEnemies();
    }

    void spawnEnemy(GameObject enemyPrefab, float radius)
    {
        float r = Random.Range(0, 1);
        if (r < 0.5)
        {
            spawnEnemyAtShip(enemyPrefab, radius);
        } else
        {
            spawnEnemyAtPortal(enemyPrefab, radius);
        }
        Debug.Log("Enemy Spawned");
    }

    void despawnEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i]);
        }
    }

    void spawnEnemyAtShip(GameObject enemyPrefab, float radius)
    {
        GameObject shipObject = GameManager.instance.shipObject;
        if (shipObject == null)
        {
            return;
        }
        // float minDist = Mathf.Max(cam.pixelWidth / 2, cam.scal / 2);
        Vector3 upperRightPos = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, -Camera.main.transform.position.z));
        Vector3 cameraPos = Camera.main.transform.position;
        cameraPos.z = 0;
        upperRightPos.z = 0;
        float minDist = Vector3.Distance(cameraPos, upperRightPos);
        Vector3 shipPos = shipObject.transform.position;
        Vector3 randomDir = Random.insideUnitSphere;
        randomDir.z = 0;
        Vector2 enemyPos = randomDir.normalized * minDist + randomDir * radius;
        Instantiate(enemyPrefab, new Vector3(enemyPos.x, enemyPos.y, 0), Quaternion.identity);
    }

    void spawnEnemyAtPortal(GameObject enemyPrefab, float radius)
    {
        GameObject portalObject = GameManager.instance.portalObject;
        if (portalObject == null)
        {
            return;
        }
        Vector3 portalPos = portalObject.transform.position;
        Vector3 randomDir = Random.insideUnitSphere;
        randomDir.z = 0;
        Vector2 enemyPos = randomDir * radius;
        Instantiate(enemyPrefab, new Vector3(enemyPos.x, enemyPos.y, 0), Quaternion.identity);
    }
    /*void spawnEnemyAtPortal(GameObject enemyPrefab, float radius)
    {
    }*/

    void doFirstSpawnWave()
    {
        for (int i = 0; i < flyCount; i++)
        {
            spawnEnemy(flyEnemyPrefab, flySpawnDist);
        }
        for (int i = 0; i < beetleCount; i++)
        {
            spawnEnemy(beetleEnemyPrefab, beetleSpawnDist);
        }
        for (int i = 0; i < wormCount; i++)
        {
            spawnEnemy(wormEnemyPrefab, wormSpawnDist);
        }
    }
}
