using System.Threading;
using UnityEngine;
using UnityEngine.Video;

public class LessonManager : MonoBehaviour {
    public Lesson currentLesson;
    public string[] currentNotes;
    public int[][,] currentFretPositions;
    private int count = 0;
    private float timer = 0f;
    private float interval;

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

    public void LoadLesson(Lesson lesson) {
        currentLesson = lesson;
        currentNotes = currentLesson.GetNotes();
        currentFretPositions = currentLesson.GetFretPositions();
        interval = currentLesson.GetInterval();
        count = 0;
        LoadFretPositions(currentFretPositions[count]);
    }

    void Start() {
        LoadLesson(new Lesson2());
    }

    void Update() {
        if (count < currentNotes.Length && count < currentFretPositions.Length) {
            timer += Time.deltaTime;
            if (timer >= interval) {
                count += 1;
                if (count < currentFretPositions.Length) {
                    LoadFretPositions(currentFretPositions[count]);
                }
                timer = 0f;
            }
        }
    }
}