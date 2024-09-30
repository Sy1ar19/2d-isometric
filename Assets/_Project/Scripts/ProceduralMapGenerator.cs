using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class ProceduralMapGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase _waterTile, _grassTile, _sandTile, _rockTile;

    [Header("Map Settings")]
    [SerializeField] private int _width = 100;
    [SerializeField] private int _height = 100;

    [Header("Noise Settings")]
    [SerializeField] private NoiseGenerator _noiseGenerator;

    [Header("Biome Thresholds")]
    [SerializeField] private float _waterThreshold = 0.3f;
    [SerializeField] private float _sandThreshold = 0.5f;
    [SerializeField] private float _rockThreshold = 0.8f;

    [Header("Debugging and Visualization")]
    [SerializeField] private bool _showNoiseMap = true;
    private Texture2D _noiseTexture;

    public void GenerateMap()
    {
        if (_noiseGenerator == null)
        {
            Debug.LogError("NoiseGenerator не назначен!");
            return;
        }

        _tilemap.ClearAllTiles();

        float[,] noiseMap = _noiseGenerator.GenerateNoiseMap();

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                float elevation = noiseMap[x, y];

                string biome = GetBiome(elevation);
                SetTile(x, y, biome);

                if (_showNoiseMap)
                {
                    VisualizeNoise(x, y, elevation);
                }
            }
        }

        if (_showNoiseMap && _noiseTexture != null)
        {
            _noiseTexture.Apply();
        }
    }

    string GetBiome(float elevation)
    {
        if (elevation < _waterThreshold)
            return "WATER";
        if (elevation < _sandThreshold)
            return "SAND";
        if (elevation < _rockThreshold)
            return "GRASS";
        return "ROCK";
    }

    void SetTile(int x, int y, string biome)
    {
        switch (biome)
        {
            case "WATER":
                _tilemap.SetTile(new Vector3Int(x, y, 0), _waterTile);
                break;
            case "SAND":
                _tilemap.SetTile(new Vector3Int(x, y, 0), _sandTile);
                break;
            case "GRASS":
                _tilemap.SetTile(new Vector3Int(x, y, 0), _grassTile);
                break;
            case "ROCK":
            default:
                _tilemap.SetTile(new Vector3Int(x, y, 0), _rockTile);
                break;
        }
    }

    void VisualizeNoise(int x, int y, float elevation)
    {
        if (_noiseTexture == null)
        {
            _noiseTexture = new Texture2D(_width, _height);
        }

        Color color = Color.Lerp(Color.black, Color.white, elevation);
        _noiseTexture.SetPixel(x, y, color);
    }
}
