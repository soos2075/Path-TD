using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_1 : MonoBehaviour
{
    public MapData_SO[] targetMapData;

    public List<WaveBase> targetStage;

    public string targetCsvData;
    private string stageName;


    private void Awake()
    {
        LoadMap(SelectStage.Instance.Stage);
    }
    void Start()
    {
        StageLoadFromManager();
    }
    void Update()
    {

    }

    public void StageLoadFromManager ()
    {
        if (SelectStage.Instance.Stage > 0)
        {
            stageName = "Stage" + SelectStage.Instance.Stage.ToString();
            targetStage = Load_StageData(stageName);
            GameManager.m_Save.FileName = stageName;
            //AutoSave();
        }
        else 
        {
            Load_StageCSV();
        }

        if (GameManager.m_Save.SaveFileSearch()) //? 플레이하던 중간데이터가 존재한다면
        {
            //Debug.Log("플레이하던 데이터가 존재합니다. 불러올까요?");
            SoundManager.Instance.PlaySound("UI/UIPopup");
            GameManager.m_UI.ShowPopUp<UI_Load>("LoadPopUp");
            //LoadAutoSave();
        }
        else if (SelectStage.Instance.Stage == 1)
        {
            GameManager.m_UI.ShowPopUp<UI_PopUp>("Tutorial_Detail", true);
        }
    }



    public void CoinCopyBug_100()
    {
        GameManager.Instance.AddCoin(100);
    }
    public void CoinCopyBug ()
    {
        GameManager.Instance.AddCoin(1000);
        GameManager.Instance.AddPlatform(10);
    }

    public void PausePopup ()
    {
        Time.timeScale = 0;
        GameManager.m_UI.ShowPopUp<UI_Pause>("PausePopUp");
        SoundManager.Instance.PlaySound("UI/Click");
    }


    public void Load_StageCSV()
    {
        if (targetStage != null)
        {
            Debug.Log("이미 스테이지가 존재함");
            return;
        }
        stageName = targetCsvData;
        targetStage = Load_StageData(targetCsvData);

        //GameManager.m_Save.FileName = targetCsvData;
        //AutoSave();
    }

    public void AutoSave() //? 스테이지 클리어시 자동저장
    {
        GameManager.m_Save.SaveToJson();
    }

    public void Stage_Restart()
    {
        if (string.IsNullOrEmpty(stageName))
        {
            Debug.Log("스테이지데이터없음");
            return;
        }

        if (!GameManager.m_Save.SaveFileSearch())
        {
            SceneChange.Instance.Change("#3_Stage");
            SoundManager.Instance.PlaySound("UI/Click");
            return;
        }

        GameManager.Instance.StopAllCoroutines();
        StopAllCoroutines();
        GameManager.m_Enemy.AllEnemyClear();
        GameManager.Instance.PlayState = Define.PlayMode.TowerBuild;

        GameManager.Instance.GetComponent<Tower_Builder>().selected_Tower = null;
        GameManager.m_UI.CloseAll();

        GameManager.m_Enemy.WaveOverEventList.Clear();
        //for (int i = 0; i < GameManager.m_Enemy.WaveOverEventList.Count; i++)
        //{
        //    GameManager.m_Enemy.E_WaveOver -= GameManager.m_Enemy.WaveOverEventList[i];
        //}
        //FindObjectOfType<Tower_Builder>().BuildCancle();
        GameManager.Instance.GetComponent<EditTool>().Edit_Reset();

        GameManager.m_Save.StageRestart();

        targetStage = Load_StageData(stageName);

        GameManager.Instance.GetComponent<Navi>().Init_Navi();

        GameManager.Instance.AddCoin(0);
    }

    public void LoadAutoSave()
    {
        //stageName = "AutoSaveTemp";  //? @@@@@@@@@@@@@


        GameManager.Instance.StopAllCoroutines();
        StopAllCoroutines();
        GameManager.m_Enemy.AllEnemyClear();
        GameManager.Instance.PlayState = Define.PlayMode.TowerBuild;

        GameManager.Instance.GetComponent<Tower_Builder>().selected_Tower = null;
        GameManager.m_UI.CloseAll();

        GameManager.m_Save.LoadToStorage();

        targetStage = Load_StageData(stageName);

        //GameManager.Instance.GetComponent<Navi>().SearchMap(true);
        GameManager.Instance.GetComponent<Navi>().Init_Navi();

        GameManager.Instance.AddCoin(0);
    }


    public void Stage_Reset()
    {
        GameManager.m_Save.DeleteSaveFile();
    }



    void LoadMap (int stage) //? 맵 로드
    {
        GameManager.m_Map.MakeTileMap(targetMapData[stage]);
        GameManager.m_Map.RenderTileMap(GameManager.m_Map.nowMap);
    }


    DataTableManager.CSV_Data data;
    List<WaveBase> Load_StageData (string fileName) //? 스테이지 데이터 로드
    {
        List<WaveBase> stateData = new List<WaveBase>();
        if (data == null)
        {
            data = GameManager.m_Data.CSV_LOAD_Stage(fileName);
        }
        for (int i = 0; i < data.Counter; i++)
        {
            int income;
            int platform;
            int life;

            var loadData = GameManager.m_Enemy.SearchWave(data.Line, (i + 1), out income, out platform, out life);
            stateData.Add(new WaveBase(loadData, income, platform, life));
        }
        return stateData;
    }

    //List<WaveBase> Load_StageData(string fileName, int waveNum) //? 스테이지 데이터 로드 (중간부터)
    //{
    //    List<WaveBase> stateData = new List<WaveBase>();
    //    for (int i = waveNum; i < data.Counter; i++)
    //    {
    //        int income;
    //        int platform;
    //        int life;

    //        var loadData = GameManager.m_Enemy.SearchWave(data.Line, (i + 1), out income, out platform, out life);
    //        stateData.Add(new WaveBase(loadData, income, platform, life));
    //    }
    //    return stateData;
    //}



    public void WaveStartButton ()
    {
        if (GameManager.Instance.PlayState == Define.PlayMode.MapEdit)
        {
            Debug.Log("Edit모드가 종료되지않음");
            SoundManager.Instance.PlaySound("UI/WrongClick");
            return;
        }

        if (targetStage == null)
        {
            Debug.Log("스테이지정보가없음");
            return;
        }
        if (targetStage.Count <= GameManager.m_Enemy.Wave)
        {
            Debug.Log("스테이지끝");
            return;
        }

        if (targetStage[GameManager.m_Enemy.Wave].runtimeCoroutine == null)
        {
            targetStage[GameManager.m_Enemy.Wave].WaveStart();
            SoundManager.Instance.PlaySound("UI/Start");
            Debug.Log("Wave 시작");
            StartCoroutine(targetStage[GameManager.m_Enemy.Wave].WaveCoroutine());
            GameManager.Instance.PlayState = Define.PlayMode.Play;

            //StartCoroutine(GameManager.Instance.GetComponent<Navi>().LineDisable());

            StageManager.Instance.WaveStartCallback();
        }
        else
            Debug.Log("이미 코루틴실행중");
    }


    public class WaveBase //? 웨이브 정보담는 클래스
    {
        public int PatternCount { get; set; } //? 웨이브 패턴개수 확인용
        //private int count;
        public int Income { get; set; } //? 웨이브끝나면 코인수급
        public int Platform { get; set; } //? 웨이브끝나고 추가될 타일
        public int Life { get; set; } //? 웨이브끝나고 라이프추가인데 당장은 쓸일없을듯?


        Queue<SingleWave> enemyCreate_Q; //? 실제 웨이브가 시작되면 내부에서 꺼내서 쓰는 용도
        public class SingleWave
        {
            public string Name { get; set; }
            public int Quantity { get; set; }
            public float CycleTime { get; set; }

            public SingleWave (string name, int quan, float cycle)
            {
                Name = name;
                Quantity = quan;
                CycleTime = cycle;
            }
        }

        public void AddWave (string name, int quantity, float cycle)
        {
            enemyCreate_Q.Enqueue(new SingleWave(name, quantity, cycle));

            monsterList.Add(new SingleWave(name, quantity, cycle));
        }

        public List<SingleWave> monsterList; //? 씬에서 웨이브 정보를 알기위해 밖에서 가져가는 용도



        public WaveBase (List<EnemyManager.Pattern> data, int income, int tile, int life)
        {
            Income = income;
            Platform = tile;
            Life = life;

            PatternCount = data.Count;
            enemyCreate_Q = new Queue<SingleWave>();
            monsterList = new List<SingleWave>();

            for (int i = 0; i < PatternCount; i++)
            {
                AddWave(data[i].names, data[i].quantity, data[i].times);
            }
        }

        public List<IEnumerator> runtimeCoroutine;
        public IEnumerator WaveCoroutine()
        {
            for (int i = 0; i < runtimeCoroutine.Count; i++)
            {
                //Debug.Log("대기중");
                yield return GameManager.Instance.StartCoroutine(runtimeCoroutine[i]);
            }
            GameManager.Instance.StartCoroutine(GameManager.m_Enemy.c_Stage_End(Income, Platform, Life));
            //StageManager.Instance.WaveOverCallback();
        }

        public void WaveStart ()
        {
            runtimeCoroutine = new List<IEnumerator>();
            for (int i = 0; i < PatternCount; i++)
            {
                SingleWave pattern = enemyCreate_Q.Dequeue();
                runtimeCoroutine.Add(GameManager.m_Enemy.Instant_Wave(pattern.Quantity, pattern.CycleTime, pattern.Name));
            }
        }
    }


    //List<Action> Wave_List = new List<Action>();
    //int Wave;
    //public void Wave_Start() //? 밖에서 웨이브 시작 버튼 누르면 호출되는 함수
    //{
    //    Wave_List[Wave].Invoke();
    //    Wave++;
    //    StartCoroutine(FindObjectOfType<Navi>().LineDisable());
    //}
    //void Wave_Add()
    //{
    //    Wave_List.Add(() => Wave_1());
    //    Wave_List.Add(() => Wave_2());
    //    Wave_List.Add(() => Wave_3());
    //}
    //void Wave_1()
    //{
    //    //Debug.Log("//대충 5마리 소환");
    //    Debug.Log("Wave 시작");
    //    GameManager.Instance.PlayState = Define.PlayMode.Play;
    //    StartCoroutine(GameManager.m_Enemy.Instant_Wave(5, 1f, "Enemy_A"));
    //    StartCoroutine(GameManager.m_Enemy.c_Stage_End());
    //}
    //void Wave_2()
    //{
    //    Debug.Log("//대충 10마리 소환");
    //}
    //void Wave_3()
    //{
    //    Debug.Log("//대충 5마리 소환했다가 기다렸다가 5마리 더 소환");
    //}
}
