using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NoiseGenerator))]
public class NoiseGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        NoiseGenerator noiseGen = (NoiseGenerator)target;

        if (GUILayout.Button("Generate Noise"))
        {
            noiseGen.GenerateTexture();

            ProceduralMapGenerator mapGen = FindObjectOfType<ProceduralMapGenerator>();
            if (mapGen != null)
            {
                mapGen.GenerateMap();
            }
        }

        if (noiseGen.NoiseTexture != null)
        {
            GUILayout.Label("Noise Preview:");
            GUILayout.Label(noiseGen.NoiseTexture, GUILayout.Width(256), GUILayout.Height(256));
        }
    }
}
