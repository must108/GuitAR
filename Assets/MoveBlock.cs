using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class MoveBlock : MonoBehaviour
{
    public float speed = 2.0f;  // Speed of the movement
    public float height = 2.0f; // Maximum height for the up/down movement

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;  // Save the starting position
    }

    void Update()
    {
        // Calculate the new Y position using Mathf.PingPong
        float newY = Mathf.PingPong(Time.time * speed, height);
        
        // Update the block's position while keeping the X and Z coordinates constant
        transform.position = new Vector3(startPos.x, startPos.y + newY, startPos.z);
    }
}
