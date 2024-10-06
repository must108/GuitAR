using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLessonManager : MonoBehaviour
{
    [SerializeField] private GameObject subLessons;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject goBackButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartLessonEddie()
    {
        subLessons.SetActive(false);
        goBackButton.SetActive(false);
        exitButton.SetActive(true);
    }
    public void StartLessonGood()
    {
        subLessons.SetActive(false);
        goBackButton.SetActive(false);
        exitButton.SetActive(true);
    }
    public void StartLessonCMajor()
    {
        subLessons.SetActive(false);
        goBackButton.SetActive(false);
        exitButton.SetActive(true);
    }
    public void StartLessonGmajor()
    {
        subLessons.SetActive(false);
        goBackButton.SetActive(false);
        exitButton.SetActive(true);
    }
    public void ExitSubLesson()
    {
        subLessons.SetActive(true);
        exitButton.SetActive(false);
        goBackButton.SetActive(true);
    }

}
