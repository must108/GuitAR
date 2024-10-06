using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndDisplayCurrentNote : MonoBehaviour
{
    [SerializeField] DisplayNoteManager displayNote;
    [SerializeField] NewAudioScript audioScript;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        string text = audioScript.theyPlayed.GetTheyPlayed().GetIsBeingPlayed();
        if (text != null)
        {
            if (text.Equals("Unknown")) text = "";
            displayNote.setText(text);
        }

    }
}
