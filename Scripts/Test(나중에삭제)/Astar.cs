using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour
{
    MapMaker.TileType[,] _map;



    class Pos
    {
        public int posX { get; set; }
        public int posY { get; set; }

        public Pos (int x, int y)
        {
            posX = x;
            posY = y;
        }
    }

    private List<Pos> moveList = new List<Pos>();

    private Pos Destination;
    private Pos Player;

    public bool isMove = false;

    void Start()
    {
        //_map = FindObjectOfType<GameManager>().map;
        Destination = new Pos(_map.GetLength(0) - 1, _map.GetLength(1) - 1);
        Player = new Pos(0, 0);

        AstarAlgorithm(_map);
    }

    private int moveCount = 0;
    private float timeCount = 0;
    void Update()
    {
        if (isMove)
        {
            timeCount += Time.deltaTime;
            if (timeCount > 0.5 && moveList.Count > moveCount)
            {
                //Debug.Log(moveCount + "번째 이동 x : " + moveList[moveCount].posX + ", y : " + moveList[moveCount].posY);
                transform.position = new Vector3(moveList[moveCount].posX, moveList[moveCount].posY, 0);
                timeCount = 0;
                moveCount++;
            }
        }
        
    }

    void AstarAlgorithm (MapMaker.TileType[,] map)
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
        int startF = Math.Abs(Destination.posX - Player.posX) + Math.Abs(Destination.posY - Player.posY);
        open[Player.posX, Player.posY] = startF;
        pq.Push(new PQNode() { F = startF, G = 0, H = startF, posX = Player.posX, posY = Player.posY });
        parent[Player.posX, Player.posY] = new Pos(Player.posX, Player.posY);

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
                if (map[nextX, nextY] == MapMaker.TileType.Wall)
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

        PathCalculatorFromParent(parent);
    }
    //int debugCount = 0;
    void PathCalculatorFromParent(Pos[,] parent)
    {
        int x = Destination.posX;
        int y = Destination.posY;

        moveList.Add(new Pos(x, y));                    //? 목적지를 첫번째로 추가
        while (parent[x, y].posX != x || parent[x, y].posY != y)  //? 목적지부터 경로를 거슬러 올라가서 하나씩 추가
        {
            moveList.Add(parent[x, y]);
            Pos pos = parent[x, y];
            x = pos.posX;
            y = pos.posY;



            //moveList.Add(parent[x, y]);
            //x = parent[x, y].posX;
            //y = parent[x, y].posY;

            //? 아 씨발 드디어찾았다 위에꺼는 지금 x 에 저걸 대입하는순간 아래 y에서 x를 사용하기때문에 바뀐값이 들어가네 씨발;;; 뭔가 이상하더라
            //? 이거 찾는데 한 2시간이상걸린듯 개 ㅈ같은거시발

            //moveList.Add(new Pos(x, y));
            //Pos pos = parent[x, y];
            //x = pos.posX;
            //y = pos.posY;
        }
        //moveList.Add(new Pos(x, y));
        moveList.Reverse();                                      //? 전체 경로를 추가했으면 시작부터 탐색해야하므로 반전시켜준다.
    }
}



//class PriorityQueue<T> where T : IComparable<T> 
//{
//    //? 우선순위 큐 구현 : 우선순위 큐란 부모가 항상 자식보다 큰 이진트리를 말함(힙 트리)
//    //? 두번째 규칙은 항상 모든 레벨에 노드가 꽉차있어야하고(마지막층 제외), 채울 땐 항상 왼쪽부터

//    //? i 노드의 왼쪽 자식은 (2 * i) + 1
//    //? i 노드의 왼쪽 자식은 (2 * i) + 2
//    //? i 노드의 부모는 (i - 1) / 2 (소수점을 버려서 +2는 상관없음)

//    List<T> _list = new List<T>();
//    public void Push(T data)
//    {
//        _list.Add(data);

//        int now = _list.Count - 1;

//        while (now > 0)
//        {
//            int parent = (now - 1) / 2;

//            if (_list[now].CompareTo(_list[parent]) > 0)
//            {
//                T temp = _list[parent];
//                _list[parent] = _list[now];
//                _list[now] = temp;

//                now = parent;
//            }
//            else
//                break;
//        }
//    }

//    public T Pop()
//    {
//        T pop = _list[0];

//        int count = _list.Count - 1;
//        _list[0] = _list[count];
//        _list.RemoveAt(count);
//        count--;

//        int start = 0;
//        while (true)
//        {
//            int left = 2 * start + 1;  //? 자식 트리 찾기 - 왼쪽
//            int right = 2 * start + 2; //? 자식 트리 찾기 - 오른쪽


//            int next = start;                //? 값이 바뀌면 저장할 변수

//            if (left <= count && _list[next].CompareTo(_list[left]) == -1)      //? 왼쪽이랑 비교 = 높은값 저장 , 자식이 있는지 확인(count보다 작으면 최하위)
//            {                   //! *** 여기서 조건 두개중에 앞에부분이 거짓이면 뒷부분을 아예 확인을 안해서 인덱스 에러가 안나는데
//                next = left;    //! 조건 순서를 반대로하면 앞의 조건부터 읽기때문에 자식이 없는 상황이면 인덱스에러가뜸
//            }
//            if (right <= count && _list[next].CompareTo(_list[right]) == -1)     //? 오른쪽이랑 비교 = 기존값과 왼쪽값 비교에서 높은값이랑 비교하기때문에 왼,오,기존 중에 높은값
//            {
//                next = right;
//            }

//            if (start == next)  //? 왼쪽 오른쪽 둘다 비교해봤는데 숫자가 그대로라면 바로 탈출
//            {
//                break;
//            }

//            T temp = _list[start];
//            _list[start] = _list[next];
//            _list[next] = temp;

//            start = next;
//        }

//        return pop;
//    }


//    public int Count
//    {
//        get { return _list.Count; }
//    }
//}

//struct PQNode : IComparable<PQNode>
//{
//    public int F;
//    public int G;
//    public int H;
//    public int posX;
//    public int posY;

//    public int CompareTo(PQNode other)
//    {
//        if (F == other.F)
//        {
//            return 0;
//        }
//        return F < other.F ? 1 : -1;  //이부분이 PriorityQueue에서 F가 높은순으로 트리를 만들지 낮은순으로 만들지 결정함
//    }
//}
