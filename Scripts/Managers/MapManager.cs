using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager
{
    GameObject Tile_Root
    {
        get
        {
            GameObject root = GameObject.Find("@Tile_Root");
            if (root == null)
            {
                root = new GameObject { name = "@Tile_Root" };
            }
            return root;
        }
    }

    public Vector2Int Pos_Start { get; set; }
    public Vector2Int Pos_Goal { get; set; }

    public int Size_X { get; set; }
    public int Size_Y { get; set; }

    public Define.TileType[,] nowMap { get; set; } = null;

    public Define.TileType[,] MakeTileMap(int sizeX, int sizeY , bool open)
    {
        Define.TileType[,] map = new Define.TileType[sizeX, sizeY];
        for (int i = 0; i < sizeX; i++)
        {
            for (int k = 0; k < sizeY; k++)
            {
                if (open)
                {
                    map[i, k] = Define.TileType.Changeable_Ground;
                }
                else
                {
                    map[i, k] = Define.TileType.Changeable_Floor;
                }
                
            }
        }
        map[0, 0] = Define.TileType.Start;
        map[sizeX - 1, sizeY - 1] = Define.TileType.Goal;

        //GameManager.m_Resource.Instant_Prefab("Tile/Tile_Start", new Vector3(0, 0, 0), Tile_Root.transform);
        //GameManager.m_Resource.Instant_Prefab("Tile/Tile_Goal", new Vector3(sizeX - 1, sizeY - 1, 0), Tile_Root.transform);

        nowMap = map;
        return map;
    }

    public Define.TileType[,] MakeTileMap(int sizeX, int sizeY, Vector2Int start_pos, Vector2Int goal_pos, bool open = true)
    {
        Define.TileType[,] map = new Define.TileType[sizeX, sizeY];
        for (int i = 0; i < sizeX; i++)
        {
            for (int k = 0; k < sizeY; k++)
            {
                if (open)
                {
                    map[i, k] = Define.TileType.Changeable_Ground;
                }
                else
                {
                    map[i, k] = Define.TileType.Changeable_Floor;
                }

            }
        }

        map[start_pos.x, start_pos.y] = Define.TileType.Start;
        map[goal_pos.x, goal_pos.y] = Define.TileType.Goal;

        nowMap = map;
        return map;
    }

    //public Define.TileType[,] MakeTileMap(int size, Vector2Int start_pos, Vector2Int goal_pos, bool open = true)
    //{
    //    int sizeX = size * 2;
    //    Define.TileType[,] map = MakeTileMap(sizeX, size, start_pos, goal_pos, open);

    //    nowMap = map;
    //    return map;
    //}

    public Define.TileType[,] MakeTileMap(MapData_SO data)
    {
        Pos_Start = data.temp_Map.startPos;
        Pos_Goal = data.temp_Map.goalPos;

        Size_X = data.temp_Map.sizeX;
        Size_Y = data.temp_Map.sizeY;

        Define.TileType[,] map = new Define.TileType[data.temp_Map.sizeX, data.temp_Map.sizeY];

        for (int i = 0; i < data.temp_Map.sizeX; i++)
        {
            for (int j = 0; j < data.temp_Map.sizeY; j++)
            {
                map[i, j] = data.temp_Map.tile_X[i].tile_Y[j];
            }
        }

        map[data.temp_Map.startPos.x, data.temp_Map.startPos.y] = Define.TileType.Start;
        map[data.temp_Map.goalPos.x, data.temp_Map.goalPos.y] = Define.TileType.Goal;

        nowMap = map;
        return map;
    }

    public Define.TileType[,] MakeTileMap(SaveLoadManager.StageSaveData data)
    {
        Pos_Start = data.startPos;
        Pos_Goal = data.goalPos;
        Size_X = data.sizeX;
        Size_Y = data.sizeY;

        Define.TileType[,] map = new Define.TileType[data.sizeX, data.sizeY];

        for (int i = 0; i < data.sizeX; i++)
        {
            for (int j = 0; j < data.sizeY; j++)
            {
                map[i, j] = data.tile_X[i].tile_Y[j];
            }
        }
        map[data.startPos.x, data.startPos.y] = Define.TileType.Start;
        map[data.goalPos.x, data.goalPos.y] = Define.TileType.Goal;

        nowMap = map;
        return map;
    }



    public void RenderTileMap(Define.TileType[,] map, int resolution = 1)
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int k = 0; k < map.GetLength(1); k++)
            {

                if (map[i, k] == Define.TileType.Tower)
                {
                    GameManager.m_Resource.Instant_Prefab($"Tile/Tile_Floor", new Vector3(i * resolution, k * resolution, 0), Tile_Root.transform);
                }
                else if (map[i, k] == Define.TileType.Changeable_Tower)
                {
                    GameManager.m_Resource.Instant_Prefab($"Tile/Tile_Changeable_Floor", new Vector3(i * resolution, k * resolution, 0), Tile_Root.transform);
                }
                else
                {
                    GameManager.m_Resource.Instant_Prefab($"Tile/Tile_{map[i, k].ToString()}", new Vector3(i * resolution, k * resolution, 0), Tile_Root.transform);
                }
            }
        }
    }


    public void AllMapClear()
    {
        for (int i = 0; i < Tile_Root.transform.childCount; i++)
        {
            GameManager.Destroy(Tile_Root.transform.GetChild(i).gameObject);
        }
    }



}
