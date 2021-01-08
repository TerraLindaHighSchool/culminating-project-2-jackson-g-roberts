using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float followDist;
    public float sensitivity;

    public float viewAngleX;
    public float viewAngleY;

    private GameManager gameManager;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void LateUpdate()
    {
        if (!gameManager.gameOver)
        {
            orbitAroundPlayer();
        }
        else
        {

        }
    }

    void orbitAroundPlayer()
    {
        float mouseMoveX = Input.GetAxis("Mouse X");
        viewAngleX += mouseMoveX * sensitivity * (1 - Time.deltaTime);

        float mouseMoveY = Input.GetAxis("Mouse Y");
        viewAngleY += mouseMoveY * sensitivity * (1 - Time.deltaTime);

        clampViewAngles();

        transform.rotation = Quaternion.Euler(-viewAngleY, viewAngleX, 0.0f);

        float maxDist = followDist;
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, transform.TransformDirection(Vector3.back), out hit, followDist))
        {
            maxDist = hit.distance - 0.5f;
            if ((hit.transform.gameObject.CompareTag("Enemy") || hit.transform.gameObject.CompareTag("Boss")) && !player.GetComponent<PlayerController>().isCollideable) maxDist = hit.distance;
        }

        Vector3 xPositionMod = new Vector3(-Mathf.Sin(viewAngleX * Mathf.Deg2Rad), 0.0f, -Mathf.Cos(viewAngleX * Mathf.Deg2Rad)) * (maxDist * Mathf.Cos(viewAngleY * Mathf.Deg2Rad));
        Vector3 yPositionMod = new Vector3(0.0f, -Mathf.Sin(viewAngleY * Mathf.Deg2Rad), 0.0f) * maxDist;
        transform.position = player.transform.position + xPositionMod + yPositionMod;
    }
    void clampViewAngles()
    {
        if (viewAngleX <= -180.0f) viewAngleX = 180.0f;
        if (viewAngleX > 180.0f) viewAngleX = -179.9f;

        if (viewAngleY < -90.0f) viewAngleY = -90.0f;
        if (viewAngleY > 90.0f) viewAngleY = 90.0f;

    }
}
