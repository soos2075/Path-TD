using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navi : MonoBehaviour
{
    public class Pos
    {
        public int posX { get; set; }
        public int posY { get; set; }

        public Pos(int x, int y)
        {
            posX = x;
            posY = y;
        }
    }

    public List<Pos> moveList = new List<Pos>();

    private Pos Destination;
    private Pos StartPos;

    private Define.TileType[,] _map;

    public bool path_correct_Check;


    //public GameObject particle_Line;

    //private LineRenderer myLine;


    public void Init_Navi()
    {
        StageManager.Instance.ResetEvent();

        SearchMap(true);

        StageManager.Instance.Event_WaveStart += LineDisible;
        StageManager.Instance.Event_WaveOver += SearchMap;
    }

    private void Start()
    {
        Init_Navi();
    }


    void SearchMap()
    {
        SearchMap(true);
    }


    public void SearchMap(bool refresh)
    {
        _map = GameManager.m_Map.nowMap;

        StartPos = new Pos(GameManager.m_Map.Pos_Start.x, GameManager.m_Map.Pos_Start.y);
        Destination = new Pos(GameManager.m_Map.Pos_Goal.x, GameManager.m_Map.Pos_Goal.y);

        AstarAlgorithm(_map, refresh);
    }


    void AstarAlgorithm(Define.TileType[,] map, bool refresh)
    {
        // U L D R UL DL DR UR 위 왼 아래 오른 위왼 아래왼 아래오른 위오른
        int[] deltaX = new int[4] { 0, -1, 0, 1 };
        int[] deltaY = new int[4] { 1, 0, -1, 0 };

        // 점수 매기기
        // F = G + H
        // F = 최종 점수 (작을 수록 좋음, 경로에 따라 달라짐)
        // G = 시작점에서 해당 좌표까지 이동하는데 드는 비용 (작을 수록 좋음, 경로에 따라 달라짐)
        // H = 목적지에서 얼마나 가까운지 (작을 수록 좋음, 고정)

        // (y, x) 이미 방문했는지 여부 (방문 = closed 상태)
        bool[,] closed = new bool[map.GetLength(0), map.GetLength(1)]; // CloseList

        // (y, x) 가는 길을 한 번이라도 발견했는지
        // 발견X => MaxValue
        // 발견O => F = G + H
        int[,] open = new int[map.GetLength(0), map.GetLength(1)]; // OpenList
        for (int y = 0; y < map.GetLength(0); y++)
            for (int x = 0; x < map.GetLength(1); x++)
                open[y, x] = int.MaxValue;

        Pos[,] parent = new Pos[map.GetLength(0), map.GetLength(1)];

        // 오픈리스트에 있는 정보들 중에서, 가장 좋은 후보를 빠르게 뽑아오기 위한 도구
        PriorityQueue<PQNode> pq = new PriorityQueue<PQNode>();

        // 시작점 발견 (예약 진행)
        int startF = Math.Abs(Destination.posX - StartPos.posX) + Math.Abs(Destination.posY - StartPos.posY);
        open[StartPos.posX, StartPos.posY] = startF;
        pq.Push(new PQNode() { F = startF, G = 0, H = startF, posX = StartPos.posX, posY = StartPos.posY });
        parent[StartPos.posX, StartPos.posY] = new Pos(StartPos.posX, StartPos.posY);

        while (pq.Count > 0)
        {
            // 제일 좋은 후보를 찾는다
            PQNode node = pq.Pop();
            // 동일한 좌표를 여러 경로로 찾아서, 더 빠른 경로로 인해서 이미 방문(closed)된 경우 스킵
            if (closed[node.posX, node.posY])
                continue;

            // 방문한다
            closed[node.posX, node.posY] = true;
            // 목적지 도착했으면 바로 종료
            if (node.posX == Destination.posX && node.posY == Destination.posY)
                break;

            // 상하좌우 등 이동할 수 있는 좌표인지 확인해서 예약(open)한다
            for (int i = 0; i < deltaX.Length; i++)
            {
                int nextX = node.posX + deltaX[i];
                int nextY = node.posY + deltaY[i];

                // 유효 범위를 벗어났으면 스킵
                if (nextX < 0 || nextX >= map.GetLength(0) || nextY < 0 || nextY >= map.GetLength(1))
                    continue;
                // 벽으로 막혀서 갈 수 없으면 스킵
                if (map[nextX, nextY] == Define.TileType.Floor 
                    || map[nextX, nextY] == Define.TileType.Changeable_Floor
                    || map[nextX, nextY] == Define.TileType.Changeable_Tower
                    || map[nextX, nextY] == Define.TileType.Tower)
                    continue;
                // 이미 방문한 곳이면 스킵
                if (closed[nextX, nextY])
                    continue;

                // 비용 계산
                int g = node.G + 1; //cost[i];
                int h = Math.Abs(Destination.posX - nextX) + Math.Abs(Destination.posY - nextY);
                // 다른 경로에서 더 빠른 길 이미 찾았으면 스킵
                if (open[nextX, nextY] < g + h)
                    continue;

                //Debug.Log(debugCount + "번째  x : " + nextX + "   y : " + nextY);
                //debugCount++;

                // 예약 진행
                open[nextX, nextY] = g + h;
                pq.Push(new PQNode() { F = g + h, G = g, H = h, posX = nextX, posY = nextY });
                parent[nextX, nextY] = new Pos(node.posX, node.posY);
                //Debug.Log(debugCount + "번째  x : " + nextX + "   y : " + nextY);
                //debugCount++;
            }
        }

        if (parent[Destination.posX, Destination.posY] != null)
        {
            if (refresh)
            {
                PathCalculatorFromParent(parent);
            }
            path_correct_Check = true;
        }
        else
        {
            Debug.Log("길이 막혔음");
            path_correct_Check = false;
        }
    }
    void PathCalculatorFromParent(Pos[,] parent)
    {
        moveList.Clear();

        int x = Destination.posX;
        int y = Destination.posY;

        moveList.Add(new Pos(x, y));                    //? 목적지를 첫번째로 추가
        while (parent[x, y].posX != x || parent[x, y].posY != y)  //? 목적지부터 경로를 거슬러 올라가서 하나씩 추가
        {
            moveList.Add(parent[x, y]);
            Pos pos = parent[x, y];
            x = pos.posX;
            y = pos.posY;
        }
        moveList.Reverse();                                      //? 전체 경로를 추가했으면 시작부터 탐색해야하므로 반전시켜준다.

        DrawLine(moveList);
    }


    Coroutine C_line;

    private void DrawLine(List<Pos> pos)
    {
        //if (myLine == null)
        //{
        //    myLine = Instantiate(particle_Line).GetComponent<LineRenderer>();
        //}
        //myLine.positionCount = pos.Count;

        //for (int i = 0; i < pos.Count; i++)
        //{
        //    myLine.SetPosition(i, new Vector3(pos[i].posX, pos[i].posY, 0));
        //}
        //LineVisible();

        if (C_line != null)
        {
            StopCoroutine(C_line);
        }
        C_line = StartCoroutine(LinePainter(pos));
    }

    //public IEnumerator LineDisable ()
    //{
    //    if (GameManager.Instance.PlayState == Define.PlayMode.Play)
    //    {
    //        myLine.gameObject.SetActive(false);

    //        yield return new WaitUntil(() => GameManager.Instance.PlayState != Define.PlayMode.Play);
    //        myLine.gameObject.SetActive(true);
    //    }  
    //}

    //public void LineVisible ()
    //{
    //    if (GameManager.Instance.PlayState != Define.PlayMode.Play && !myLine.gameObject.activeSelf)
    //    {
    //        myLine.gameObject.SetActive(true);
    //    }
    //}
    [SerializeField]
    List<GameObject> liner = new List<GameObject>();
    IEnumerator LinePainter(List<Pos> pos)
    {
        LineDisible();

        GameObject root = GameObject.Find("@LinePainter");
        if (root == null)
        {
            root = new GameObject { name = "@LinePainter" };
        }

        for (int i = 1; i + 1 < pos.Count; i++)
        {
            liner.Add(GameManager.m_Resource.Instant_Prefab("Navi/LinePainter_Early", new Vector3(pos[i].posX, pos[i].posY, 0), root.transform));
        }

        for (int i = 0; i < liner.Count; i++)
        {
            //yield return null;
            yield return new WaitForSeconds(Time.unscaledDeltaTime * 2);
            liner[i].GetComponentInChildren<ParticleSystem>().Stop();
            liner[i].GetComponentInChildren<ParticleSystem>().Play();
            //yield return null;
        }
    }

    void LineDisible()
    {
        if (C_line != null)
        {
            StopCoroutine(C_line);
        }

        if (liner != null)
        {
            foreach (var item in liner)
            {
                GameManager.m_Resource.Disable_Prefab(item);
            }
            liner.Clear();
        }
    }



}
