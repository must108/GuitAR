using UnityEngine;

public abstract class Lesson : MonoBehaviour {
    public abstract string[] GetNotes();
    public abstract int[][,] GetFretPositions();
    public abstract float GetInterval();
}