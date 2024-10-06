using UnityEngine;

public abstract class Music : MonoBehaviour {
    public abstract string[] GetNotes();
    public abstract int[][,] GetFretPositions();
    public abstract float GetInterval();
}