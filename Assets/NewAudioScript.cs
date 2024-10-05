using UnityEngine;
using System.Collections.Generic;

public class AudioProcessor : MonoBehaviour
{
    // Data structure
    public class TheyPlayed
    {
        public bool isNote;
        public bool isChord;
        public bool isNewNote;
        public int?[] frets; // array of nullable ints
        public string isBeingPlayed;
    }

    // Audio variables
    private AudioSource audioSource;
    private int sampleRate = 44100;
    private int bufferSize = 1024;

    // Buffers
    private float[] audioBuffer;
    private Queue<float[]> lastThreeBuffers = new Queue<float[]>();
    private List<float> averageEnergyList = new List<float>();

    // Thresholds
    private float volumeThreshold = 0.01f;
    private float dynamicThreshold = 0.0f;
    private float tolerance = 0.12f; // 12 cents tolerance

    // Chord maps
    private Dictionary<string, float[]> chordMaps = new Dictionary<string, float[]>();

    void Start()
    {
        // Initialize variables
        audioBuffer = new float[bufferSize];

        // Initialize chord maps
        InitializeChordMaps();

        // Create AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();

        // Start microphone
        StartMicrophone();
    }

    void Update()
    {
        // Process audio
        ProcessAudio();
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
        // Add more chords as needed
    }

    void StartMicrophone()
    {
        audioSource.clip = Microphone.Start(null, true, 1, sampleRate);
        audioSource.loop = true;
        while (!(Microphone.GetPosition(null) > 0)) { } // Wait until the recording has started
        audioSource.Play();
    }

    void ProcessAudio()
    {
        // Get spectrum data
        float[] spectrum = new float[bufferSize];
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        // Calculate energy (volume)
        float bufferEnergy = CalculateEnergy(spectrum);

        if (bufferEnergy < volumeThreshold)
        {
            // Discard the sample
            return;
        }
        else
        {
            // Save the energy and recalculate dynamic threshold
            averageEnergyList.Add(bufferEnergy);
            if (averageEnergyList.Count > 10)
                averageEnergyList.RemoveAt(0);

            float averageEnergy = CalculateAverageEnergy(averageEnergyList);
            dynamicThreshold = volumeThreshold * bufferEnergy / averageEnergy;

            // Check for new note attack using onset detection
            bool isNewNote = OnsetDetection(spectrum);

            // Pitch detection
            float pitch;
            float confidence;
            PitchDetection(spectrum, out pitch, out confidence);

            if (confidence > 0.9f) // Arbitrary confidence threshold
            {
                // High confidence, return result to game logic
                TheyPlayed theyPlayed = new TheyPlayed();
                theyPlayed.isNote = true;
                theyPlayed.isChord = false;
                theyPlayed.isNewNote = isNewNote;
                theyPlayed.frets = new int?[6]; // Populate as needed
                theyPlayed.isBeingPlayed = "Note: " + pitch.ToString("F2") + " Hz";

                // Output theyPlayed to game logic
                ProcessTheyPlayed(theyPlayed);
            }
            else
            {
                // Assume chord
                float[] chroma = CreateChromagram(spectrum);
                string matchedChord = MatchChord(chroma);

                if (!string.IsNullOrEmpty(matchedChord))
                {
                    TheyPlayed theyPlayed = new TheyPlayed();
                    theyPlayed.isNote = false;
                    theyPlayed.isChord = true;
                    theyPlayed.isNewNote = isNewNote;
                    theyPlayed.frets = new int?[6]; // Populate as needed
                    theyPlayed.isBeingPlayed = "Chord: " + matchedChord;

                    // Output theyPlayed to game logic
                    ProcessTheyPlayed(theyPlayed);
                }
                else
                {
                    // Temporal smoothing with last 3 buffers
                    lastThreeBuffers.Enqueue((float[])spectrum.Clone());
                    if (lastThreeBuffers.Count > 3)
                        lastThreeBuffers.Dequeue();

                    float[] smoothedSpectrum = TemporalSmoothing(lastThreeBuffers);

                    float[] smoothedChroma = CreateChromagram(smoothedSpectrum);
                    string smoothedMatchedChord = MatchChord(smoothedChroma);

                    if (!string.IsNullOrEmpty(smoothedMatchedChord))
                    {
                        TheyPlayed theyPlayed = new TheyPlayed();
                        theyPlayed.isNote = false;
                        theyPlayed.isChord = true;
                        theyPlayed.isNewNote = isNewNote;
                        theyPlayed.frets = new int?[6]; // Populate as needed
                        theyPlayed.isBeingPlayed = "Chord: " + smoothedMatchedChord;

                        // Output theyPlayed to game logic
                        ProcessTheyPlayed(theyPlayed);
                    }
                    else
                    {
                        // Further FFT refinement if needed
                        // Implement additional processing here
                    }
                }
            }
        }
    }

    float CalculateEnergy(float[] buffer)
    {
        float energy = 0f;
        foreach (var sample in buffer)
        {
            energy += sample * sample;
        }
        return energy / buffer.Length;
    }

    float CalculateAverageEnergy(List<float> energies)
    {
        float sum = 0f;
        foreach (var energy in energies)
        {
            sum += energy;
        }
        return sum / energies.Count;
    }

    bool OnsetDetection(float[] spectrum)
    {
        // Simple onset detection based on energy change
        if (averageEnergyList.Count < 2)
            return false;

        float currentEnergy = averageEnergyList[averageEnergyList.Count - 1];
        float previousEnergy = averageEnergyList[averageEnergyList.Count - 2];

        return (currentEnergy - previousEnergy) > dynamicThreshold;
    }

    void PitchDetection(float[] spectrum, out float pitch, out float confidence)
    {
        int maxIndex = 0;
        float maxValue = 0f;

        for (int i = 0; i < spectrum.Length; i++)
        {
            if (spectrum[i] > maxValue)
            {
                maxValue = spectrum[i];
                maxIndex = i;
            }
        }

        // Convert index to frequency
        float freqN = (float)maxIndex * sampleRate / 2f / spectrum.Length;
        pitch = freqN;
        confidence = maxValue; // Not a true confidence measure, refine as needed
    }

    float[] CreateChromagram(float[] spectrum)
    {
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
        Debug.Log(theyPlayed.isBeingPlayed);
    }
}
