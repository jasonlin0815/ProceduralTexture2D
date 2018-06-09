using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TextureGenerator))]
public class TextureGeneratorEditor : Editor
{
    private bool showTextureOptions = true;
    private bool showTerrainOptions = false;
    private bool showNoiseOptions = false;
    private bool showAudioOptions = false;

    #region Texture options
    SerializedProperty width;
    SerializedProperty height;
    #endregion

    #region Terrain options
    SerializedProperty terrainHeightCap;
    SerializedProperty terrainMainColor;
    SerializedProperty showTerrainOutline;
    SerializedProperty terrainOutlineColor;
    SerializedProperty terrainOutlineSize;
    #endregion

    #region Noise options
    SerializedProperty moveSpeed;
    SerializedProperty animateSpeed;
    SerializedProperty octaves;
    SerializedProperty persistance;
    SerializedProperty lacunarity;
    SerializedProperty scale;
    #endregion

    #region Audio options
    SerializedProperty listenToAudio;
    SerializedProperty audioChannel;
    SerializedProperty audioThreshold;
    SerializedProperty musicResponseScale;
    SerializedProperty musicDecayRate;
    #endregion

    private void OnEnable()
    {
        width = serializedObject.FindProperty("width");
        height = serializedObject.FindProperty("height");

        terrainHeightCap = serializedObject.FindProperty("terrainHeightCap");
        terrainMainColor = serializedObject.FindProperty("terrainMainColor");
        showTerrainOutline = serializedObject.FindProperty("showTerrainOutline");
        terrainOutlineColor = serializedObject.FindProperty("terrainOutlineColor");
        terrainOutlineSize = serializedObject.FindProperty("terrainOutlineSize");

        moveSpeed = serializedObject.FindProperty("moveSpeed");
        animateSpeed = serializedObject.FindProperty("animateSpeed");
        octaves = serializedObject.FindProperty("octaves");
        persistance = serializedObject.FindProperty("persistance");
        lacunarity = serializedObject.FindProperty("lacunarity");
        scale = serializedObject.FindProperty("scale");

        listenToAudio = serializedObject.FindProperty("listenToAudio");
        audioChannel = serializedObject.FindProperty("audioChannel");
        audioThreshold = serializedObject.FindProperty("audioThreshold");
        musicResponseScale = serializedObject.FindProperty("musicResponseScale");
        musicDecayRate = serializedObject.FindProperty("musicDecayRate");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        TextureGUI();
        TerrainGUI();
        NoiseGUI();
        AudioGUI();

        // EditorGUI();

        serializedObject.ApplyModifiedProperties();
    }

    private void TextureGUI()
    {
        showTextureOptions = EditorGUILayout.Foldout(showTextureOptions, "Texture Options");
        if (showTextureOptions)
        {
            EditorGUILayout.PropertyField(width);
            EditorGUILayout.PropertyField(height);
        }
    }

    private void TerrainGUI()
    {
        showTerrainOptions = EditorGUILayout.Foldout(showTerrainOptions, "Terrain Options");
        if (showTerrainOptions)
        {
            EditorGUILayout.PropertyField(terrainHeightCap);
            EditorGUILayout.PropertyField(terrainMainColor);

            showTerrainOutline.boolValue = EditorGUILayout.BeginToggleGroup("Terrain Outline", showTerrainOutline.boolValue);
            EditorGUILayout.PropertyField(terrainOutlineColor);
            EditorGUILayout.PropertyField(terrainOutlineSize);
            EditorGUILayout.EndToggleGroup();
        }
    }

    private void NoiseGUI()
    {
        showNoiseOptions = EditorGUILayout.Foldout(showNoiseOptions, "Noise Options");
        if (showNoiseOptions)
        {
            EditorGUILayout.PropertyField(moveSpeed);
            EditorGUILayout.PropertyField(animateSpeed);
            EditorGUILayout.PropertyField(octaves);
            EditorGUILayout.PropertyField(persistance);
            EditorGUILayout.PropertyField(lacunarity);
            EditorGUILayout.PropertyField(scale);
        }
    }

    private void AudioGUI()
    {
        showAudioOptions = EditorGUILayout.Foldout(showAudioOptions, "Audio Options");
        if (showAudioOptions)
        {
            listenToAudio.boolValue = EditorGUILayout.BeginToggleGroup("Audio Listener", listenToAudio.boolValue);
            EditorGUILayout.PropertyField(audioChannel);
            EditorGUILayout.PropertyField(audioThreshold);
            EditorGUILayout.PropertyField(musicResponseScale);
            EditorGUILayout.PropertyField(musicDecayRate);

            EditorGUILayout.EndToggleGroup();
        }
    }

    private void EditorGUI()
    {
        TextureGenerator textureGenerator = (TextureGenerator)target;
        if (GUILayout.Button("Generate"))
        {
            textureGenerator.Generate();
        }
    }
}
