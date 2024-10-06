using System.Threading;
using UnityEngine;
using UnityEngine.Video;

public class LessonManager : MonoBehaviour {
    public Music currentLesson;
    public string[] currentNotes;
    private int count = 0;
    private float timer = 0f;

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

    public void LoadLesson(Music lesson) {
        currentLesson = lesson;
        currentNotes = currentLesson.GetNotes();
        count = 0;
        LoadFretPositions(Music.NoteObjects[currentNotes[count]]);
    }

    void Start() {
        LoadLesson(new Lesson2());
    }

    void Update() {
        if (count < currentNotes.Length) {
            timer += Time.deltaTime;
            if (timer >= Music.GetInterval()) {
                count += 1;
                LoadFretPositions(Music.NoteObjects[currentNotes[count]]);
                timer = 0f;
            }
        }
    }
}