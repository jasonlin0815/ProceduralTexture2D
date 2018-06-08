using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise
{
    public float[] GenerateNoiseMap(int width, int height, float scale, int octaves, float persistance, float lacunarity, float movement, float animateDelta)
    {
        // Allocate space to generate noise map containing the terrain height at x coordinate
        float[] noiseMap = new float[width];

        // Adjust scale if input is invalid
        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        // Loop through every pixel of width
        for (int w = 0; w < width; ++w)
        {
            // Amplitude, frequency and noise height are used to add noises layers
            float amplitude = 1;
            float frequency = 1;
            float noiseHeight = 0;

            // Each octaves will add an extra layer to the overall noise, with different amplitude and frequency sampling
            for (int i = 0; i < octaves; ++i)
            {
                float sampleWidth = (w + movement) / scale * frequency;
                float noiseValue = Mathf.PerlinNoise(sampleWidth, animateDelta);

                noiseHeight += noiseValue * amplitude;
                amplitude *= persistance;
                frequency *= lacunarity;
            }

            // Normalize the noiseHeight back to [0, 1] scale
            noiseMap[w] = noiseHeight / octaves * height;
        }

        return noiseMap;
    }
}
