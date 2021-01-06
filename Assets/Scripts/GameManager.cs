using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab, dummyPrefab, powerupPrefab;

    public List<GameObject> powerUpIndicatorPrefabs;

    public int level = 0;

    private float spawnRange = 9.0f;

    void Start()
    {
        LoadLevel(level);
    }

    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0) loadNextLevel();
    }

    void loadNextLevel()
    {
        level++;
        LoadLevel(level);
    }

    

    void LoadLevel(int levelToLoad)
    {
        switch (levelToLoad)
        {
            case 0:
                SpawnDummies(1);
                SpawnPowerUps(new[] { PowerUp.Type.BOUNCE });
                break;
            case 1:
                foreach (GameObject powerUp in GameObject.FindGameObjectsWithTag("Power Up")) Destroy(powerUp);
                SpawnEnemies(1);
                SpawnPowerUps(new[] { PowerUp.Type.BOUNCE });
                break;
            case 2:
                SpawnEnemies(2);
                SpawnPowerUps(new[] { PowerUp.Type.BOUNCE });
                break;
            case 3:
                SpawnEnemies(4);
                SpawnPowerUps(new[] { PowerUp.Type.BOUNCE, PowerUp.Type.BOUNCE });
                break;
            default:
                Debug.Log("Attempted to load invalid level " + levelToLoad);
                break;
        }
    }
    void SpawnPowerUps(PowerUp.Type[] types)
    {
        foreach (PowerUp.Type type in types)
        {
            GameObject newPowerUpObject = Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
            newPowerUpObject.GetComponent<PowerUp>().type = type;
            newPowerUpObject.GetComponent<MeshFilter>().mesh = powerUpIndicatorPrefabs[0].GetComponent<MeshFilter>().sharedMesh;
        }
    }

    void SpawnEnemies(int numOfEnemies)
    {
        for (int i = 0; i < numOfEnemies; i++) Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
    }

    void SpawnDummies(int numOfDummies)
    {
        for (int i = 0; i < numOfDummies; i++) Instantiate(dummyPrefab, GenerateSpawnPosition(), dummyPrefab.transform.rotation);
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosY = Random.Range(-spawnRange, spawnRange);
        return new Vector3(spawnPosX, 0.0f, spawnPosY);
    }
}
