using System.Collections;
using System.Collections.Generic;
using Core3lb;
using UnityEngine;

public class AudioProcessor : MonoBehaviour
{
    private AudioClip currentClip;
    private AudioSource audioInterface;
    private string selectedMic;
    public int sampleWindow = 128;
    public GameObject colorChanger;
    // Start is called before the first frame update
    void Start()
    {
        audioInterface = GetComponent<AudioSource>();

        if (Microphone.devices.Length > 0)
        {
            Debug.Log("Available Microphones:");
            foreach (string device in Microphone.devices)
            {
                Debug.Log(device);
            }
            selectedMic = Microphone.devices[0];        // Android audio input
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
            Debug.Log("Available Microphones:");
            foreach (string device in Microphone.devices)
            {
                Debug.Log(device);
            }
            Debug.Log(selectedMic);
            Debug.Log(level);
        }
    }

    float GetAudioLevel()
    {
        float[] data = new float[sampleWindow];
        int micPosition = Microphone.GetPosition("") - sampleWindow + 1;
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
