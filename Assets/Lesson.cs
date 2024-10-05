using UnityEngine;

public class Lesson : MonoBehaviour {
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
}