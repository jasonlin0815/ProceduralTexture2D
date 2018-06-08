using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class TextureGenerator : MonoBehaviour
{
    [SerializeField]
    private int width = 1920;

    [SerializeField]
    private int height = 1080;

    [SerializeField]
    private Color terrainColor;

    [Range(0, 10)]
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

    private Texture2D texture;
    private SpriteRenderer spriteRenderer;
    private float movement = 0;
    private float timeLapse = 0;

    private void Awake()
    {
        // Create a Texture2D object with given width and height
        texture = new Texture2D(width, height);

        // Generate the texture with time = 0
        Clear();
        Generate(0, 0);
        Apply();
    }

    private void Update()
    {
        movement += Time.deltaTime * moveSpeed;
        timeLapse += Time.deltaTime * animateSpeed;
        Clear();
        Generate(movement, timeLapse);
        Apply();
    }

    public void Clear()
    {
        // Fill the texture with transparent color
        // Traverse using the return texture width and height from Texture2D.blackTexture
        Texture2D blackTexture = Texture2D.blackTexture;
        for (int w = 0; w < texture.width;)
        {
            for (int h = 0; h < texture.height;)
            {
                texture.SetPixels32(w, h, blackTexture.width, blackTexture.height, blackTexture.GetPixels32());
                h += blackTexture.height;
            }
            w += blackTexture.width;
        }
        texture.Apply();
    }

    public void Generate(float movement, float time)
    {
        // Generate Noise to create terrrain line
        float[] heightmap = Noise.GenerateNoiseMap(width, height, scale, octaves, persistance, lacunarity, movement, time);
        for (int w = 0; w < width; ++w)
        {
            texture.SetPixels(w, 0, 1, (int)heightmap[w], Enumerable.Repeat<Color>(terrainColor, (int)heightmap[w]).ToArray());
        }
    }

    public void Apply()
    {
        // Apply the pixel changes to texture
        texture.Apply();

        // Apply the texture to the sprite renderer by converting the Texture2D to Sprite object
        spriteRenderer = GetComponent<SpriteRenderer>();
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        spriteRenderer.sprite = sprite;
    }
}
