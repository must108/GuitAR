using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cube_main : MonoBehaviour
{
    // Speed of rotation
    public float rotationSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the cube around its Y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}