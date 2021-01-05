using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 10;

    public GameObject followCamera;
    
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
}
