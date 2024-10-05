using UnityEngine;
using System.Collections.Generic;

public class NewAudioScript : MonoBehaviour
{
    // Data structure
    public class TheyPlayed
    {
        public bool isNote;
        public bool isChord;
        public bool isNewNote;
        public int?[,] frets; // [4 frets (0 to 3), 6 strings]
        public string isBeingPlayed;

        public override string ToString()
        {
            // Build a string representation of the frets array
            string fretsString = "";
            for (int fret = 0; fret < 4; fret++)
            {
                fretsString += $"Fret {fret + 1}: ";
                for (int stringIndex = 0; stringIndex < 6; stringIndex++)
                {
                    fretsString += frets[fret, stringIndex]?.ToString() ?? "null";
                    if (stringIndex < 5) fretsString += ", ";
                }
                fretsString += "\n";
            }
            return $"TheyPlayed [isNote={isNote}, isChord={isChord}, isNewNote={isNewNote}, frets=\n{fretsString}, isBeingPlayed='{isBeingPlayed}']";
        }
    }

    // Audio variables
    private AudioSource audioSource;

    private string selectedMic;
    public int sampleRate = 44100;
    public int bufferSize = 1024;

    // Buffers
    private float[] audioBuffer;
    private Queue<float[]> lastThreeBuffers = new Queue<float[]>();
    private List<float> averageEnergyList = new List<float>();

    // Thresholds
    public float volumeThreshold = 0.01f;
    private float dynamicThreshold = 0.0f;
    public float tolerance = 0.12f; // 12 cents tolerance

    // Chord maps
    public Dictionary<string, float[]> chordMaps = new Dictionary<string, float[]>();

    // Note mappings
    private int[] openStringMidiNotes = new int[6] { 40, 45, 50, 55, 59, 64 }; // E2, A2, D3, G3, B3, E4
    private Dictionary<int, List<(int stringIndex, int fret)>> midiNoteToStringFret = new Dictionary<int, List<(int, int)>>();

    private bool isNewNoteDetected = false;
    private float lastBufferEnergy = 0f;

    void Start()
    {
        // Initialize variables
        audioBuffer = new float[bufferSize];

        // Initialize chord maps
        InitializeChordMaps();

        // Initialize note mappings
        InitializeNoteMappings();

        // Get AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Start microphone
        StartMicrophone();
    }

    void Update()
    {
        // Process audio
        if (Microphone.IsRecording(selectedMic))
        {
            ProcessAudio();
        }
    }

    void InitializeChordMaps()
    {
        // Initialize chord maps with expected chroma vectors
        chordMaps["C Major"] = new float[12];
        chordMaps["C Major"][0] = 1f; // C
        chordMaps["C Major"][4] = 1f; // E
        chordMaps["C Major"][7] = 1f; // G

        chordMaps["G Major"] = new float[12];
        chordMaps["G Major"][7] = 1f;  // G
        chordMaps["G Major"][11] = 1f; // B
        chordMaps["G Major"][2] = 1f;  // D

        // Add more chords as needed, covering all possible chords within the first four frets
    }

    void InitializeNoteMappings()
    {
        // Initialize midiNoteToStringFret dictionary
        // Map MIDI notes to possible string and fret combinations within the first four frets
        for (int stringIndex = 0; stringIndex < 6; stringIndex++)
        {
            for (int fret = 0; fret <= 4; fret++)
            {
                int midiNote = openStringMidiNotes[stringIndex] + fret;
                if (!midiNoteToStringFret.ContainsKey(midiNote))
                {
                    midiNoteToStringFret[midiNote] = new List<(int, int)>();
                }
                midiNoteToStringFret[midiNote].Add((stringIndex, fret));
            }
        }
    }

    void StartMicrophone()
    {
        Debug.Log("Available Microphones:");
        foreach (string device in Microphone.devices)
        {
            Debug.Log(device);
        }
        // Quest: "Android audio input"
        selectedMic = "Analogue 1 + 2 (Focusrite USB Audio)"; // Replace with your microphone name or use the first available

        // Ensure 'selectedMic' is valid
        if (string.IsNullOrEmpty(selectedMic))
        {
            if (Microphone.devices.Length > 0)
            {
                selectedMic = Microphone.devices[0];
                Debug.Log("No microphone specified. Using default: " + selectedMic);
            }
            else
            {
                Debug.LogError("No microphone devices found.");
                return;
            }
        }

        // Start the microphone and assign it to an AudioClip
        AudioClip micClip = Microphone.Start(selectedMic, true, 1, sampleRate);
        audioSource.clip = micClip;
        audioSource.loop = true;

        // Wait until the microphone starts recording
        while (!(Microphone.GetPosition(selectedMic) > 0)) { }

        audioSource.Play();
    }

    void ProcessAudio()
    {
        Debug.Log("In ProcessAudio()");
        // Get spectrum data
        float[] spectrum = new float[bufferSize];
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        // Calculate energy (volume)
        float bufferEnergy = CalculateEnergy(spectrum);
        Debug.Log($"Buffer energy: {bufferEnergy}");

        // Onset detection for new note
        isNewNoteDetected = OnsetDetection(bufferEnergy);


        Debug.Log("Buffer energy above threshold. Processing sample.");

        // Save the energy and recalculate dynamic threshold
        averageEnergyList.Add(bufferEnergy);
        if (averageEnergyList.Count > 10)
            averageEnergyList.RemoveAt(0);

        float averageEnergy = CalculateAverageEnergy(averageEnergyList);
        dynamicThreshold = volumeThreshold * bufferEnergy / averageEnergy;

        // Detect pitches
        List<float> detectedFrequencies = DetectPitches(spectrum);
        List<int> detectedMidiNotes = new List<int>();
        foreach (float freq in detectedFrequencies)
        {
            // Calculate MIDI note number
            float midiNoteFloat = 69 + 12 * Mathf.Log(freq / 440f, 2f);
            int midiNote = Mathf.RoundToInt(midiNoteFloat);
            detectedMidiNotes.Add(midiNote);
        }

        // Determine if it's a note or a chord
        bool isChord = detectedMidiNotes.Count > 1;
        bool isNote = detectedMidiNotes.Count == 1;

        // Create TheyPlayed object
        TheyPlayed theyPlayed = new TheyPlayed();
        theyPlayed.isNote = isNote;
        theyPlayed.isChord = isChord;
        theyPlayed.isNewNote = isNewNoteDetected;
        theyPlayed.frets = new int?[4, 6]; // frets 0 to 3 (first 4 frets), strings 0 to 5

        // Initialize frets array to null
        for (int fret = 0; fret < 4; fret++)
        {
            for (int stringIndex = 0; stringIndex < 6; stringIndex++)
            {
                theyPlayed.frets[fret, stringIndex] = null;
            }
        }

        // Keep track of which strings are being played
        bool[] stringsPlayed = new bool[6];

        foreach (int midiNote in detectedMidiNotes)
        {
            if (midiNoteToStringFret.ContainsKey(midiNote))
            {
                List<(int stringIndex, int fret)> possiblePositions = midiNoteToStringFret[midiNote];
                // Select the position with the lowest fret number
                (int stringIndex, int fret) selectedPosition = (-1, int.MaxValue);
                foreach (var pos in possiblePositions)
                {
                    if (pos.fret <= 4 && pos.fret < selectedPosition.fret && !stringsPlayed[pos.stringIndex])
                    {
                        selectedPosition = pos;
                    }
                }
                if (selectedPosition.stringIndex != -1)
                {
                    stringsPlayed[selectedPosition.stringIndex] = true;
                    int fretIndex = selectedPosition.fret; // fret 0 to 4
                    if (selectedPosition.fret == 0)
                    {
                        // Open string
                        theyPlayed.frets[0, selectedPosition.stringIndex] = 0;
                    }
                    else
                    {
                        // Finger on fret
                        theyPlayed.frets[selectedPosition.fret - 1, selectedPosition.stringIndex] = 1;
                    }
                }
            }
        }

        // For strings that are being played but no finger is on any fret (open strings not assigned to any detected note), set frets[0, stringIndex] = 0
        for (int stringIndex = 0; stringIndex < 6; stringIndex++)
        {
            if (stringsPlayed[stringIndex])
            {
                bool hasFingerOnFret = false;
                for (int fretIndex = 0; fretIndex < 4; fretIndex++)
                {
                    if (theyPlayed.frets[fretIndex, stringIndex] == 1)
                    {
                        hasFingerOnFret = true;
                        break;
                    }
                }
                if (!hasFingerOnFret)
                {
                    // String is being played open
                    theyPlayed.frets[0, stringIndex] = 0;
                }
            }
            else
            {
                // String is not being played
                for (int fretIndex = 0; fretIndex < 4; fretIndex++)
                {
                    theyPlayed.frets[fretIndex, stringIndex] = null;
                }
            }
        }

        // Set isBeingPlayed
        if (isNote)
        {
            int midiNote = detectedMidiNotes[0];
            string noteName = MidiNoteToName(midiNote);
            theyPlayed.isBeingPlayed = noteName;
        }
        else if (isChord)
        {
            // Create chromagram
            float[] chroma = CreateChromagram(spectrum);
            string matchedChord = MatchChord(chroma);
            theyPlayed.isBeingPlayed = matchedChord ?? "Unknown Chord";
        }

        // Output theyPlayed to game logic
        ProcessTheyPlayed(theyPlayed);

    }

    float CalculateEnergy(float[] buffer)
    {
        Debug.Log("In CalculateEnergy()");
        float energy = 0f;
        foreach (var sample in buffer)
        {
            energy += sample * sample;
        }
        return energy / buffer.Length;
    }

    float CalculateAverageEnergy(List<float> energies)
    {
        Debug.Log("In CalculateAverageEnergy()");
        float sum = 0f;
        foreach (var energy in energies)
        {
            sum += energy;
        }
        return sum / energies.Count;
    }

    bool OnsetDetection(float bufferEnergy)
    {
        Debug.Log("In OnsetDetection()");
        // Simple onset detection based on energy change
        bool isNewNote = false;
        if (averageEnergyList.Count > 0)
        {
            float energyChange = bufferEnergy - lastBufferEnergy;
            if (energyChange > dynamicThreshold)
            {
                isNewNote = true;
            }
        }
        lastBufferEnergy = bufferEnergy;
        return isNewNote;
    }

    List<float> DetectPitches(float[] spectrum)
    {
        Debug.Log("In DetectPitches()");
        List<float> peakFrequencies = new List<float>();

        // Simple peak detection
        float threshold = 0.01f; // Adjust as needed

        for (int i = 1; i < spectrum.Length - 1; i++)
        {
            if (spectrum[i] > threshold && spectrum[i] > spectrum[i - 1] && spectrum[i] > spectrum[i + 1])
            {
                // Convert bin index to frequency
                float freq = i * sampleRate / 2f / spectrum.Length;
                peakFrequencies.Add(freq);
            }
        }

        // Sort peaks by amplitude descending
        List<(float frequency, float amplitude)> peaks = new List<(float, float)>();
        foreach (float freq in peakFrequencies)
        {
            int bin = (int)(freq * spectrum.Length * 2 / sampleRate);
            float amplitude = spectrum[bin];
            peaks.Add((freq, amplitude));
        }

        peaks.Sort((a, b) => b.amplitude.CompareTo(a.amplitude));

        // Limit to top 6 peaks (max number of strings)
        int N = 6;
        List<float> detectedFrequencies = new List<float>();
        for (int i = 0; i < Mathf.Min(N, peaks.Count); i++)
        {
            detectedFrequencies.Add(peaks[i].frequency);
        }

        return detectedFrequencies;
    }

    string MidiNoteToName(int midiNote)
    {
        string[] noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
        int noteIndex = midiNote % 12;
        int octave = (midiNote / 12) - 1;
        return noteNames[noteIndex] + octave;
    }

    float[] CreateChromagram(float[] spectrum)
    {
        Debug.Log("In CreateChromagram()");
        float[] chroma = new float[12];
        for (int i = 0; i < spectrum.Length; i++)
        {
            float freq = (float)i * sampleRate / 2f / spectrum.Length;
            if (freq < 20f || freq > 5000f)
                continue;

            // Calculate pitch class
            float noteNumber = 12f * Mathf.Log(freq / 440f, 2f) + 69f;
            int midiNote = Mathf.RoundToInt(noteNumber);
            int pitchClass = midiNote % 12;

            chroma[pitchClass] += spectrum[i];
        }
        return chroma;
    }

    string MatchChord(float[] chroma)
    {
        Debug.Log("In MatchChord()");
        // Normalize chroma vector
        float chromaSum = 0f;
        foreach (float value in chroma)
            chromaSum += value;

        if (chromaSum == 0f)
            return null;

        for (int i = 0; i < chroma.Length; i++)
            chroma[i] /= chromaSum;

        // Compare chroma to chord maps
        string bestMatch = null;
        float bestCorrelation = 0f;

        foreach (var chordMap in chordMaps)
        {
            float correlation = 0f;
            for (int i = 0; i < chroma.Length; i++)
            {
                correlation += chroma[i] * chordMap.Value[i];
            }

            if (correlation > bestCorrelation)
            {
                bestCorrelation = correlation;
                bestMatch = chordMap.Key;
            }
        }

        if (bestCorrelation > tolerance)
            return bestMatch;

        return null;
    }

    float[] TemporalSmoothing(Queue<float[]> buffers)
    {
        Debug.Log("In TemporalSmoothing()");
        // Average the spectra
        float[] smoothedSpectrum = new float[bufferSize];
        foreach (var buffer in buffers)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                smoothedSpectrum[i] += buffer[i];
            }
        }
        for (int i = 0; i < smoothedSpectrum.Length; i++)
        {
            smoothedSpectrum[i] /= buffers.Count;
        }
        return smoothedSpectrum;
    }

    void ProcessTheyPlayed(TheyPlayed theyPlayed)
    {
        // Implement game logic processing here
        Debug.Log(theyPlayed);
    }
}
