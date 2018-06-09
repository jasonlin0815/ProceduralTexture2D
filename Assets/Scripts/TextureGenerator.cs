using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class TextureGenerator : MonoBehaviour
{
    [SerializeField]
    private int width;

    [SerializeField]
    private int height;

    [SerializeField]
    private int terrainHeightCap;

    [SerializeField]
    private Color terrainMainColor;

    [SerializeField]
    private bool showTerrainOutline = false;

    [SerializeField]
    private Color terrainOutlineColor;

    [Range(0, 5)]
    [SerializeField]
    private int terrainOutlineSize = 1;

    [Range(0, 1000)]
    [SerializeField]
    private float moveSpeed;

    [Range(0, 10)]
    [SerializeField]
    private float animateSpeed;

    [Range(1, 5)]
    [SerializeField]
    private int octaves = 3;

    [Range(0, 1)]
    [SerializeField]
    private float persistance;

    [Range(1, 2)]
    [SerializeField]
    private float lacunarity;

    [Range(1, 1000)]
    [SerializeField]
    private float scale;

    [SerializeField]
    private bool listenToAudio = false;

    [Range(0, 7)]
    [SerializeField]
    private int audioChannel;

    [Range(0, 1)]
    [SerializeField]
    private float audioThreshold;

    [Range(1, 2)]
    [SerializeField]
    private float musicResponseScale;

    [Range(0, 1)]
    [SerializeField]
    private float musicDecayRate;

    private Texture2D texture;
    private SpriteRenderer spriteRenderer;
    private float movement = 0;
    private float timeLapse = 0;
    private float currentResponseScale = 1;
    private Noise noise;
    private Material mat;

    private void Awake()
    {
        mat = ((Renderer)GetComponent(typeof(Renderer))).material;
        // Create a Texture2D object with given width and height
        texture = new Texture2D(width, height);
        noise = new Noise();

        // Apply the texture to the sprite renderer by converting the Texture2D to Sprite object
        spriteRenderer = GetComponent<SpriteRenderer>();
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        spriteRenderer.sprite = sprite;
    }

    private void Update()
    {
        movement += Time.deltaTime * moveSpeed;
        timeLapse += Time.deltaTime * animateSpeed;

        ListenToMusic();
        Create(movement, timeLapse);
    }

    private float[] ApplyFrequency(float[] heightmap)
    {
        // Only execute if necessary
        if (currentResponseScale > 1)
        {
            for (int h = 0; h < heightmap.Length; ++h)
            {
                heightmap[h] *= currentResponseScale;
            }
        }

        return heightmap;
    }

    private void Create(float movement, float time)
    {
        // Pass information to shader
        mat.SetColor("_Color", terrainMainColor);
        mat.SetColor("_OutlineColor", terrainOutlineColor);
        mat.SetInt("_HeightCap", (terrainHeightCap == 0) ? height : terrainHeightCap);
        mat.SetInt("_OutlinePixel", (showTerrainOutline) ? terrainOutlineSize : 0);

        // Generate Noise to create terrrain line
        float[] heightmap = noise.GenerateNoiseMap(width, height, scale, octaves, persistance, lacunarity, movement, time);

        // Apply music frequency to height map
        heightmap = ApplyFrequency(heightmap);

        // Transform the heightmap to 2D texture for shader processing
        Texture2D heightmapTexture = new Texture2D(width, 1);
        Color[] colors = new Color[width];
        for(int i = 0; i < heightmap.Length; ++i)
        {
            colors[i] = new Color(heightmap[i] / height, 0, 0);
        }
        heightmapTexture.SetPixels(colors);
        heightmapTexture.Apply();

        mat.SetTexture("_HeightMap", heightmapTexture);
    }

    private void ListenToMusic()
    {
        // If the texture is listening to BGM music
        if (listenToAudio)
        {
            // Obtain the audio band result from AudioAnalyzer, and apply response scale to the texture if the result exceed the threshold
            if (AudioAnalyzer.instance.audioBand[audioChannel] > audioThreshold)
            {
                currentResponseScale = musicResponseScale;
            }
        }

        // Buffer the texture scale jitter
        currentResponseScale -= Time.deltaTime * musicDecayRate;
        currentResponseScale = Mathf.Clamp(currentResponseScale, 1, musicResponseScale);
    }

    public void Clear()
    {
        // Fill the color array
        List<Color> colors = new List<Color>();
        for (int h = 0; h < height; ++h)
        {
            colors.AddRange(Enumerable.Repeat<Color>(Color.clear, width));
        }
        texture.SetPixels(colors.ToArray());
    }

    public void Generate()
    {
        Clear();

        ListenToMusic();
        Create(0,0);
    }
}
