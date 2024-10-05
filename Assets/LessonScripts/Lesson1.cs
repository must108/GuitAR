// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Threading;
// using Unity.VisualScripting;
// using UnityEngine;

// const[,] BINARY_NOTE = new int[4, 6] {
//     { 0, 0, 0, 0, 0, 1 },
//     { 0, 0, 0, 0, 0, 0 },
//     { 0, 0, 0, 0, 0, 0 },
//     { 0, 0, 0, 0, 0, 0 },
// }

// const PLAYED_NOTE = 'F';

// public class Lesson1 : MonoBehavior {

//     private float timer = 0f;
//     private float interval = 1f;

//     void LoadFretPositions(int[,] fretArray) {
//         foreach (Transform child in transform) {
//             FretSphere fretSphere = child.gameObject
//                                     .GetComponent<fretSphere>();
//             int rowIndex = fretSphere.sphereRowIndex;
//             int colIndex = fretSphere.sphereColIndex;

//             if (rowIndex < fretArray.GetLength(0) && 
//                 colIndex < fretArray.GetLength(1)) {
//                 if (fretArray[rowIndex, colIndex] == 1) {
//                     child.gameObject.SetActive(true);
//                 } else {
//                     child.gameObject.SetActive(false);
//                 }
//             }

//         }
//     }

//     void Update() {
//         timer += Time.deltaTime;

//         if (timer >= interval) {

//         }
//     }
// }