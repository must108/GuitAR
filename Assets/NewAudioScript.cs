using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // Required for using LINQ (for averaging the list)

public class TheyPlayed
{
    public bool isNewNote;      // True when a new strum is detected
    public string isBeingPlayed;  // The musical note being played (e.g., "C#4")
    public int?[,] frets;       // Array to represent 4 frets and 6 strings. Each index is a string, each row is a fret.

    // Constructor to initialize the frets array
    public TheyPlayed()
    {
        frets = new int?[4, 6]; // Initialize the array to hold 4 frets x 6 strings
        ResetFrets();
    }

    // Reset frets to all null values when needed
    public void ResetFrets()
    {
        for (int fret = 0; fret < 4; fret++)
        {
            for (int stringIndex = 0; stringIndex < 6; stringIndex++)
            {
                frets[fret, stringIndex] = null;  // Null if the string is not being played
            }
        }
    }

    // Method to update fret data for a given string and fret
    public void UpdateFret(int fret, int stringIndex, int? value)
    {
        if (fret >= 0 && fret < 4 && stringIndex >= 0 && stringIndex < 6)
        {
            frets[fret, stringIndex] = value;  // Set the fret value, 1 = finger on fret, 0 = played open string
        }
    }

    // String representation of the TheyPlayed object, including frets and the note played
    public override string ToString()
    {
        string fretsString = "";

        for (int fret = 0; fret < 4; fret++)
        {
            fretsString += $"Fret {fret + 1}: ";
            for (int stringIndex = 0; stringIndex < 6; stringIndex++)
            {
                // Use the "?" operator to handle null values, printing "null" if it's not populated
                fretsString += frets[fret, stringIndex]?.ToString() ?? "null";
                if (stringIndex < 5) fretsString += ", "; // Add commas between strings
            }
            fretsString += "\n"; // Newline after each fret
        }

        return $"TheyPlayed [isNewNote={isNewNote}, frets=\n{fretsString}, isBeingPlayed='{isBeingPlayed}']";
    }
}



public class NewAudioScript : MonoBehaviour
{
    private AudioClip currentClip;
    private AudioSource audioInterface;
    private string selectedMic;


    public int bufferSize = 2048;
    private float[] audioBuffer; // Time domain buffer, freqs over time
    public float thresholdAmplitude = 0.000001f; // Adjust this value based on your microphone sensitivity
    public int spectrumSize = 4096; // The size of the spectrum for frequency analysis
    public float[] spectrumData; // The array to store spectrum data for frequency analysis
    public float referenceFrequency = 440.0f; // A4 reference frequency for tuning, 440 Hz
    public string[] noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" }; // Note names for MIDI values
                                                                                                     // Frequencies of guitar notes from E2 to E6
    private float[] guitarFrequencies = {
    82.41f, 87.31f, 92.50f, 98.00f, 103.83f, 110.00f, 116.54f, 123.47f, 130.81f, 138.59f,
    146.83f, 155.56f, 164.81f, 174.61f, 185.00f, 196.00f, 207.65f, 220.00f, 233.08f, 246.94f,
    261.63f, 277.18f, 293.66f, 311.13f, 329.63f, 349.23f, 369.99f, 392.00f, 415.30f, 440.00f,
    466.16f, 493.88f, 523.25f, 554.37f, 587.33f, 622.25f, 659.26f, 698.46f, 739.99f, 783.99f,
    830.61f, 880.00f, 932.33f, 987.77f, 1046.50f, 1108.73f, 1174.66f, 1244.51f, 1318.51f
};

    private string[] guitarNoteNames = {
    "E2", "F2", "F#2", "G2", "G#2", "A2", "A#2", "B2", "C3", "C#3",
    "D3", "D#3", "E3", "F3", "F#3", "G3", "G#3", "A3", "A#3", "B3",
    "C4", "C#4", "D4", "D#4", "E4", "F4", "F#4", "G4", "G#4", "A4",
    "A#4", "B4", "C5", "C#5", "D5", "D#5", "E5", "F5", "F#5", "G5",
    "G#5", "A5", "A#5", "B5", "C6", "C#6", "D6", "D#6", "E6"
};

    private TheyPlayed theyPlayed = new TheyPlayed(); // Instance of the TheyPlayed class

    // Variables for temporal smoothing and peak detection
    public Queue<float[]> spectrumHistory;
    private int smoothingWindow = 5;  // Number of recent values to average for smoothing

    void Start()
    {

        audioInterface = GetComponent<AudioSource>();

        if (Microphone.devices.Length > 0)
        {
            selectedMic = Microphone.devices[0]; // Pick the first available microphone
            Debug.Log($"Selected microphone: {selectedMic}");

            currentClip = Microphone.Start(selectedMic, true, 1, 44100);
            audioInterface.clip = currentClip;
            audioInterface.loop = true;
            audioInterface.Play();

            audioBuffer = new float[bufferSize];
            spectrumData = new float[spectrumSize]; // Initialize the spectrum data array
            spectrumHistory = new Queue<float[]>();
        }
        else
        {
            Debug.LogError("No microphone detected.");
        }
    }

    void Update()
    {
        if (Microphone.IsRecording(selectedMic))
        {
            int micPosition = Microphone.GetPosition(selectedMic) - bufferSize;
            if (micPosition < 0) return;

            currentClip.GetData(audioBuffer, micPosition);

            // Perform FFT and get spectrum data
            audioInterface.GetSpectrumData(spectrumData, 0, FFTWindow.Hanning);

            // Queue to store the spectrum history
            // Add the current spectrum data to the history
            if (spectrumHistory.Count >= smoothingWindow)
            {
                spectrumHistory.Dequeue();  // Remove the oldest frame
            }
            spectrumHistory.Enqueue((float[])spectrumData.Clone());

            // Apply temporal smoothing to the spectrum data
            float[] smoothedSpectrum = ApplyTemporalSmoothing(spectrumHistory);

            // Perform peak detection on the FFT data
            string noteName = PerformPeakDetection(smoothedSpectrum);

            if (noteName == "Unknown")
            {
                Debug.LogWarning("Unknown note detected");
            }
            else
            {
                Debug.Log($"Final Detected note: {noteName}");
            }

            // Update TheyPlayed object
            theyPlayed.isNewNote = true;
            theyPlayed.isBeingPlayed = noteName;
        }
    }


    // Function to perform temporal smoothing
    float[] ApplyTemporalSmoothing(Queue<float[]> history)
    {
        float[] smoothed = new float[spectrumSize];

        // Iterate through the history to calculate the average spectrum data
        foreach (float[] frame in history)
        {
            for (int i = 0; i < spectrumSize; i++)
            {
                smoothed[i] += frame[i];
            }
        }

        // Divide by the number of frames in history to get the average
        for (int i = 0; i < spectrumSize; i++)
        {
            smoothed[i] /= history.Count;
        }

        return smoothed;
    }


    string PerformPeakDetection(float[] spectrumData)
    {
        int windowSize = 3;
        float fundamentalFrequency = 0f;
        int fundamentalIndex = -1;
        float fundamentalAmplitude = 0f;

        // Iterate over the spectrum data within the guitar frequency range
        for (int i = windowSize; i < spectrumData.Length - windowSize; i++)
        {
            float frequency = i * AudioSettings.outputSampleRate / 2 / spectrumSize;

            if (frequency >= 82.0f && frequency <= 1318.0f)
            {
                // Check if the current bin is a peak
                bool isPeak = true;
                for (int j = 1; j <= windowSize; j++)
                {
                    if (spectrumData[i] <= spectrumData[i - j] || spectrumData[i] <= spectrumData[i + j])
                    {
                        isPeak = false;
                        break;
                    }
                }

                // Only consider peaks above the amplitude threshold
                if (isPeak && spectrumData[i] > thresholdAmplitude)
                {
                    // Check if this peak is a potential fundamental frequency
                    if (fundamentalIndex == -1 || frequency < fundamentalFrequency)
                    {
                        fundamentalFrequency = frequency;
                        fundamentalIndex = i;
                        fundamentalAmplitude = spectrumData[i];
                    }
                }
            }
        }

        // If no fundamental frequency was found, return "Unknown"
        if (fundamentalIndex == -1)
        {
            Debug.LogWarning("No valid frequency found within guitar range.");
            return "Unknown";
        }

        // Map the fundamental frequency to the closest note
        float minDifference = float.MaxValue;
        int closestIndex = -1;

        for (int i = 0; i < guitarFrequencies.Length; i++)
        {
            float diff = Mathf.Abs(fundamentalFrequency - guitarFrequencies[i]);
            if (diff < minDifference)
            {
                minDifference = diff;
                closestIndex = i;
            }
        }

        // Return the note name associated with the closest frequency
        if (closestIndex != -1)
        {
            return guitarNoteNames[closestIndex];
        }
        else
        {
            return "Unknown";
        }
    }



    void OnDisable()
    {
        // Stop the microphone recording when the object is disabled or destroyed
        if (Microphone.IsRecording(selectedMic))
        {
            Microphone.End(selectedMic);
            Debug.Log("Microphone recording stopped.");
        }
    }
}
