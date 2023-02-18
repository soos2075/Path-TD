using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

public class SaveLoadManager
{
    private string _name = "Temp";
    public string FileName 
    { 
        private get { return "AutoSave" + _name; }
        set { _name = value; }
    }

    private StageSaveData tempData;


    public class StageSaveData
    {
        public tileY[] tile_X; //? 맵정보

        public int sizeX;
        public int sizeY;
        public Vector2Int startPos;
        public Vector2Int goalPos;

        [System.Serializable]
        public class tileY
        {
            public Define.TileType[] tile_Y;
            public tileY(int size)
            {
                tile_Y = new Define.TileType[size];
            }
        }
        public StageSaveData(Define.TileType[,] data, int x, int y)
        {
            tile_X = new tileY[x];
            sizeX = x;
            sizeY = y;

            for (int i = 0; i < x; i++)
            {
                tile_X[i] = new tileY(y);
                for (int j = 0; j < y; j++)
                {
                    tile_X[i].tile_Y[j] = data[i, j];

                    if (data[i, j] == Define.TileType.Start)
                    {
                        startPos = new Vector2Int(i, j);
                    }
                    if (data[i, j] == Define.TileType.Goal)
                    {
                        goalPos = new Vector2Int(i, j);
                    }
                }
            }
        }


        public GameData gameData; //? 게임상황 정보

        [System.Serializable]
        public class GameData
        {
            public int platform;
            public int coin;
            public int life;
            public int wave;
            public int score;

            public GameData(int p, int c, int l, int w, int s)
            {
                platform = p;
                coin = c;
                life = l;
                wave = w;
                score = s;
            }
        }

        public List<TowerData> towerData; //? 타워상황 정보

        [System.Serializable]
        public class TowerData
        {
            public int KeyNumber;
            public bool isActive;
            public int Level;
            public Vector2 Position;
            public int TowerCode;
            //public GameObject origin;
            public TowerData(int num, bool check, int lev, Vector2 pos, int code)
            {
                KeyNumber = num;
                isActive = check;
                Level = lev;
                Position = pos;
                TowerCode = code;

            }
        }
    }


    public bool SaveFileSearch()
    {
        return SaveFileSearch(FileName);
    }
    public bool SaveFileSearch(string searchFileName)
    {
        string searchName;
        FileInfo fileInfo;


#if UNITY_EDITOR
        searchName = $"{Application.dataPath}/{searchFileName}.json";
        fileInfo = new FileInfo(searchName);
        return fileInfo.Exists;
#endif

#if UNITY_ANDROID
        searchName = $"{Application.persistentDataPath}/{searchFileName}.json";
        fileInfo = new FileInfo(searchName);
        return fileInfo.Exists;
#endif
    }


    public void DeleteSaveFile()
    {
        DeleteSaveFile(FileName);
    }

    public void DeleteSaveFile(string targetFile)
    {
        if (SaveFileSearch(targetFile))
        {
#if UNITY_EDITOR
            File.Delete($"{Application.dataPath}/{targetFile}.json");
            Debug.Log(targetFile + " Delete Complete");
#endif
#if UNITY_ANDROID
            File.Delete($"{Application.persistentDataPath}/{targetFile}.json");
#endif
        }
        else
            Debug.Log(targetFile + " 이 존재하지 않습니다.");
    }




    public void SaveToJson()
    {
        StageSaveData saveData = new StageSaveData(GameManager.m_Map.nowMap,
    GameManager.m_Map.Size_X,
    GameManager.m_Map.Size_Y);

        saveData.gameData = new StageSaveData.GameData
            (GameManager.Instance.Platform, GameManager.Instance.Coin, GameManager.Instance.Life, GameManager.m_Enemy.Wave, GameManager.Instance.Score);

        saveData.towerData = new List<StageSaveData.TowerData>();
        for (int i = 0; i < GameManager.m_Tower.key_Number; i++)
        {
            Tower_Stat t;
            GameManager.m_Tower.tower_List.TryGetValue(i, out t);

            if (t.confirm_Check)
            {
                saveData.towerData.Add(new StageSaveData.TowerData(i, t.confirm_Check, t.Level, t.Position, (int)t.Tower_Code));
            }
        }

        string saveText = JsonUtility.ToJson(saveData);
        Debug.Log("Json으로 저장할 데이터 : " + saveText);


        FileStream fileStream;
        byte[] data;
#if UNITY_EDITOR
        //? 파일로 저장하기 방법 1
        fileStream = new FileStream($"{Application.dataPath}/{FileName}.json", FileMode.Create);
        data = Encoding.UTF8.GetBytes(saveText);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
#endif
#if UNITY_ANDROID
        //? 파일로 저장하기 방법 1 - 모바일
        fileStream = new FileStream($"{Application.persistentDataPath}/{FileName}.json", FileMode.Create);
        data = Encoding.UTF8.GetBytes(saveText);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
#endif


        ////? 파일로 저장하기 방법 2
        //File.WriteAllText(Application.dataPath + "/myFile2.json", aaaa);



        tempData = saveData;
    }

    //? dataPath = Asset폴더 /// persistentDataPath = C:\Users\USER\AppData\LocalLow\SeonghyunKim\DefenseGame_alpha
    //? 안드로이드에선 또 달라짐. 하여튼 접근가능한경로는 persistentDataPath를 써야함
    StageSaveData LoadToJson()
    {
        FileStream fileStream;
        byte[] data;

#if UNITY_EDITOR //? 파일 불러오기
        fileStream = new FileStream($"{Application.dataPath}/{FileName}.json", FileMode.Open);
        data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
#endif
#if UNITY_ANDROID //? 모바일
        fileStream = new FileStream($"{Application.persistentDataPath}/{FileName}.json", FileMode.Open);
        data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
#endif

        string jsonData = Encoding.UTF8.GetString(data);

        StageSaveData load_data = JsonUtility.FromJson<StageSaveData>(jsonData);

        return load_data;
    }

    public void StageRestart()
    {
        if (tempData == null)
        {
            tempData = LoadToJson();
        }

        //? 맵 초기화
        GameManager.m_Map.AllMapClear();
        GameManager.m_Map.MakeTileMap(tempData);
        GameManager.m_Map.RenderTileMap(GameManager.m_Map.nowMap);

        //? 게임데이터,웨이브 초기화
        GameManager.Instance.Initialization_DefaultValue(tempData.gameData.life, tempData.gameData.coin, tempData.gameData.platform, tempData.gameData.score);
        GameManager.m_Enemy.Wave = tempData.gameData.wave;

        //? 타워 초기화
        GameManager.Instance.PlayState = Define.PlayMode.SaveLoad;
        Tower_Builder tb = GameManager.Instance.GetComponent<Tower_Builder>();
        for (int i = 0; i < GameManager.m_Tower.key_Number; i++)
        {
            Tower_Stat t;
            GameManager.m_Tower.tower_List.TryGetValue(i, out t);

            tb.RemoveRestart(t, t.Position);
        }
        
        for (int i = 0; i < tempData.towerData.Count; i++)
        {
            tb.BuildTower(tempData.towerData[i].Position, tempData.towerData[i].TowerCode);
            tb.BuildConfirm_Restart(tempData.towerData[i].Level);
        }
        GameManager.Instance.PlayState = Define.PlayMode.TowerBuild;
    }





    public void LoadToStorage () //? 저장된거 불러오기
    {
        if (tempData == null)
        {
            tempData = LoadToJson();
        }

        //? 맵 초기화
        GameManager.m_Map.AllMapClear();
        GameManager.m_Map.MakeTileMap(tempData);
        GameManager.m_Map.RenderTileMap(GameManager.m_Map.nowMap);

        //? 게임데이터,웨이브 초기화
        GameManager.Instance.Initialization_DefaultValue(tempData.gameData.life, tempData.gameData.coin, tempData.gameData.platform, tempData.gameData.score);
        GameManager.m_Enemy.Wave = tempData.gameData.wave;

        
        GameManager.Instance.PlayState = Define.PlayMode.SaveLoad; //? 타워 불러오기 시작


        Tower_Builder tb = GameManager.Instance.GetComponent<Tower_Builder>();
        tb.map = GameManager.m_Map.nowMap;

        for (int i = 0; i < tempData.towerData.Count; i++)
        {
            tb.BuildTower(tempData.towerData[i].Position, tempData.towerData[i].TowerCode);

            tb.BuildConfirm_Restart(tempData.towerData[i].Level);

        }
        

        GameManager.Instance.PlayState = Define.PlayMode.TowerBuild; //? 타워 불러오기 종료
    }
}
