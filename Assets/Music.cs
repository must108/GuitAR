using UnityEngine;

public abstract class Music : MonoBehaviour {

    public static float interval = 1f;
    public abstract string[] GetNotes();
    public abstract int[][,] GetFretPositions();
    public static float GetInterval() {
        return interval;
    }
}