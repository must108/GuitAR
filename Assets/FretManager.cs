using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class FretManager : MonoBehaviour
{

    [SerializeField] public bool test = false;
    private float timer = 0f;       // Timer to track time elapsed
    private readonly float interval = 1f;    // Interval of 5 seconds
    private int counter = 0;
    void LoadFretPositions(int[,] fretArray)
    {
        // Loop through all children of this GameObject
        foreach (Transform child in transform)
        {
            // Access the child GameObject
            FretSphere fretSphere = child.gameObject.GetComponent<FretSphere>();
            int rowIndex = fretSphere.sphereRowIndex;
            int colIndex = fretSphere.sphereColIndex;
            if (rowIndex < fretArray.GetLength(0) && colIndex < fretArray.GetLength(1))
            {
                if (fretArray[rowIndex, colIndex] == 1)
                {
                    child.gameObject.SetActive(true);
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }

            // You can perform any operations on the child here
        }
    }

    void Update()
    {
        
        if (test == true)
        {
            print("fasdfad");
            // Add the time since the last frame to the timer
            timer += Time.deltaTime;
            print(timer);


            // Check if the timer has reached or exceeded the interval (5 seconds)
            if (timer >= interval)
            {
                // print("Hi this is running");
                // counter %= 4;
                int[,] array1 = new int[4, 6]
                {
                {0,1,0,0,0,0},
                {0,0,0,1,0,0},
                {0,0,0,0,0,1},
                {0,1,0,0,0,0}
                };

                int[,] array2 = new int[4, 6]
                {
                {0,0,0,1,0,0},
                {0,0,0,0,0,0},
                {0,0,1,0,0,0},
                {0,0,0,0,0,1},
                };

                int[,] array3 = new int[4, 6]
                {
                {0,0,0,0,0,0},
                {0,0,0,1,0,0},
                {0,1,0,0,0,0},
                {0,0,0,0,0,1},
                };

                int[,] array4 = new int[4, 6]
                {
                {0,0,0,0,0,0},
                {0,0,0,0,0,0},
                {0,0,0,0,0,0},
                {0,0,0,0,0,0},
                };

                if (counter == 0) LoadFretPositions(array1);
                else if (counter == 1) LoadFretPositions(array2);
                else if (counter == 2) LoadFretPositions(array3);
                else if (counter == 3) LoadFretPositions(array4);

                counter = (counter + 1) % 4;

                // Reset the timer
                timer = 0f;
            }
        }
    }


}
