using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
    public delegate void WaveOver();
    //public event WaveOver E_WaveOver;
    public List<WaveOver> WaveOverEventList = new List<WaveOver>();


    public int stage_enemy_Count { get; set; } = 0;
    public int Wave { get; set; }

    GameObject Enemy_Root //? 타워 폴더
    {
        get
        {
            GameObject root = GameObject.Find("@Enemy_Root");
            if (root == null)
            {
                root = new GameObject { name = "@Enemy_Root" };
            }
            return root;
        }
    }

    private int OrderNumber { get; set; }

    public IEnumerator Instant_Wave(int quantity, float seconds, string name)
    {
        WaitForSeconds sec = new WaitForSeconds(seconds);

        if (string.IsNullOrEmpty(name)) //? 웨이브없이 지연시간만 줄경우
        {
            yield return sec;
        }
        else
        {
            for (int i = 0; i < quantity; i++)
            {
                GameObject enemy = GameManager.m_Resource.Instant_Prefab($"Enemy/{name}",
                    new Vector3(GameManager.m_Map.Pos_Start.x, GameManager.m_Map.Pos_Start.y), Enemy_Root.transform);

                Debug.Log($"{enemy.name} 생성 , HP : {enemy.GetComponent<EnemyMove>().Hp} , Speed : {enemy.GetComponent<EnemyMove>().move_Speed}");

                stage_enemy_Count++;

                OrderNumber++;
                enemy.GetComponentInChildren<SpriteRenderer>().sortingOrder = OrderNumber;

                enemy.GetComponent<EnemyMove>().PriorityPoint -= OrderNumber;
                
                yield return sec;
                //enemy_List.Add(enemy);
            }
        }
    }


    public void AllEnemyClear()
    {
        for (int i = 0; i < Enemy_Root.transform.childCount; i++)
        {
            Enemy_Root.transform.GetChild(i).gameObject.SetActive(false);
        }
        stage_enemy_Count = 0;
    }


    public class EnemyPriorityPointComparer : IComparer<Collider2D> //? PriorityPoint로 정렬하기 - 숫자가 클수록 우선순위
    {
        public int Compare(Collider2D x, Collider2D y)
        {
            if (x.GetComponent<EnemyMove>().PriorityPoint > y.GetComponent<EnemyMove>().PriorityPoint)
            {
                return -1;
            }
            else if (x.GetComponent<EnemyMove>().PriorityPoint < y.GetComponent<EnemyMove>().PriorityPoint)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
    public class DistanceEnemyAndTowerComparer : IComparer<Collider2D> //? 적과의 거리로 정렬하기 - 거리가 짧을수록 우선순위
    {
        public int Compare(Collider2D x, Collider2D y)
        {
            if (Vector3.Distance(towerPos, x.transform.position) > Vector3.Distance(towerPos, y.transform.position))
            {
                return 1;
            }
            else if (Vector3.Distance(towerPos, x.transform.position) < Vector3.Distance(towerPos, y.transform.position))
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
        Vector2 towerPos;
        public void InitTowerTransform(Vector2 pos)
        {
            towerPos = pos;
        }
    }
    public class DistanceEnemyAndGoalComparer : IComparer<Collider2D> //? 이동한 거리로 정렬하기 - 멀리이동했을수록 우선순위
    {
        public int Compare(Collider2D x, Collider2D y)
        {
            var _x = x.GetComponent<EnemyMove>();
            var _y = y.GetComponent<EnemyMove>();

            if (_x.survivalTime * _x.move_Speed > _y.survivalTime * _y.move_Speed)
            {
                return -1;
            }
            else if (_x.survivalTime * _x.move_Speed < _y.survivalTime * _y.move_Speed)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        Vector2 towerPos;
        public void InitTowerTransform(Vector2 pos)
        {
            towerPos = pos;
        }
    }





    //? 사실 여기는 오브젝트 풀링을 하면 stage_enemy_Count 가 처음부터 최대숫자로 되어있을꺼기때문에 처음부터 c_Stage_End()를 실행시켜놔도 무방함
    public IEnumerator c_Stage_End(int quantity, float time)
    {
        yield return new WaitForSeconds(quantity * time);
        yield return new WaitUntil(() => stage_enemy_Count <= 0);
        GameManager.Instance.PlayState = Define.PlayMode.TowerBuild;
        Debug.Log("Wave 종료");
    }
    public IEnumerator c_Stage_End(int income, int tile, int life)
    {
        yield return new WaitUntil(() => stage_enemy_Count <= 0);
        Debug.Log($"Wave 종료// income = Coin : {income} // Platform : {tile} // Life : {life}");
        
        var asdf = GameObject.FindObjectOfType<Stage_1>();
        if (Wave + 1 == asdf.targetStage.Count)  //? 더이상 웨이브가 없는경우 (스테이지클리어)
        {
            if (GameManager.Instance.Life > 0)
            {
                var popup = GameManager.m_UI.ShowPopUp<UI_PopUp>("Stage_Clear");
                SoundManager.Instance.PlaySound("UI/StageClear");
                SelectStage.Instance.SavePlayerPrefs();
            }
        }
        else
        {
            if (GameManager.Instance.Life > 0)
            {
                if (WaveOverEventList.Count > 0)
                {
                    float frame = 3.0f / WaveOverEventList.Count;
                    if (frame < 0.75f)
                    {
                        frame = 0.75f;
                    }
                    for (int i = 0; i < WaveOverEventList.Count; i++)
                    {
                        WaveOverEventList[i]();
                        yield return new WaitForSeconds(frame);
                    }
                }
                var popup = GameManager.m_UI.ShowPopUp<UI_PopUp>("Wave_Clear");
                SoundManager.Instance.PlaySound("UI/WaveClear");
                popup.CloseSelf(2.0f);

                yield return new WaitForSeconds(2.0f);

                GameManager.Instance.PlayState = Define.PlayMode.TowerBuild;

                Wave++;

                GameManager.Instance.AddCoin(income);
                GameManager.Instance.AddPlatform(tile);
                GameManager.Instance.AddLife(life);

                GameManager.m_Save.SaveToJson();
            }
        }
        StageManager.Instance.WaveOverCallback();
    }

    public IEnumerator c_GameOver()
    {
        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(0.2f);
        Time.timeScale = 0;

        var popup = GameManager.m_UI.ShowPopUp<UI_PopUp>("Game_Over");
        SoundManager.Instance.PlaySound("UI/GameOver");
    }



    #region CSV LOAD
    //? 스테이지 데이터테이블 로드, 가져오기ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    const string parsing_Wave = "Wave_";
    const string parsing_Income = "Income_";

    public List<Pattern> SearchWave(string[] data, int index, out int income, out int platform, out int life)
    {
        int _income = 0;
        int _platform = 0;
        int _life = 0;


        List<Pattern> pattern_List = new List<Pattern>();
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i].Contains(parsing_Wave + index.ToString("D2"))) //? 숫자를 특정자릿수로 표기하기 / D? = ?자리만큼 앞에 0추가함
            {
                //Debug.Log(data[i]);
                string[] temp = data[i].Split(',');
                //Debug.Log($"0000{temp[0]}");
                //Debug.Log($"1111{temp[1]}");
                //Debug.Log($"2222{temp[2]}");
                //Debug.Log($"3333{temp[3]}");

                pattern_List.Add(new Pattern(temp[2], int.Parse(temp[3]), float.Parse(temp[4])));
            }
            if (data[i].Contains(parsing_Income + index.ToString("D2")))
            {
                string[] temp = data[i].Split(',');


                _income = int.Parse(temp[3]);
                _platform = int.Parse(temp[4]);
                _life = int.Parse(temp[5]);
                //if (string.IsNullOrEmpty(temp[4]))
                //{
                //    Debug.Log("빈칸체크");
                //}
            }
        }
        income = _income;
        platform = _platform;
        life = _life;

        return pattern_List;
    }

    public class Pattern //? 파일에서 받아온정보 = Name,Quantity,Time
    {
        public string names;
        public int quantity;
        public float times;
        public Pattern(string na, int qu, float ti)
        {
            names = na;
            quantity = qu;
            times = ti;
        }
    }
    #endregion
}
