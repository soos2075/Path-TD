using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Maker : MonoBehaviour
{
    public int size_X;
    public int size_Y;

    public bool openCheck = true;
    public int resolution = 1;

    public Vector2Int start_pos;
    public Vector2Int goal_pos;


    private void Awake()
    {
        Define.TileType[,] myMap = GameManager.m_Map.MakeTileMap(size_X, size_Y, start_pos, goal_pos, openCheck);
        GameManager.m_Map.RenderTileMap(myMap, resolution);
    }


    void Start()
    {

    }
    void Update()
    {
        
    }



}
