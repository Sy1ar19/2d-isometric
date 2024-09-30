using UnityEngine;

[ExecuteInEditMode]
public class NoiseGenerator : MonoBehaviour
{
    [Range(1, 512)] [SerializeField] private int _width = 256;  
    [Range(1, 512)][SerializeField] private int _height = 256;
    [SerializeField] private float _scale = 20f;
    [SerializeField] private bool _autoUpdate;
    [SerializeField] private Texture2D _noiseTexture;

    [SerializeField] private int _seed = 0;

    public float Scale => _scale;
    public Texture2D NoiseTexture => _noiseTexture;

    public float[,] GenerateNoiseMap()
    {
        float[,] noiseMap = new float[_width, _height];

        System.Random prng = new System.Random(_seed);
        float offsetX = prng.Next(-100000, 100000);
        float offsetY = prng.Next(-100000, 100000);

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                float xCoord = (x + offsetX) / _width * _scale;
                float yCoord = (y + offsetY) / _height * _scale;

                noiseMap[x, y] = Mathf.PerlinNoise(xCoord, yCoord);
            }
        }

        return noiseMap;
    }

    public Texture2D GenerateTexture()
    {
        float[,] noiseMap = GenerateNoiseMap();
        _noiseTexture = new Texture2D(_width, _height);

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                Color color = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
                _noiseTexture.SetPixel(x, y, color);
            }
        }

        _noiseTexture.Apply();
        return _noiseTexture;
    }

    private void OnValidate()
    {
        if (_autoUpdate)
        {
            GenerateTexture();
        }
    }
}
