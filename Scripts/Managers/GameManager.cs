using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { Initialize(); return _instance; } }

    private static void Initialize()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<GameManager>();
            if (_instance == null)
            {
                GameObject go = new GameObject { name = "@GameManager" };
                go.AddComponent<GameManager>();
                //DontDestroyOnLoad(go);
                _instance = go.GetComponent<GameManager>();
            }
        }
    }



    private ResourceManager _resource = new ResourceManager();
    private MapManager _map = new MapManager();
    private UIManager _ui = new UIManager();
    private TowerManager _tower = new TowerManager();
    private EnemyManager _enemy = new EnemyManager();
    private DataTableManager _data = new DataTableManager();
    private PoolManager _pool = new PoolManager();
    private SaveLoadManager _save = new SaveLoadManager();
    private InfoDataManager _info = new InfoDataManager();

    public static ResourceManager m_Resource { get { return Instance._resource; } }  
    public static MapManager m_Map { get { return Instance._map; } }  
    public static UIManager m_UI { get { return Instance._ui; } } 
    public static TowerManager m_Tower { get { return Instance._tower; } }
    public static EnemyManager m_Enemy { get { return Instance._enemy; } }
    public static DataTableManager m_Data { get { return Instance._data; } }
    public static PoolManager m_Pool { get { return Instance._pool; } }
    public static SaveLoadManager m_Save { get { return Instance._save; } }
    public static InfoDataManager m_Info { get { return Instance._info; } }



    //? ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ



    public int Tower_Number { get; set; } = -1;
    public bool selected_tower { get; set; }
    public bool selected_floor { get; set; }
    public int StageSelect { get; set; } = 0;

    [SerializeField]
    private Define.PlayMode _playState = new Define.PlayMode();
    public Define.PlayMode PlayState { get { return _playState; } set { _playState = value; } }






    private void Awake()
    {
        //? 이거 삭제하면 이 씬에서만 쓰는 싱글톤으로 쓸 수 있음. 지금 게임매니저 오브젝트안에 다른 스크립트들도 많이붙어있어서 얘를 삭제할순없으니 이렇게 하는게 맞는듯
        //DontDestroyOnLoad(this);
    }
    private void Start()
    {
        //SoundManager.Instance.PlaySound("BGM/SMP1_THEME_Voyager", Define.AudioType.BGM);

        _instance._info.init();
    }

    //public float gameSpeed;
    //private void Update()
    //{
    //    gameSpeed = Time.timeScale;
    //}

    public int Life { get; private set; }
    public void AddLife(int value = 1)
    {
        Life += value;
    }
    public void SubtractLife(int value = 1)
    {
        Life -= value;
        if (Life <= 0)
        {
            StartCoroutine(GameManager.m_Enemy.c_GameOver());
        }
    }


    public delegate void AddCoinDelegate(int coin, bool AddorSub);
    public event AddCoinDelegate Event_AddCoin;


    public int Coin { get; private set; } //? capital, money, cash, credit, metal, 등등 여러개있긴한데 일단은 구현용
    public bool SubtractCoin (int value)
    {
        if (Coin >= value)
        {
            Coin -= value;
            Event_AddCoin(value, false);
            return true;
        }
        else
        {
            SoundManager.Instance.PlaySound("UI/WrongClick");
            Debug.Log("돈이 부족합니다.");
            return false;
        }
    }
    public void AddCoin (int value)
    {
        Coin += value;
        Event_AddCoin(value, true);
    }

    
    public int Platform { get; private set; }
    public void AddPlatform (int value = 1)
    {
        Platform += value;
    }
    public bool SubtractPlatform (int value = 1)
    {
        if (Platform == 0)
        {
            return false;
        }
        Platform -= value;
        return true;
    }

    public int Score { get; private set; }
    public void AddScore(int value)
    {
        Score += value;
    }


    public int SupplyPower { get; private set; } //? 인구수 또는 전력, 구현예정


    public void Initialization_DefaultValue(int l, int c, int p, int s)
    {
        Life = l;
        Coin = c;
        Platform = p;
        Score = s;
    }


    

}
