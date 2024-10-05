using System.Collections;
using System.Collections.Generic;
using Core3lb;
using TMPro;
using UnityEngine;

public class DisplayNoteManager : MonoBehaviour
{
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color incorrect;
    [SerializeField] private Color correct;
    [SerializeField] bool setCorrect = false;
    [SerializeField] bool setIncorrect = false;
    [SerializeField] bool setDefault = false;
    [SerializeField] TextMeshProUGUI text;
    private Renderer thisRenderer;
    void Start()
    {
       thisRenderer =  transform.GetComponent<Renderer>();
    }

    void SetIncorrect()
    {
        thisRenderer.material.color = incorrect;
        text.color = incorrect;
        text.alpha = 1f; // Ensure the alpha is fully opaque
    }

    void SetCorrect()
    {
        thisRenderer.material.color = correct;
        text.color = correct;
        text.alpha = 1f; // Ensure the alpha is fully opaque
    }

    void SetDefault()
    {
        thisRenderer.material.color = defaultColor;
        text.color = defaultColor;
        text.alpha = 1f; // Ensure the alpha is fully opaque
    }

    void Update()
    {
        if(setCorrect)
        {
            setCorrect = false;
            SetCorrect();
        }
        if(setIncorrect)
        {
            setIncorrect = false;
            SetIncorrect();
        }
        if(setDefault)
        {
            setDefault = false;
            SetDefault();
        }
    }
}
