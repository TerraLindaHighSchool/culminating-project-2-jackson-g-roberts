using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 10;

    public float launchSpeed = 15;

    public float launchUpForce = 5;

    public bool isCollideable;

    public GameObject followCamera, powerUpIndicator;

    public List<GameObject> powerUpIndicatorPrefabs;

    public PowerUp.Type powerUpState;

    private Rigidbody rb;

    private GameManager gameManager;
    
    void Start()
    {
        powerUpState = PowerUp.Type.NONE;

        isCollideable = true;
        
        rb = GetComponent<Rigidbody>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        powerUpIndicator.transform.position = transform.position;
        powerUpIndicator.transform.Rotate(new Vector3(0.0f, 15.0f, 0.0f) * Time.deltaTime);

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
        float cameraVertDirection = followCamera.GetComponent<CameraController>().viewAngleY;

        if (Input.GetKey("w")) rb.AddForce(createVectorFromDirection(cameraDirection, false) * speed);
        if (Input.GetKey("s")) rb.AddForce(-createVectorFromDirection(cameraDirection, false) *  speed);
        if (Input.GetKey("a"))
        {
            if (cameraDirection - 90.0f <= -180.0f)
            {
                rb.AddForce(-createVectorFromDirection(cameraDirection - 270.0f, false) * speed);
            }
            else
            {
                rb.AddForce(createVectorFromDirection(cameraDirection - 90.0f, false) * speed);
            }
        }
        if (Input.GetKey("d"))
        {
            if (cameraDirection + 90.0f > 180.0f)
            {
                rb.AddForce(createVectorFromDirection(cameraDirection - 270.0f, false) * speed);
            }
            else
            {
                rb.AddForce(createVectorFromDirection(cameraDirection + 90.0f, false) * speed);
            }
        }

        if (Input.GetKey("space"))
        {
            if (powerUpState == PowerUp.Type.LAUNCH)
            {
                rb.AddForce(Vector3.up * launchUpForce, ForceMode.Impulse);
                rb.AddForce(createVectorFromDirection(cameraDirection, false) * launchSpeed, ForceMode.Impulse);
                powerUpState = PowerUp.Type.NONE;
                UpdatePowerUpIndicator();
            }

            if (powerUpState == PowerUp.Type.NOCLIP)
            {
                isCollideable = false;
                rb.useGravity = false;
                GetComponent<SphereCollider>().enabled = false;
                GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                powerUpState = PowerUp.Type.NONE;
                UpdatePowerUpIndicator();
                StartCoroutine(NoclipCountdownRoutine());
            }
        }
    }

    IEnumerator NoclipCountdownRoutine()
    {
        yield return new WaitForSeconds(5);
        isCollideable = true;
        rb.useGravity = true;
        GetComponent<SphereCollider>().enabled = true;
        GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    Vector3 createVectorFromDirection(float direction, bool useVert)
    {
        return new Vector3(Mathf.Sin(direction * Mathf.Deg2Rad), useVert ? Mathf.Sin(followCamera.GetComponent<CameraController>().viewAngleY * Mathf.Deg2Rad) : 0.0f, Mathf.Cos(direction * Mathf.Deg2Rad));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Power Up") && isCollideable)
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
            rb.AddForce(collision.gameObject.GetComponent<BossController>().GetPlayerLaunchVector(gameObject), ForceMode.Impulse);
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

    public void UpdatePowerUpIndicator()
    {
        if (powerUpState == PowerUp.Type.NONE)
        {
            powerUpIndicator.GetComponent<MeshFilter>().mesh = null;
            return;
        }

        powerUpIndicator.GetComponent<MeshFilter>().mesh = powerUpIndicatorPrefabs[(int) powerUpState - 1].GetComponent<MeshFilter>().sharedMesh;
    }
}
