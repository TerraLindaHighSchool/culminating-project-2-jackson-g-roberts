using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 10;

    public GameObject followCamera, powerUpIndicator;

    public List<GameObject> powerUpIndicatorPrefabs;

    public PowerUp.Type powerUpState;

    private Rigidbody rb;

    private GameManager gameManager;
    
    void Start()
    {
        powerUpState = PowerUp.Type.NONE;
        
        rb = GetComponent<Rigidbody>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        powerUpIndicator.transform.position = transform.position;

        if (transform.position.y <= -10)
        {
            gameManager.gameOver = true;
            powerUpState = PowerUp.Type.NONE;
            UpdatePowerUpIndicator();
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        float cameraDirection = followCamera.GetComponent<CameraController>().viewAngleX;

        if (Input.GetKey("w")) rb.AddForce(createVectorFromDirection(cameraDirection) * speed);
        if (Input.GetKey("s")) rb.AddForce(-createVectorFromDirection(cameraDirection) *  speed);
        if (Input.GetKey("a"))
        {
            if (cameraDirection - 90.0f <= -180.0f)
            {
                rb.AddForce(-createVectorFromDirection(cameraDirection - 270.0f) * speed);
            }
            else
            {
                rb.AddForce(createVectorFromDirection(cameraDirection - 90.0f) * speed);
            }
        }
        if (Input.GetKey("d"))
        {
            if (cameraDirection + 90.0f > 180.0f)
            {
                rb.AddForce(createVectorFromDirection(cameraDirection - 270.0f) * speed);
            }
            else
            {
                rb.AddForce(createVectorFromDirection(cameraDirection + 90.0f) * speed);
            }
        }
    }

    Vector3 createVectorFromDirection(float direction)
    {
        return new Vector3(Mathf.Sin(direction * Mathf.Deg2Rad), 0.0f, Mathf.Cos(direction * Mathf.Deg2Rad));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Power Up"))
        {
            if (powerUpState == PowerUp.Type.NONE)
            {
                PowerUp powerUp = other.GetComponent<PowerUp>();
                powerUpState = powerUp.type;
                UpdatePowerUpIndicator();
                Destroy(other.gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (powerUpState == PowerUp.Type.BOUNCE)
            {
                List<GameObject> nearbyEnemies = new List<GameObject>();
                foreach (GameObject enemyObject in GameObject.FindGameObjectsWithTag("Enemy")) if (Vector3.Distance(transform.position, enemyObject.transform.position) <= 5) nearbyEnemies.Add(enemyObject);
                if (!nearbyEnemies.Contains(collision.gameObject)) nearbyEnemies.Add(collision.gameObject);
                foreach (GameObject enemyObject in nearbyEnemies) enemyObject.GetComponent<Rigidbody>().AddExplosionForce(50.0f, transform.position, 5.0f, 1.0f, ForceMode.Impulse);
                powerUpState = PowerUp.Type.NONE;
                UpdatePowerUpIndicator();
            }
        }
        if (collision.gameObject.CompareTag("Boss"))
        {
            collision.gameObject.GetComponent<BossController>().isHit(gameObject);
            if (powerUpState == PowerUp.Type.BOUNCE)
            {
                List<GameObject> nearbyEnemies = new List<GameObject>();
                foreach (GameObject enemyObject in GameObject.FindGameObjectsWithTag("Enemy")) if (Vector3.Distance(transform.position, enemyObject.transform.position) <= 5) nearbyEnemies.Add(enemyObject);
                if (!nearbyEnemies.Contains(collision.gameObject)) nearbyEnemies.Add(collision.gameObject);
                foreach (GameObject enemyObject in nearbyEnemies) enemyObject.GetComponent<Rigidbody>().AddExplosionForce(50.0f, transform.position, 5.0f, 1.0f, ForceMode.Impulse);
                powerUpState = PowerUp.Type.NONE;
                UpdatePowerUpIndicator();
            }
        }
    }

    void UpdatePowerUpIndicator()
    {
        switch (powerUpState)
        {
            case PowerUp.Type.NONE:
                powerUpIndicator.GetComponent<MeshFilter>().mesh = null;
                break;
            case PowerUp.Type.BOUNCE:
                powerUpIndicator.GetComponent<MeshFilter>().mesh = powerUpIndicatorPrefabs[0].GetComponent<MeshFilter>().sharedMesh;
                break;
        }
    }
}
