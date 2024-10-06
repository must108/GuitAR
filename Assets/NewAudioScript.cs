using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Debug = UnityEngine.Debug;
using System.Linq; // Required for using LINQ (for averaging the list)
//using System.Diagnostics;

public class TheyPlayed
{
    protected string isBeingPlayed;  // The musical note being played (e.g., "C#4")
    protected int?[,] frets;       // Array to represent 4 frets and 6 strings. Each index is a string, each row is a fret.

    // Constructor to initialize the frets array
    public TheyPlayed()
    {
        frets = new int?[4, 6]; // Initialize the array to hold 4 frets x 6 strings
        ResetFrets();
    }

    public string GetIsBeingPlayed()
    {
        return isBeingPlayed;
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

        return $"TheyPlayed [isBeingPlayed='{isBeingPlayed}', frets=\n{fretsString}]";
    }

    public TheyPlayed GetTheyPlayed()
    {
        // Create a new TheyPlayed object
        TheyPlayed copy = new TheyPlayed();

        // Copy the isBeingPlayed value
        copy.setIsBeingPlayed(this.isBeingPlayed);

        // Copy the frets array
        for (int fret = 0; fret < 4; fret++)
        {
            for (int stringIndex = 0; stringIndex < 6; stringIndex++)
            {
                copy.frets[fret, stringIndex] = this.frets[fret, stringIndex];
            }
        }

        // Return the copy
        return copy;
    }

    public void setIsBeingPlayed(string note)
    {
        isBeingPlayed = note;
    }

    public void MapNoteNameToFretArray(string noteName)
    {
        int fretCount = 4; // Frets 1 to 4

        // Reset the frets array
        ResetFrets();

        // Mapping note names to their positions on the fretboard
        // Each note maps to a list of positions (stringIndex, fret)
        Dictionary<string, List<(int stringIndex, int fret)>> notePositions = new Dictionary<string, List<(int, int)>>()
    {
        // String 6 (Low E string), index 0
        { "E2", new List<(int, int)>{ (0, -1) } },
        { "F2", new List<(int, int)>{ (0, 0) } },
        { "F#2", new List<(int, int)>{ (0, 1) } },
        { "G2", new List<(int, int)>{ (0, 2) } },
        { "G#2", new List<(int, int)>{ (0, 3) } },

        // String 5 (A string), index 1
        { "A2", new List<(int, int)>{ (1, -1) } },
        { "A#2", new List<(int, int)>{ (1, 0) } },
        { "B2", new List<(int, int)>{ (1, 1) } },
        { "C3", new List<(int, int)>{ (1, 2) } },
        { "C#3", new List<(int, int)>{ (1, 3) } },

        // String 4 (D string), index 2
        { "D3", new List<(int, int)>{ (2, -1) } },
        { "D#3", new List<(int, int)>{ (2, 0) } },
        { "E3", new List<(int, int)>{ (2, 1) } },
        { "F3", new List<(int, int)>{ (2, 2) } },
        { "F#3", new List<(int, int)>{ (2, 3) } },

        // String 3 (G string), index 3
        { "G3", new List<(int, int)>{ (3, -1) } },
        { "G#3", new List<(int, int)>{ (3, 0) } },
        { "A3", new List<(int, int)>{ (3, 1) } },
        { "A#3", new List<(int, int)>{ (3, 2) } },
        { "B3", new List<(int, int)>{ (4, -1) } }, // B3 on String 3 and String 2

        // String 2 (B string), index 4
        { "C4", new List<(int, int)>{ (4, 0) } },
        { "C#4", new List<(int, int)>{ (4, 1) } },
        { "D4", new List<(int, int)>{ (4, 2) } },
        { "D#4", new List<(int, int)>{ (4, 3) } },

        // String 1 (High E string), index 5
        { "E4", new List<(int, int)>{ (5, -1) } },
        { "F4", new List<(int, int)>{ (5, 0) } },
        { "F#4", new List<(int, int)>{ (5, 1) } },
        { "G4", new List<(int, int)>{ (5, 2) } },
        { "G#4", new List<(int, int)>{ (5, 3) } },
    };

        // Check if the note exists in the dictionary
        if (notePositions.TryGetValue(noteName, out List<(int stringIndex, int fret)> positions))
        {
            // Select the position with the lowest fret number
            var selectedPosition = positions.OrderBy(pos => pos.fret == -1 ? 0 : pos.fret).First();

            int stringIndex = selectedPosition.stringIndex;
            int fret = selectedPosition.fret;

            if (fret == -1)
            {
                // Open string: populate the entire column with 0s
                for (int f = 0; f < fretCount; f++)
                {
                    UpdateFret(f, stringIndex, 0);
                }
            }
            else
            {
                // String is played: populate with 0s, set the fret to 1
                for (int f = 0; f < fretCount; f++)
                {
                    UpdateFret(f, stringIndex, 0);
                }
                UpdateFret(fret, stringIndex, 1); // Finger on the specific fret
            }

            // Set other strings to null (not played)
            for (int s = 0; s < 6; s++)
            {
                if (s != stringIndex)
                {
                    for (int f = 0; f < fretCount; f++)
                    {
                        UpdateFret(f, s, null);
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning($"Note {noteName} not found within the first four frets.");
            ResetFrets(); // Ensure frets are reset if note is not found
        }
    }
}



public class NewAudioScript : MonoBehaviour
{
    private AudioClip currentClip;
    private AudioSource audioInterface;
    private string selectedMic;

    public float threshold = 0.17f; // Adjust as needed
    public int bufferSize = 4096;
    private float[] audioBuffer; // Time domain buffer, freqs over time
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

    public TheyPlayed theyPlayed = new TheyPlayed(); // Instance of the TheyPlayed class

    // Variables for temporal smoothing and peak detection
    private Queue<string> recentNotes = new Queue<string>();
    public int noteBufferSize = 5;
    void Start()
    {
        audioInterface = GetComponent<AudioSource>();

        string primaryMicName = "Analogue 1 + 2 (Focusrite USB Audio)";
        string secondaryMicName = "Default Microphone";

        string[] availableMics = Microphone.devices;

        if (availableMics.Length == 0)
        {
            Debug.LogError("No microphone detected.");
            return;
        }

        if (availableMics.Contains(primaryMicName))
        {
            selectedMic = primaryMicName;
            Debug.Log($"Selected primary microphone: {selectedMic}");
        }
        else if (availableMics.Contains(secondaryMicName))
        {
            selectedMic = secondaryMicName;
            Debug.Log($"Selected secondary microphone: {selectedMic}");
        }
        else
        {
            selectedMic = availableMics[0];
            Debug.Log($"Selected default microphone: {selectedMic}");
        }

        currentClip = Microphone.Start(selectedMic, true, 1, 44100);
        audioInterface.clip = currentClip;
        audioInterface.loop = true;
        audioInterface.Play();

        audioBuffer = new float[bufferSize];

    }

    void Update()
    {
        //Stopwatch stopwatch = new Stopwatch();
        //stopwatch.Start();
        if (Microphone.IsRecording(selectedMic))
        {
            int micPosition = Microphone.GetPosition(selectedMic) - bufferSize;
            if (micPosition < 0) return;

            currentClip.GetData(audioBuffer, micPosition);

            ApplyHanningWindow(audioBuffer);

            // new peak detection!!
            float fundamentalFrequency = DetectFundamentalFrequencyYIN(audioBuffer);

            string noteName = "Unknown";

            if (fundamentalFrequency > 0)
            {
                noteName = MapFrequencyToNoteName(fundamentalFrequency);
            }
            else
            {
                Debug.LogWarning("No valid frequency detected.");
            }


            // Temporal smoothing time!
            recentNotes.Enqueue(noteName);
            if (recentNotes.Count > noteBufferSize)
            {
                recentNotes.Dequeue();
            }

            string mostCommonNote = recentNotes
                .GroupBy(n => n)
                .OrderByDescending(g => g.Count())
                .First()
                .Key;


            // Update TheyPlayed object
            theyPlayed.setIsBeingPlayed(mostCommonNote);
            theyPlayed.MapNoteNameToFretArray(mostCommonNote);
            // TheyPlayed testy = theyPlayed.GetTheyPlayed();


            Debug.Log($"Final Detected note: {mostCommonNote}");
            Debug.Log(theyPlayed.ToString());
            // Debug.Log(testy.ToString());
            //stopwatch.Stop();
            //long timeTaken = stopwatch.ElapsedMilliseconds;
            //Debug.Log($"Audio Script execution time: {timeTaken} ms");
        }
    }

    void ApplyHanningWindow(float[] buffer)
    {
        int length = buffer.Length;
        for (int i = 0; i < length; i++)
        {
            buffer[i] *= 0.5f * (1 - Mathf.Cos(2 * Mathf.PI * i / (length - 1)));
        }
    }

    float DetectFundamentalFrequencyYIN(float[] buffer)
    {
        int sampleRate = 44100;
        int tauMax = buffer.Length / 2;
        float[] differenceFunction = new float[tauMax];
        float[] cumulativeMeanNormalizedDifference = new float[tauMax];
        int tauEstimate = -1;

        // Step 1: Difference function
        for (int tau = 1; tau < tauMax; tau++)
        {
            float sum = 0f;
            for (int j = 0; j < buffer.Length - tau; j++)
            {
                float diff = buffer[j] - buffer[j + tau];
                sum += diff * diff;
            }
            differenceFunction[tau] = sum;
        }

        // Step 2: Cumulative mean normalized difference function
        cumulativeMeanNormalizedDifference[0] = 1;
        float runningSum = 0f;
        for (int tau = 1; tau < tauMax; tau++)
        {
            runningSum += differenceFunction[tau];
            cumulativeMeanNormalizedDifference[tau] = differenceFunction[tau] / (runningSum / tau);
        }

        // Step 3: Absolute threshold
        for (int tau = 2; tau < tauMax; tau++)
        {
            if (cumulativeMeanNormalizedDifference[tau] < threshold)
            {
                while (tau + 1 < tauMax && cumulativeMeanNormalizedDifference[tau + 1] < cumulativeMeanNormalizedDifference[tau])
                {
                    tau++;
                }
                tauEstimate = tau;
                break;
            }
        }

        // Step 4: Parabolic interpolation
        if (tauEstimate != -1)
        {
            float betterTau = tauEstimate;
            if (tauEstimate > 0 && tauEstimate < cumulativeMeanNormalizedDifference.Length - 1)
            {
                float s0 = cumulativeMeanNormalizedDifference[tauEstimate - 1];
                float s1 = cumulativeMeanNormalizedDifference[tauEstimate];
                float s2 = cumulativeMeanNormalizedDifference[tauEstimate + 1];
                betterTau = tauEstimate + (s2 - s0) / (2 * (2 * s1 - s2 - s0));
            }
            else
            {
                betterTau = tauEstimate;
            }
            float fundamentalFrequency = sampleRate / betterTau;
            return fundamentalFrequency;
        }
        else
        {
            return -1f; // No pitch found
        }
    }

    string MapFrequencyToNoteName(float frequency)
    {
        if (frequency <= 0) return "Unknown";

        float minDifference = float.MaxValue;
        int closestIndex = -1;

        for (int i = 0; i < guitarFrequencies.Length; i++)
        {
            float diff = Mathf.Abs(frequency - guitarFrequencies[i]);
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
