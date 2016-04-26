using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TilePRNGMapGenerator : MonoBehaviour {

    public TileMap[] maps;
    public int mapIndex;

    public Transform tileSprite;
    public float isometricTileHeight;
    public float isometricTileWidth;


    TileMap currentMap;

    
    public bool autoUpdate;
    public TerrainType[] regions;

    void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        currentMap = maps[mapIndex];
        float[,] noiseMap = Noise.GenerateNoiseMap(currentMap.mapSize.x, currentMap.mapSize.y, currentMap.seed, currentMap.noiseScale, currentMap.octaves, currentMap.persistence, currentMap.lacunarity, currentMap.offset);
        
        

        //create map holder object
        string holderName = "Generated Map";
        if (transform.FindChild(holderName))
        {
            DestroyImmediate(transform.FindChild(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        //spawning tiles
        int sortingOrderIndex = 0;
        for (int x = 0; x < currentMap.mapSize.x; x++)
        {
            for (int y = 0; y < currentMap.mapSize.y; y++)
            {
                float tileHeight = noiseMap[x,y];
                TerrainType tileTerrain;
                for (int i = 0; i < regions.Length; i++)
                {
                    if (tileHeight <= regions[i].terrainHeight)
                    {
                        tileTerrain = regions[i];
                        Vector2 tilePosition = CoordToPosition(x, y);
                        for (int z = 0; z < Mathf.RoundToInt(currentMap.tileHeightCurve.Evaluate(tileHeight)*10); z++)
                        {
                            Transform newTile = Instantiate(tileSprite, tilePosition + (new Vector2(0f,.5f) * z), Quaternion.identity) as Transform;

                            newTile.parent = mapHolder;

                            SpriteRenderer spriteRenderer = newTile.GetComponent<SpriteRenderer>();
                            spriteRenderer.sprite = tileTerrain.sprite;
                            spriteRenderer.sortingOrder = sortingOrderIndex;
                            sortingOrderIndex++;
                        }
                        
                        break;
                    }
                }
                
                

                
               
            }
        }
        
    }

    Vector2 CoordToPosition(int x, int y)
    {
        float zeroX = 0f +.5f * x - .5f * y;
        float zeroY = (.25f * currentMap.mapSize.y + .25f * currentMap.mapSize.x) / 2f  -.25f * y - .25f * x ;
        return new Vector2(zeroX, zeroY);
    }

    [System.Serializable]
    public struct Coord
    {
        public int x;
        public int y;

        public Coord(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }

    [System.Serializable]
    public class TileMap
    {
        public Coord mapSize;
        public float noiseScale;
        public int octaves;
        [Range(0, 1)]
        public float persistence;
        public float lacunarity;
        public Vector2 offset;
        public AnimationCurve tileHeightCurve;
        public int seed;
        public float heightMultiplier;

        public Coord mapCenter
        {
            get
            {
                return new Coord(mapSize.x / 2, mapSize.y / 2);
            }
        }
    }

    void OnValidate()
    {
        if (currentMap.mapSize.x < 1)
        {
            currentMap.mapSize.x = 1;
        }
        if (currentMap.mapSize.y < 1)
        {
            currentMap.mapSize.y = 1;
        }
        if (currentMap.lacunarity < 1)
        {
            currentMap.lacunarity = 1;
        }
        if (currentMap.octaves < 0)
        {
            currentMap.octaves = 0;
        }


    }
    [System.Serializable]
    public struct TerrainType
    {
        public string name;
        public float terrainHeight;
        public Sprite sprite;
    }
}
