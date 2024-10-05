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
    public int spectrumSize = 1024;
    public float[] spectrumData;
    public float referenceFrequency = 440.0f; // A4 = 440Hz
    public string[] noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

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
            selectedMic = "Analogue 1 + 2 (Focusrite USB Audio)";        // "Android audio input"
            currentClip = Microphone.Start(selectedMic, true, 1, 44100);
            audioInterface.clip = currentClip;
            audioInterface.loop = true;   // Ensure loop is enabled for continuous play
            audioInterface.Play();


            spectrumData = new float[spectrumSize];
            Debug.Log("selected: " + selectedMic);
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

            // Perform frequency analysis
            audioInterface.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);
            float dominantFrequency = GetDominantFrequency();
            int midiNote = FrequencyToMidi(dominantFrequency);
            string noteName = MidiToNoteName(midiNote);

            Debug.Log("Dominant Frequency: " + dominantFrequency + " Hz, Closest Note: " + noteName);
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

    // Get the dominant frequency from the spectrum data
    float GetDominantFrequency()
    {
        float maxAmplitude = 0f;
        int maxIndex = 0;

        for (int i = 0; i < spectrumSize; i++)
        {
            if (spectrumData[i] > maxAmplitude)
            {
                maxAmplitude = spectrumData[i];
                maxIndex = i;
            }
        }

        float dominantFrequency = maxIndex * AudioSettings.outputSampleRate / 2 / spectrumSize;
        return dominantFrequency;
    }

    int FrequencyToMidi(float frequency)
    {
        if (frequency <= 0) return -1; // Invalid frequency

        // Calculate the MIDI note number from frequency
        float midiNoteFloat = 12 * Mathf.Log(frequency / referenceFrequency, 2) + 69;
        return Mathf.RoundToInt(midiNoteFloat); // Round to nearest MIDI note number
    }

    string MidiToNoteName(int midiNote)
    {
        int noteIndex = (midiNote - 12) % 12; // Modulo to wrap around the 12 notes in an octave
        int octave = (midiNote / 12) - 1; // Calculate the octave number

        return noteNames[noteIndex] + octave; // Combine note name and octave
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
