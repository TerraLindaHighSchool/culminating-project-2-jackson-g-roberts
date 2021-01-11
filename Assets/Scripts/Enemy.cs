using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 5.0f;

    public float leapForce = 5.0f;

    public float leapUpForce = 5.0f;

    public Type type;

    private Rigidbody rb;
    private GameObject player;

    private GameManager gameManager;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        if (type == Type.BIG)
        {
            transform.localScale *= 3;
            rb.mass *= 2;
        }

        if (type == Type.LEAP) InvokeRepeating("Leap", 1.0f, 2.0f);
    }

    void FixedUpdate()
    {
        if (!gameManager.gameOver)
        {
            Vector3 lookDirection = (player.transform.position - transform.position).normalized;
            rb.AddForce(lookDirection * speed);
        }
    }

    void Leap()
    {
        if (!gameManager.gameOver)
        {
            Vector3 lookDirection = (player.transform.position - transform.position).normalized;
            rb.AddForce(new Vector3(lookDirection.x, lookDirection.y + leapUpForce, lookDirection.z) * leapForce, ForceMode.Impulse);
        }
    }

    public enum Type
    {
        NORMAL,
        BIG,
        LEAP
    }
}
