using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public float speed;

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

    public void isHit(GameObject player)
    {
        numOfHits++;
        GetComponent<Rigidbody>().mass--;
        GetComponent<Rigidbody>().AddExplosionForce(numOfHits * 10.0f, player.transform.position, 10.0f, numOfHits, ForceMode.Impulse);
    }
}
