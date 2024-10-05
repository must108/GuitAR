using System.Collections;
using System.Collections.Generic;
using Core3lb;
using UnityEngine;

public class AudioProcessor : MonoBehaviour
{
    private AudioClip currentClip;
    private AudioSource audioInterface;
    public int sampleWindow = 128;
    public string selectedMic;
    public GameObject colorChanger;
    // Start is called before the first frame update
    void Start()
    {
        audioInterface = GetComponent<AudioSource>();

        if (Microphone.devices.Length > 0)
        {
            selectedMic = Microphone.devices[0];
            currentClip = Microphone.Start(selectedMic, true, 1, 44100);
            audioInterface.clip = currentClip;
            audioInterface.Play();
        }
        else
        {
            Debug.LogError("AWAAA NO MIC");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Microphone.IsRecording(selectedMic))
        {
            float level = GetAudioLevel();

            if (level > 0.1f)
            {
                Debug.Log("awa 1");
            }
            else if (level > 0.05f)
            {
                Debug.Log("awa 2");
            }
            else
            {
                Debug.Log("awa 3");
            }
        }
    }

    float GetAudioLevel()
    {
        float[] data = new float[sampleWindow];
        int micPosition = Microphone.GetPosition(selectedMic) - sampleWindow + 1;
        if (micPosition < 0) return 0;

        currentClip.GetData(data, micPosition);

        // Get the average level of the samples
        float sum = 0;
        for (int i = 0; i < sampleWindow; i++)
        {
            sum += Mathf.Abs(data[i]);
        }

        return sum / sampleWindow;
    }

    void OnDisable()
    {
        // Stop the microphone when the object is disabled or destroyed
        if (Microphone.IsRecording(selectedMic))
        {
            Microphone.End(selectedMic);
        }
    }
}
