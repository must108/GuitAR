using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLessonManager : MonoBehaviour
{
    [SerializeField] private GameObject subLessons;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject goBackButton;
    [SerializeField] private Music lesson1;
    [SerializeField] private Music lesson2;
    [SerializeField] private Music lesson3;
    [SerializeField] private Music lesson4;

    [SerializeField] private LessonManager lessonManager;

    public void StartLessonEddie()
    {
        subLessons.SetActive(false);
        goBackButton.SetActive(false);
        exitButton.SetActive(true);
        lessonManager.LoadLesson(lesson1);
    }
    public void StartLessonGood()
    {
        subLessons.SetActive(false);
        goBackButton.SetActive(false);
        exitButton.SetActive(true);
        lessonManager.LoadLesson(lesson2);
    }
    public void StartLessonCMajor()
    {
        subLessons.SetActive(false);
        goBackButton.SetActive(false);
        exitButton.SetActive(true);
        lessonManager.LoadLesson(lesson3);
    }
    public void StartLessonGmajor()
    {
        subLessons.SetActive(false);
        goBackButton.SetActive(false);
        exitButton.SetActive(true);
        lessonManager.LoadLesson(lesson4);
    }
    public void ExitSubLesson()
    {
        subLessons.SetActive(true);
        exitButton.SetActive(false);
        goBackButton.SetActive(true);
        lessonManager.Abort();
    }

    void Update()
    {
        if(lessonManager.IsDone())
        {
            ExitSubLesson();
        }
    }

}
