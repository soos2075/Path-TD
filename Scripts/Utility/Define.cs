using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{

    public enum Language
    {
        Eng = 0,
        Kor = 1,

    }



    public enum TileType
    {
        Blank = 0, //? 타워건설불가, 없애기불가, 이동불가
        Ground = 1, //? 이동경로, 막을 수 없음
        Floor = 2,  //? 타워지을 발판, 없앨 수 없음
        Tower = 3,  //? 타워, Floor랑만 치환이 됨
        Changeable_Ground = 4,  //? 이동경로, 막을 수 있음
        Changeable_Floor = 5,   //? 타워지을 발판, 없앨 수 있음
        Changeable_Tower = 6,   //? 타워, Changeble_Floor랑만 치환이 됨
        Start = 7,
        Goal = 8,
    }

    public enum AudioType
    {
        Effect,
        BGM,
    }


    public enum MouseEvent
    {
        None = 0,
        Click = 1,
        Press = 2
    }

    public enum PlayMode
    {
        Preparation,
        Play,
        Pause,
        MapEdit,
        TowerBuild,
        SaveLoad,
    }



    //public enum TowerName
    //{
    //    FireTower = 0,
    //    LazerTower = 1,
    //    SplashTower = 2,
    //    MultiTower = 3,
    //    PoisonTower = 4,
    //    SpiderTower = 5,
    //    SlowTower = 6,
    //    BuffTower = 7,
    //    SpearTower = 8,
    //    AirTower = 9,
    //}

    public static readonly string[] TowerName =
    {
        "FireTower" ,
        "LazerTower",
        "SplashTower",
        "MultiTower",
        "PoisonTower",
        "SpiderTower",
        "SlowTower",
        "BuffTower",
        "SpearTower",
        "AirTower",
        "ThunderTower",
        "ArcaneTower",
        "WaterTower",
        "LightningTower",
        "CoinTower",


    };

}

class PriorityQueue<T> where T : IComparable<T>
{
    //? 우선순위 큐 구현 : 우선순위 큐란 부모가 항상 자식보다 큰 이진트리를 말함(힙 트리)
    //? 두번째 규칙은 항상 모든 레벨에 노드가 꽉차있어야하고(마지막층 제외), 채울 땐 항상 왼쪽부터

    //? i 노드의 왼쪽 자식은 (2 * i) + 1
    //? i 노드의 왼쪽 자식은 (2 * i) + 2
    //? i 노드의 부모는 (i - 1) / 2 (소수점을 버려서 +2는 상관없음)

    List<T> _list = new List<T>();
    public void Push(T data)
    {
        _list.Add(data);

        int now = _list.Count - 1;

        while (now > 0)
        {
            int parent = (now - 1) / 2;

            if (_list[now].CompareTo(_list[parent]) > 0)
            {
                T temp = _list[parent];
                _list[parent] = _list[now];
                _list[now] = temp;

                now = parent;
            }
            else
                break;
        }
    }

    public T Pop()
    {
        T pop = _list[0];

        int count = _list.Count - 1;
        _list[0] = _list[count];
        _list.RemoveAt(count);
        count--;

        int start = 0;
        while (true)
        {
            int left = 2 * start + 1;  //? 자식 트리 찾기 - 왼쪽
            int right = 2 * start + 2; //? 자식 트리 찾기 - 오른쪽


            int next = start;                //? 값이 바뀌면 저장할 변수

            if (left <= count && _list[next].CompareTo(_list[left]) == -1)      //? 왼쪽이랑 비교 = 높은값 저장 , 자식이 있는지 확인(count보다 작으면 최하위)
            {                   //! *** 여기서 조건 두개중에 앞에부분이 거짓이면 뒷부분을 아예 확인을 안해서 인덱스 에러가 안나는데
                next = left;    //! 조건 순서를 반대로하면 앞의 조건부터 읽기때문에 자식이 없는 상황이면 인덱스에러가뜸
            }
            if (right <= count && _list[next].CompareTo(_list[right]) == -1)     //? 오른쪽이랑 비교 = 기존값과 왼쪽값 비교에서 높은값이랑 비교하기때문에 왼,오,기존 중에 높은값
            {
                next = right;
            }

            if (start == next)  //? 왼쪽 오른쪽 둘다 비교해봤는데 숫자가 그대로라면 바로 탈출
            {
                break;
            }

            T temp = _list[start];
            _list[start] = _list[next];
            _list[next] = temp;

            start = next;
        }

        return pop;
    }


    public int Count
    {
        get { return _list.Count; }
    }
}

struct PQNode : IComparable<PQNode>
{
    public int F;
    public int G;
    public int H;
    public int posX;
    public int posY;

    public int CompareTo(PQNode other)
    {
        if (F == other.F)
        {
            return 0;
        }
        return F < other.F ? 1 : -1;  //이부분이 PriorityQueue에서 F가 높은순으로 트리를 만들지 낮은순으로 만들지 결정함
    }
}

