using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class start_button : MonoBehaviour
{
    // Speed of rotation
    public float rotationSpeed = 20f;
    // Boolean to track if the button is clicked
    private bool isClicked = false;

    // Start is called before the first frame update
    void Start()
    {
        // Print a message to the console
        Debug.Log("Hello World!");
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the button if it is clicked
        if (isClicked)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        } else {
            transform.Rotate(Vector3.up, 20 * Time.deltaTime);
        }
    }

    // Method called when the button is selected in VR
    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        Debug.Log("Button selected!");
        isClicked = true;
    }
}