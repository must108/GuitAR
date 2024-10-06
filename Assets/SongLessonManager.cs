using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SongLessonManager : MonoBehaviour
{
    [SerializeField] private GameObject subLessons;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject goBackButton;
    [SerializeField] private Music redRiver;
    [SerializeField] private Music minute;
    [SerializeField] private Music twinkle;
    [SerializeField] private TextMeshProUGUI title;

    [SerializeField] private LessonManager lessonManager;

    public void StartRedRiver()
    {
        subLessons.SetActive(false);
        goBackButton.SetActive(false);
        exitButton.SetActive(true);
        lessonManager.LoadLesson(redRiver);
        title.text = "Red River Valley";
    }
    public void StartMinute()
    {
        subLessons.SetActive(false);
        goBackButton.SetActive(false);
        exitButton.SetActive(true);
        lessonManager.LoadLesson(minute);
        title.text = "Minute in G";
    }
    public void StartTwinkle()
    {
        subLessons.SetActive(false);
        goBackButton.SetActive(false);
        exitButton.SetActive(true);
        lessonManager.LoadLesson(twinkle);
        title.text = "Fur Elise";
    }

    public void ExitSubLesson()
    {
        subLessons.SetActive(true);
        exitButton.SetActive(false);
        goBackButton.SetActive(true);
        lessonManager.Abort();
        title.text = "Learn A Song";
    }

    void Update()
    {
        if(lessonManager.IsDone())
        {
            ExitSubLesson();
        }
    }
}
