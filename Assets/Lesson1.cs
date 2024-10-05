using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using UnityEngine.XR.Interaction.Toolkit;

public class Lesson1 : MonoBehaviour {
    // static System.Random random = new System.Random();
    // public static int[,] ReturnRandomNote() {
    //     int[,] ReturnArray = new int[4,6];

    //     for (int i = 0; i < ReturnArray.GetLength(0); i++) {
    //         for (int j = 0; j < ReturnArray.GetLength(1); j++) {
    //             ReturnArray[i, j] = random.Next(0, 2);
    //         }
    //     }

    //     return ReturnArray;
    // }

    public void LoadFretPositions(int[,] fretArray) {
        // Loop through all children of this GameObject
        foreach (Transform child in transform) {
            // Access the child GameObject
            FretSphere fretSphere = child.gameObject.GetComponent<FretSphere>();
            if (fretSphere == null) continue; // Safeguard against missing component
            
            int rowIndex = fretSphere.sphereRowIndex;
            int colIndex = fretSphere.sphereColIndex;
            if (rowIndex < fretArray.GetLength(0) && colIndex < fretArray.GetLength(1)) {
                child.gameObject.SetActive(fretArray[rowIndex, colIndex] == 1);
            }
        }
    }

    // public static string ReturnRandomNoteName() {
    //     string[] noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
    //     return noteNames[random.Next(0, noteNames.Length)];
    // }

    private int[,] SAMPLE_OBJECT;
    private string SAMPLE_NOTE;

    private static readonly string[] Lesson1Notes = {
        "E", "F", "F#", "G", "G#",
        "A", "A#", "B", "C", "C#",
        "D", "D#", "E", "F", "F#"
    };

    private static readonly int[][,] Lesson1Objects = {
        new int [,] {
            { 0, 0, 0, 0, 0, 1 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
        },
        new int [,] {
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 1 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
        },
        new int [,] {
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 1 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
        },
        new int [,] {
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 1 },
            { 0, 0, 0, 0, 0, 0 },
        },
        new int [,] {
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 1 },
        },
        new int [,] {
            { 0, 0, 0, 0, 1, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
        },
        new int [,] {
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
        },
        new int [,] {
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
        },
        new int [,] {
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 0 },
            { 0, 0, 0, 0, 0, 0 },
        },
        new int [,] {
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 0 },
        },
        new int [,] {
            { 0, 0, 0, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
        },
        new int [,] {
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
        },
        new int [,] {
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
        },
        new int [,] {
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
        },
        new int [,] {
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 1, 0, 0 },
        },
    };

    private float timer = 0f;
    private float interval = 1f;
    private static int count = 0;

    void Start() {
        SAMPLE_OBJECT = Lesson1Objects[count];
        SAMPLE_NOTE = Lesson1Notes[count];
        LoadFretPositions(SAMPLE_OBJECT); // for the first note only
    }

    void Update() {
        timer += Time.deltaTime;
        if (timer >= interval) {
            Debug.Log("Random note: " + SAMPLE_NOTE);

            if (SAMPLE_NOTE == Lesson1Notes[count]) {
                count += 1;
                if (count >= Lesson1Notes.Length) {
                    count = 0;
                }
            }

            SAMPLE_OBJECT = Lesson1Objects[count];
            SAMPLE_NOTE = Lesson1Notes[count];
            LoadFretPositions(SAMPLE_OBJECT);
            Debug.Log("Current Note: " + SAMPLE_NOTE);
            timer = 0f;
        }
    }
};