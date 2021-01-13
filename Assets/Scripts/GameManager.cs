using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab, bossPrefab, dummyPrefab, powerupPrefab;

    public GameObject island;

    public Button startGameButton, tutorialButton, backButton;

    public Image backgroundImage;

    public List<GameObject> powerUpIndicatorPrefabs;

    public int level = 0;

    public GameState gameState;

    private float spawnRange = 9.0f;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        gameState = GameState.TITLE;
        startGameButton.gameObject.SetActive(true);
        tutorialButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(false);
        backgroundImage.sprite = Resources.Load<Sprite>("mainmenu");
    }

    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0 && GameObject.FindGameObjectsWithTag("Boss").Length <= 0 && gameState == GameState.STARTED) loadNextLevel();
    }

    public void moveToTutorial()
    {
        gameState = GameState.TUTORIAL;
        startGameButton.gameObject.SetActive(false);
        tutorialButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(true);
        backgroundImage.sprite = Resources.Load<Sprite>("tutorial");
    }

    public void moveToTitle()
    {
        gameState = GameState.TITLE;
        startGameButton.gameObject.SetActive(true);
        tutorialButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(false);
        backgroundImage.sprite = Resources.Load<Sprite>("mainmenu");
    }

    public void moveToGameScene()
    {
        SceneManager.LoadScene("Game");

        gameState = GameState.STARTED;

        LoadLevel(level);
    }

    void loadNextLevel()
    {
        level++;
        foreach (GameObject powerUp in GameObject.FindGameObjectsWithTag("Power Up")) powerUp.GetComponent<PowerUp>().levelAge++;
        LoadLevel(level);
    }

    

    void LoadLevel(int levelToLoad)
    {
        switch (levelToLoad)
        {
            case 0:
                SpawnDummies(1);
                SpawnPowerUps(new[] { ChoosePowerUpType() });
                island.transform.localScale = new Vector3(10.0f, 5.0f, 10.0f);
                break;
            case 1:
                foreach (GameObject powerUp in GameObject.FindGameObjectsWithTag("Power Up")) Destroy(powerUp);
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().powerUpState = PowerUp.Type.NONE;
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().UpdatePowerUpIndicator();
                SpawnEnemies(1);
                SpawnPowerUps(new[] { ChoosePowerUpType() });
                island.transform.localScale = new Vector3(10.0f, 5.0f, 10.0f);
                break;
            case 2:
                SpawnEnemies(2);
                SpawnPowerUps(new[] { ChoosePowerUpType() });
                island.transform.localScale = new Vector3(8.0f, 5.0f, 8.0f);
                break;
            case 3:
                SpawnEnemies(4);
                SpawnPowerUps(new[] { ChoosePowerUpType(), ChoosePowerUpType() });
                island.transform.localScale = new Vector3(8.0f, 5.0f, 8.0f);
                break;
            case 4:
                SpawnEnemies(6);
                SpawnPowerUps(new[] { ChoosePowerUpType(), ChoosePowerUpType() });
                island.transform.localScale = new Vector3(6.5f, 5.0f, 6.5f);
                break;
            case 5:
                SpawnEnemies(10);
                SpawnPowerUps(new[] { ChoosePowerUpType(), ChoosePowerUpType() });
                island.transform.localScale = new Vector3(5.0f, 5.0f, 5.0f);
                break;
            case 6:
                SpawnEnemies(15);
                SpawnPowerUps(new[] { ChoosePowerUpType(), ChoosePowerUpType() });
                island.transform.localScale = new Vector3(5.0f, 5.0f, 5.0f);
                break;
            case 7:
                SpawnBosses(1);
                SpawnPowerUps(new[] { ChoosePowerUpType(), ChoosePowerUpType(), ChoosePowerUpType() });
                island.transform.localScale = new Vector3(10.0f, 5.0f, 10.0f);
                break;
            default:
                Debug.Log("Attempted to load invalid level " + levelToLoad);
                break;
        }
    }

    PowerUp.Type ChoosePowerUpType()
    {
        return (PowerUp.Type) Random.Range(1, 4);
    }

    void SpawnPowerUps(PowerUp.Type[] types)
    {
        foreach (PowerUp.Type type in types)
        {
            GameObject newPowerUpObject = Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
            newPowerUpObject.GetComponent<PowerUp>().type = type;
            newPowerUpObject.GetComponent<MeshFilter>().mesh = powerUpIndicatorPrefabs[(int) type - 1].GetComponent<MeshFilter>().sharedMesh;
        }
    }

    void SpawnEnemies(int numOfEnemies)
    {
        for (int i = 0; i < numOfEnemies; i++)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
            switch (Random.Range(0, 3))
            {
                case 0:
                    newEnemy.GetComponent<Enemy>().type = Enemy.Type.NORMAL;
                    break;
                case 1:
                    newEnemy.GetComponent<Enemy>().type = Enemy.Type.BIG;
                    break;
                case 2:
                    newEnemy.GetComponent<Enemy>().type = Enemy.Type.LEAP;
                    break;
                default:
                    newEnemy.GetComponent<Enemy>().type = Enemy.Type.NORMAL;
                    break;
            }
        }
    }

    void SpawnDummies(int numOfDummies)
    {
        //for (int i = 0; i < numOfDummies; i++) Instantiate(dummyPrefab, GenerateSpawnPosition(), dummyPrefab.transform.rotation);
        GameObject tempDummy = Instantiate(dummyPrefab, GenerateSpawnPosition(), dummyPrefab.transform.rotation);
        Debug.Log(tempDummy.transform.position);
    }

    void SpawnBosses(int numOfBosses)
    {
        for (int i = 0; i < numOfBosses; i++) Instantiate(bossPrefab, GenerateSpawnPosition(), bossPrefab.transform.rotation);
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosY = Random.Range(-spawnRange, spawnRange);
        return new Vector3(spawnPosX, 0.0f, spawnPosY);
    }

    public enum GameState
    {
        TITLE,
        TUTORIAL,
        STARTED,
        OVER
    }
}
