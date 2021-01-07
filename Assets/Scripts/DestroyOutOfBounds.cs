using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{

    void LateUpdate()
    {
        if (transform.position.y <= -10) Destroy(gameObject);
    }
}
