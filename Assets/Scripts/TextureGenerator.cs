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

    [Range(0, 7)]
    [SerializeField]
    private int audioChannel;

    private Texture2D texture;
    private SpriteRenderer spriteRenderer;
    private float movement = 0;
    private float timeLapse = 0;
    private Noise noise;

    private void Awake()
    {
        // Create a Texture2D object with given width and height
        texture = new Texture2D(width, height);
        noise = new Noise();

        // Apply the texture to the sprite renderer by converting the Texture2D to Sprite object
        spriteRenderer = GetComponent<SpriteRenderer>();
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        spriteRenderer.sprite = sprite;

        // Initialize the texture with empty colors
        Clear();
    }

    private void Update()
    {
        movement += Time.deltaTime * moveSpeed;
        timeLapse += Time.deltaTime * animateSpeed;

        // Clear();
        Generate(movement, timeLapse);
        Apply();
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

    public void Generate(float movement, float time)
    {
        // Generate Noise to create terrrain line
        float[] heightmap = noise.GenerateNoiseMap(width, height, scale, octaves, persistance, lacunarity, movement, time);
        Color[] colors = new Color[width * height];
        int[] outlines = new int[width];

        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                Color pixelColor = Color.clear;

                // When height is lower than the heightmap noise, fill with blend of colors
                if (h < (int)heightmap[w])
                {
                    pixelColor = terrainMainColor;
                }
                else
                {
                    if (showTerrainOutline && outlines[w] < terrainOutlineSize)
                    {
                        pixelColor = terrainOutlineColor;
                        outlines[w]++;
                    }
                }

                colors[h * width + w] = pixelColor;
            }
        }

        texture.SetPixels(0, 0, width, height, colors);
    }

    public void Apply()
    {
        // Apply the pixel changes to texture
        texture.Apply(false);
    }

    public void ListenToMusic()
    {

    }
}
