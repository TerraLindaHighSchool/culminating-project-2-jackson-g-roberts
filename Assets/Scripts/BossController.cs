using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public float speed;

    public float playerLaunchForce = 2.0f;
    public float playerLaunchUpForce = 1.0f;

    private Rigidbody rb;
    private GameObject player;

    private GameManager gameManager;

    private int numOfHits;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (!gameManager.gameOver)
        {
            Vector3 lookDirection = (player.transform.position - transform.position).normalized;
            rb.AddForce(lookDirection * speed);
        }
    }

    public Vector3 GetPlayerLaunchVector(GameObject player)
    {
        Vector3 vectorFromBoss = (player.transform.position - transform.position);
        return new Vector3(vectorFromBoss.x * playerLaunchForce, playerLaunchUpForce, vectorFromBoss.z * playerLaunchForce);
    }

    public void isHit(GameObject player)
    {
        numOfHits++;
        GetComponent<Rigidbody>().mass--;
        GetComponent<Rigidbody>().AddExplosionForce(numOfHits * 15.0f, player.transform.position, 15.0f, numOfHits, ForceMode.Impulse);
    }
}
