using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LessonButtonHandler : MonoBehaviour {
    public void OnLessonButtonClick() {
        string buttonName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        Debug.Log("Button clicked: ", buttonName);

        if (buttonName === "Lesson1Button") {
            SceneManager.LoadScene("Lesson1");
        } else if (buttonName === "Lesson2Button") {
            SceneManager.LoadScene("Lesson2");
        } else if (buttonName === "Lesson3Button") {
            SceneManager.LoadScene("Lesson3");
        } else if (buttonName === "Lesson4Button") {
            SceneManager.LoadScene("Lesson4");
        } else if (buttonName === "Lesson5Button") {
            SceneManager.LoadScene("Lesson5");
        } else {
            Debug.Log("Usage error!");
        }
    }
}

