using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public struct SpriteTile
    {
        public string name;
        public int tileCode;
        public float height, width;
        public SpriteTile(string name, int tileCode, float height, float width)
        {
            this.name = name;
            this.tileCode = tileCode;
            this.height = height;
            this.width = width;
        }
    }

    [SerializeField] public GameObject[] cellSprites;
    [SerializeField] private int size = 50;
    [SerializeField] bool isNewUser;
    [SerializeField] public SpriteTile[] sprites =
    {
        new SpriteTile("Grass", 0, 1.15f, 1),
        new SpriteTile("Water", 1, 1.15f, 1),
        new SpriteTile("Bitumen", 2, 1, 1)
    };
    public float waterLevel = .4f;
    public float scale = .3f;
    GameObject[,] grid;
    private enum TileCode : int {Grass, Water, Bitumen}

    void Start()
    {
        if (isNewUser)
        {
            DrawTerrain();
        }
        else
        {
            LoadCity();
        }
    }

    void Update()
    {
        
    }

    void DrawTerrain()
    {
        float[,] noiseMap = new float[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * scale, y * scale);
                noiseMap[x, y] = noiseValue;
            }
        }

        float[,] falloffMap = new float[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {

                float xv = x / (float)size * 2 - 1;
                float yv = y / (float)size * 2 - 1;
                float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(yv));
                falloffMap[x, y] = Mathf.Pow(v, 3f) / (Mathf.Pow(v, 3f) + Mathf.Pow(2.2f - 2.2f * v, 3f));
            }
        }
        grid = new GameObject[size, size];    //Create new grid of cells 
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                float noiseValue = noiseMap[x, y];
                noiseValue -= falloffMap[x, y];
                bool isWater = noiseValue < waterLevel;
                CreateCell(ref x, ref y, isWater);
            }
        }
    }

    void CreateCell(ref int x, ref int y, bool isWater)
    {
        SpriteTile s;
        if (isWater) s = sprites[1];
        else s = sprites[0];
        float posX = ((transform.position.x + x) * (s.width * 0.5f) + (transform.position.y + y) * (s.width * -0.5f));
        float posY = ((transform.position.x + x) * (s.height * 0.25f) + (transform.position.y + y) * (s.height * 0.25f));
        GameObject node = Instantiate(cellSprites[s.tileCode], transform);
        node.transform.position = new Vector3(posX, posY, 0);
        node.name = "Cell (" + x + ", " + y + ")";
        grid[x, y] = node;
    }

    void SaveCity()
    {
        
    }

    void LoadCity()
    {

    }
}
