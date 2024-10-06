using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] public GameObject playMenu;
    [SerializeField] public GameObject lessonMenu;

    [SerializeField] public GameObject playMenuGrabber;
    [SerializeField] public GameObject lessonMenuGrabber;
    // MENU_STATE = 0: Main Menu, 1: Lesson Selector
    [SerializeField] public int MENU_STATE = 0;

    // Start is called before the first frame update
    void Start()
    {
        MENU_STATE = 0;

        playMenu.SetActive(true);
        lessonMenu.SetActive(false);

        playMenuGrabber.SetActive(true);
        lessonMenuGrabber.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayMenu()
    {
        playMenu.SetActive(true);
        lessonMenu.SetActive(false);

        playMenuGrabber.SetActive(true);
        lessonMenuGrabber.SetActive(false);
        MENU_STATE = 0;
    }

    public void LessonMenu() 
    {
        playMenu.SetActive(false);
        lessonMenu.SetActive(true);

        playMenuGrabber.SetActive(false);
        lessonMenuGrabber.SetActive(true);
        MENU_STATE = 1;
    }

    public void StartSimpleLesson()
    {
        lessonMenu.SetActive(false);
        lessonMenuGrabber.SetActive(false);
        MENU_STATE = 2;
        Debug.Log("Start Simple Lesson");
    }

    public void StartSongLesson()
    {
        lessonMenu.SetActive(false);
        lessonMenuGrabber.SetActive(false);
        MENU_STATE = 2;
        Debug.Log("Start Song Lesson");
    }

    public void StartCustomSongLesson()
    {
        lessonMenu.SetActive(false);
        lessonMenuGrabber.SetActive(false);
        MENU_STATE = 2;
        Debug.Log("Start Custom Song Lesson");
    }
}
