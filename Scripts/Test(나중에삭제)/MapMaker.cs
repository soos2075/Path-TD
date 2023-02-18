using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMaker : MonoBehaviour
{
    public GameObject tile_Wall;
    public GameObject tile_Ground;

    public int size_X;
    public int size_Y;
    void Start()
    {
        //TileType[,] map = MakeTileMap(size_X, size_Y);
        //RenderTileMap(map);
    }


    void Update()
    {
        
    }
    public enum TileType
    {
        Ground = 0,
        Wall = 1
    }

    public TileType[,] MakeTileMap(int sizeX, int sizeY)
    {
        TileType[,] map = new TileType[sizeX,sizeY];
        for (int i = 0; i < sizeX; i++)
        {
            for (int k = 0; k < sizeY; k++)
            {
                if (i % 2 == 1 && k % 2 == 1)
                {
                    map[i, k] = TileType.Wall;

                    map[i + Random.Range(-1,2), k] = TileType.Wall;
                }

                if (k % 2 == 1)
                {
                    map[i, k] = TileType.Wall;
                }
            }
        }
        for (int y = 0; y < sizeY; y++)
        {
            if (y % 2 == 1)
            {
                map[Random.Range(0, sizeX), y] = TileType.Ground;
                map[Random.Range(0, sizeX), y] = TileType.Ground;
            }
        }

        map[sizeX - 1, sizeY - 1] = TileType.Ground;

        return map;
    }

    public void RenderTileMap(TileType[,] map)
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int k = 0; k < map.GetLength(1); k++)
            {
                if (map[i,k] == TileType.Ground)
                {
                    Instantiate(tile_Ground, new Vector3(i, k, 0), Quaternion.identity);
                }
                else if (map[i, k] == TileType.Wall)
                {
                    Instantiate(tile_Wall, new Vector3(i, k, 0), Quaternion.identity);
                }
            }
        }
    }


}

