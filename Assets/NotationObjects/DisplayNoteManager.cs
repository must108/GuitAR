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
    private float timer = 0f;

    void Start()
    {
        thisRenderer =  transform.GetComponent<Renderer>();
    }
    public void setText(string inputText)
    {
        text.text = inputText;
    }

    public string getText()
    {
        return text.text;
    }

    public void SetIncorrect()
    {
        setIncorrect = false;
        thisRenderer.material.color = incorrect;
        text.color = incorrect;
        text.alpha = 1f; // Ensure the alpha is fully opaque
    }

    public void SetCorrect()
    {
        setCorrect = true;
        thisRenderer.material.color = correct;
        text.color = correct;
        text.alpha = 1f; // Ensure the alpha is fully opaque
    }

    public void SetDefault()
    {
        setDefault = true;
        thisRenderer.material.color = defaultColor;
        text.color = defaultColor;
        text.alpha = 1f; // Ensure the alpha is fully opaque
    }


    void Update()
    {
        if (setCorrect || setIncorrect || setDefault)
        {
            timer += Time.deltaTime;
            if (timer >= 1f)
            {
                SetDefault();
                setCorrect = false;
                setIncorrect = false;
                setDefault = false;
                timer = 0f;
            }
        }

    }
}
