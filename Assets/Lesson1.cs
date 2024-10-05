using UnityEngine;

public class Lesson1 : Lesson {
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