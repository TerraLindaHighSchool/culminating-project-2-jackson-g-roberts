using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public Type type;
    
    void Start()
    {

    }
    
    void Update()
    {
        transform.Rotate(new Vector3(0.0f, 15.0f, 0.0f) * Time.deltaTime);
    }

    public enum Type
    {
        NONE,
        BOUNCE
    }
}
