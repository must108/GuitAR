using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class LessonManager : MonoBehaviour
{
    private Music currentLesson;
    private string[] currentNotes;
    private bool abort = false;
    private bool lessonPlaying = false;
    private bool isDone = false;
    [SerializeField] private GameObject guitarObjectSpheres;
    [SerializeField] private DisplayNoteManager displayNote;

    [SerializeField] private DisplayNoteManager lessonDisplayNote;
    private int count = 0;
    private float timer = 0f;
    public void Abort()
    {
        abort = true;
    }
    public void LoadFretPositions(int[,] fretArray)
    {
        // Loop through all children of this GameObject
        foreach (Transform child in guitarObjectSpheres.transform)
        {
            // Access the child GameObject
            FretSphere fretSphere = child.gameObject.GetComponent<FretSphere>();
            if (fretSphere == null) continue; // Safeguard against missing component

            int rowIndex = fretSphere.sphereRowIndex;
            int colIndex = fretSphere.sphereColIndex;
            if (rowIndex < fretArray.GetLength(0) && colIndex < fretArray.GetLength(1))
            {
                child.gameObject.SetActive(fretArray[rowIndex, colIndex] == 1);
            }
        }
    }

    public bool IsDone()
    {
        if (isDone)
        {
            isDone = false;
            return true;
        }
        return false;
    }

    public void LoadLesson(Music lesson)
    {
        currentLesson = lesson;
        currentNotes = currentLesson.GetNotes();
        count = 0;
        lessonPlaying = true;
        LoadFretPositions(Music.NoteObjects[currentNotes[count]]);
    }

    void Update()
    {
        if (abort)
        {
            abort = false;
            lessonDisplayNote.setText("Play This Note!");
            lessonPlaying = false;
            LoadFretPositions(new int[,]{
                { 0, 0, 0, 0, 0, 0}, { 0, 0, 0, 0, 0, 0}, { 0, 0, 0, 0, 0, 0},{ 0, 0, 0, 0, 0, 0},
            });
        }

        if (lessonPlaying && currentNotes != null && count < currentNotes.Length)
        {
            lessonDisplayNote.setText(currentNotes[count]);
            if (currentNotes[count].Equals(displayNote.getText()))
            {
                displayNote.SetCorrect();
                count++;
                if (count >= currentNotes.Length)
                {
                    lessonDisplayNote.setText("Play This Note!");
                    isDone = true;
                    abort = true;
                }
                else
                {
                    LoadFretPositions(Music.NoteObjects[currentNotes[count]]);
                }
                

            }
            else if (count >= currentNotes.Length)
            {
                lessonDisplayNote.setText("Play This Note!");
                isDone = true;
                abort = true;
            }
        }
    }
}