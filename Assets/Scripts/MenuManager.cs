using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] public GameObject playMenu;
    [SerializeField] public GameObject lessonMenu;
    [SerializeField] public GameObject simpleLessonMenu;
    [SerializeField] public GameObject songLessonMenu;
    [SerializeField] GameObject guitar;

    // Start is called before the first frame update
    void Start()
    {
        playMenu.SetActive(true);
        lessonMenu.SetActive(false);
    }

    public void PlayMenu()
    {
        playMenu.SetActive(true);
        lessonMenu.SetActive(false);
    }

    public void LessonMenu()
    {
        playMenu.SetActive(false);
        lessonMenu.SetActive(true);
        guitar.SetActive(false);
        simpleLessonMenu.SetActive(false);
        songLessonMenu.SetActive(false);
    }

    public void StartSimpleLesson()
    {
        lessonMenu.SetActive(false);
        simpleLessonMenu.SetActive(true);
        guitar.SetActive(true);
        Debug.Log("Start Simple Lesson");
    }

    public void StartSongLesson()
    {
        lessonMenu.SetActive(false);
        songLessonMenu.SetActive(true);
        Debug.Log("Start Song Lesson");
    }
}
