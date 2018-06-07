using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureGenerator : MonoBehaviour
{
    [SerializeField]
    private int width = 1920;

    [SerializeField]
    private int height = 1080;

    [SerializeField]
    private int resolution = 1000;

    Texture2D texture;

    private void Awake()
    {
        for (int x = 0; x < width; ++x)
        {
            Debug.Log(Perlin.Noise((float)x / (float)resolution));
        }
    }
}
