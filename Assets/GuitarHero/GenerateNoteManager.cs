using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateNoteManager : MonoBehaviour
{
    [SerializeField] private GameObject guitarString1;
    [SerializeField] private float speed = 2f;
    Vector3 moveDirection;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 moveDirection = guitarString1.transform.TransformDirection(Vector3.forward); // (0, 0, 1)

        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
