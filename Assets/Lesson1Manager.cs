using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson1Manager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject SubLessons;
    [SerializeField] GameObject ExitButton;
    void Start()
    {
        
    }

    public void StartSubLesson()
    {
        SubLessons.SetActive(false);
        ExitButton.SetActive(true);
    }

    public void EndSubLesson()
    {
        SubLessons.SetActive(true);
        ExitButton.SetActive(false);
    }

}
