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
    public int sampleWindow = 1024; // The number of samples for FFT
    public int spectrumSize = 1024; // The size of the spectrum for frequency analysis
    public float[] spectrumData; // The array to store spectrum data for frequency analysis
    public float referenceFrequency = 440.0f; // A4 reference frequency for tuning, 440 Hz
    public string[] noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" }; // Note names for MIDI values

    private TheyPlayed theyPlayed = new TheyPlayed(); // Instance of the TheyPlayed class
    private float[] previousBuffer = new float[1024]; // Store previous buffer for smoothing

    // Variables for temporal smoothing and peak detection
    private List<int> midiHistory = new List<int>(); // List to store recent MIDI values for smoothing
    private int midiSmoothingWindow = 5;  // Number of recent MIDI values to average for smoothing
    public float onsetThreshold = 0.2f;   // Threshold to detect the onset of a new note

    void Start()
    {
        theyPlayed.frets = new int?[4, 6]; // Initialize the frets array in the TheyPlayed object

        audioInterface = GetComponent<AudioSource>();

        if (Microphone.devices.Length > 0)
        {
            selectedMic = Microphone.devices[0]; // Pick the first available microphone
            Debug.Log($"Selected microphone: {selectedMic}");

            currentClip = Microphone.Start(selectedMic, true, 1, 44100);
            audioInterface.clip = currentClip;
            audioInterface.loop = true;
            audioInterface.Play();

            spectrumData = new float[spectrumSize]; // Initialize the spectrum data array
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
            // Get current audio level (volume)
            float level = GetAudioLevel();

            // Apply windowing (Hanning window)
            float[] buffer = new float[sampleWindow];
            audioInterface.GetOutputData(buffer, 0);

            // Perform FFT and get spectrum data
            audioInterface.GetSpectrumData(spectrumData, 0, FFTWindow.Hanning);
            // Display the spectrum data (frequency amplitudes)
            for (int i = 0; i < spectrumSize; i++)
            {
                // Log the amplitude value of each frequency band
                // Debug.Log($"Frequency Band {i}: Amplitude = {spectrumData[i]}");
            }

            // Onset detection
            if (DetectOnset(level))
            {
                // Perform peak detection on the FFT data
                float dominantFrequency = PerformPeakDetection();
                int midiNote = FrequencyToMidi(dominantFrequency);
                string noteName = MidiToNoteName(midiNote);

                Debug.Log($"{dominantFrequency}, {midiNote}, {noteName}");

                // Apply temporal smoothing
                int smoothedMidiNote = ApplyTemporalSmoothing(midiNote);
                noteName = MidiToNoteName(smoothedMidiNote);

                Debug.Log($"{smoothedMidiNote}, {noteName}");

                // Update TheyPlayed object
                theyPlayed.isNewNote = true;
                theyPlayed.isBeingPlayed = noteName;

                // Log results
                Debug.Log($"Detected note: {noteName}");
            }
            else
            {
                theyPlayed.isNewNote = false;
            }

            previousBuffer = buffer; // Save buffer for next iteration
        }
    }


    // Apply Hanning window to the buffer
    float[] ApplyWindowing()
    {
        float[] buffer = new float[sampleWindow];
        audioInterface.GetOutputData(buffer, 0);
        float[] windowedBuffer = new float[sampleWindow];

        for (int i = 0; i < buffer.Length; i++)
        {
            // Applying the Hanning window
            float hanningValue = 0.5f * (1 - Mathf.Cos(2 * Mathf.PI * i / (buffer.Length - 1)));
            windowedBuffer[i] = buffer[i] * hanningValue;
        }

        return windowedBuffer;
    }

    // Perform onset detection by comparing current and previous volume levels
    bool DetectOnset(float currentLevel)
    {
        Debug.Log("DetectOnset()");
        float previousLevel = previousBuffer.Average();

        return Mathf.Abs(currentLevel - previousLevel) > onsetThreshold;
    }


    // Perform peak detection to find the dominant frequency
    float PerformPeakDetection()
    {
        // Apply smoothing to the spectrum data before peak detection
        float[] smoothedSpectrum = SmoothSpectrum(spectrumData);

        // Perform peak detection with a sliding window
        float maxAmplitude = 0f;
        int maxIndex = 0;

        // Define a window size for peak detection (e.g., 5 neighboring values)
        int windowSize = 5;

        for (int i = windowSize; i < smoothedSpectrum.Length - windowSize; i++)
        {
            bool isPeak = true;

            // Check if the current value is greater than its neighboring values
            for (int j = 1; j <= windowSize; j++)
            {
                if (smoothedSpectrum[i] <= smoothedSpectrum[i - j] || smoothedSpectrum[i] <= smoothedSpectrum[i + j])
                {
                    isPeak = false;
                    break;
                }
            }

            // If it's a peak, and its amplitude is greater than the current maximum, update the max
            if (isPeak && smoothedSpectrum[i] > maxAmplitude)
            {
                maxAmplitude = smoothedSpectrum[i];
                maxIndex = i;
            }
        }

        // Calculate the dominant frequency from the index of the detected peak
        float dominantFrequency = maxIndex * AudioSettings.outputSampleRate / 2 / spectrumSize;
        return dominantFrequency;
    }

    float[] SmoothSpectrum(float[] spectrumData)
    {
        // Use a simple moving average to smooth the spectrum data
        int smoothWindowSize = 3;  // Adjust the window size to control the level of smoothing
        float[] smoothedSpectrum = new float[spectrumData.Length];

        for (int i = 0; i < spectrumData.Length; i++)
        {
            float sum = 0f;
            int count = 0;

            // Calculate the average within the smoothing window
            for (int j = -smoothWindowSize; j <= smoothWindowSize; j++)
            {
                int index = i + j;
                if (index >= 0 && index < spectrumData.Length)
                {
                    sum += spectrumData[index];
                    count++;
                }
            }

            smoothedSpectrum[i] = sum / count;
        }

        return smoothedSpectrum;
    }




    // Convert detected frequency to MIDI note number
    int FrequencyToMidi(float frequency)
    {
        Debug.Log("FrequencyToMidi()");
        if (frequency <= 0) return -1;
        float midiNoteFloat = 12 * Mathf.Log(frequency / referenceFrequency, 2) + 69;
        return Mathf.RoundToInt(midiNoteFloat);
    }

    // Convert MIDI note to note name and octave
    string MidiToNoteName(int midiNote)
    {
        if (midiNote < 0)
        {
            return "Unknown";  // Return "Unknown" if midiNote is invalid
        }

        int noteIndex = (midiNote - 12) % 12;

        // Ensure the noteIndex is positive (handle negative modulus results)
        if (noteIndex < 0)
        {
            noteIndex += 12;
        }

        int octave = (midiNote / 12) - 1;  // Octave calculation

        // Ensure the noteIndex is valid before accessing the noteNames array
        return noteNames[noteIndex] + octave;
    }


    // Apply temporal smoothing by averaging recent MIDI notes
    int ApplyTemporalSmoothing(int midiNote)
    {
        Debug.Log("ApplyTemporalSmoothing()");
        midiHistory.Add(midiNote);

        if (midiHistory.Count > midiSmoothingWindow)
        {
            midiHistory.RemoveAt(0);
        }

        return Mathf.RoundToInt((float)midiHistory.Average());
    }

    float[] NormalizeAudioLevel(float[] data, float targetLevel = 1.0f)
    {
        Debug.Log("NormalizeAudioLevel()");
        float maxAmplitude = data.Max();
        if (maxAmplitude > 0)
        {
            float normalizationFactor = targetLevel / maxAmplitude;
            for (int i = 0; i < data.Length; i++)
            {
                data[i] *= normalizationFactor;  // Scale the audio data
            }
        }
        return data;
    }



    // Get the current audio level (volume)
    float GetAudioLevel()
    {
        float[] data = new float[sampleWindow];
        int micPosition = Microphone.GetPosition(selectedMic) - sampleWindow + 1;
        if (micPosition < 0) return 0;

        currentClip.GetData(data, micPosition);

        // Normalize the data to boost quiet audio
        data = NormalizeAudioLevel(data, targetLevel: 1.0f);

        // Calculate the average level of the audio samples
        float sum = 0;
        for (int i = 0; i < sampleWindow; i++)
        {
            sum += Mathf.Abs(data[i]);
        }

        return sum / sampleWindow;
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
