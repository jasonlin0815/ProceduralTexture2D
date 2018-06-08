using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioAnalyzer : MonoBehaviour
{
    [SerializeField]
    private int samplingSize = 512;

    private AudioSource audioSource;
    private float[] audioSamples;

    private float[] frequencyBands = new float[8];
    private float[] bandBuffer = new float[8];
    private float[] bufferDecrease = new float[8];

    float[] freqBandHighest = new float[8];

    [HideInInspector]
    public float[] audioBand = new float[8];

    [HideInInspector]
    public float[] audioBandBuffer = new float[8];

    private static AudioAnalyzer audioAnalyzer;
    public static AudioAnalyzer instance
    {
        get
        {
            if (!audioAnalyzer)
            {
                audioAnalyzer = FindObjectOfType<AudioAnalyzer>();
                if (!audioAnalyzer)
                {
                    Debug.LogError("AudioAnalyzer must be attached to an active gameobject in game");
                }
            }
            return audioAnalyzer;
        }
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSamples = new float[samplingSize];
    }

    private void Update()
    {
        GetSpectrumAudioSource();
        CreateFrequencyBands();
        CreateBandBuffer();
        CreateAudioBands();
    }

    private void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(audioSamples, 0, FFTWindow.Blackman);
    }

    private void CreateFrequencyBands()
    {
        int count = 0;

        for (int i = 0; i < 8; ++i)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if (i == 7)
            {
                sampleCount += 2;
            }

            for (int j = 0; j < sampleCount; ++j)
            {
                average += audioSamples[count] * (count + 1);
                ++count;
            }

            average /= count;
            frequencyBands[i] = average * 10;
        }
    }

    private void CreateBandBuffer()
    {
        for (int g = 0; g < 8; ++g)
        {
            if (frequencyBands[g] > bandBuffer[g])
            {
                bandBuffer[g] = frequencyBands[g];
                bufferDecrease[g] = 0.005f;
            }

            if (frequencyBands[g] < bandBuffer[g])
            {
                bandBuffer[g] -= bufferDecrease[g];
                bufferDecrease[g] *= 1.2f;
            }
        }
    }

    private void CreateAudioBands()
    {
        for (int i = 0; i < 8; ++i)
        {
            if (frequencyBands[i] > freqBandHighest[i])
            {
                freqBandHighest[i] = frequencyBands[i];
            }
            audioBand[i] = frequencyBands[i] / freqBandHighest[i];
            audioBandBuffer[i] = bandBuffer[i] / freqBandHighest[i];
        }
    }
}
