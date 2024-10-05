using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class start_button : MonoBehaviour
{
    // Speed of rotation
    public float rotationSpeed = 100f;
    // Boolean to track if the button is clicked
    private bool isClicked = false;

    // Update is called once per frame
    void Update()
    {
        // Get play button object
        GameObject button = GameObject.Find("StartButton");
        // If the button is clicked
        if (isClicked) {
            // Rotate the button
            button.transform.Rotate(Vector3.up * (rotationSpeed * rotationSpeed) * Time.deltaTime);
            // Print a message to the console
            Debug.Log("[GAR] Button is Clicked");
        }
    }

    public void VRButtonClicked()
    {
        // Print a message to the console
        Debug.Log("[GAR] Button Clicked");
    }
}