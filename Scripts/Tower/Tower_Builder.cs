using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tower_Builder : MonoBehaviour
{
    Vector3 mousePos;
    RaycastHit2D myRay;
    public Define.TileType[,] map;
    GameObject selected;
    Vector2 selectedPos;
    GameObject mark; //? 지금 역할이 타워 건설가능지역을 선택했을때만 나타나는 UI라서 용도를 다르게 쓰고 그냥 클릭 mark는 따로해야할듯

    [SerializeField] public Tower_Stat selected_Tower;
    [SerializeField] public Tower_Stat temp_Tower;

    //public bool Selected_Tower { get { if (selected_Tower != null) { return true; } else return false; } }

    public List<TowerManager.Tower_Status> tw_Status;
    public Dictionary<string, TowerManager.Tower_Info> tw_Info;

    List<Action> BuildAction;


    int touchCount = 0; //? 땅 두번 터치하면 초기화하는거

    private void Awake()
    {
        if (LanguageManager.Instance.Language == Define.Language.Kor)
        {
            var csv_info = GameManager.m_Data.CSV_LOAD_Tower_Info("Tower_Info");
            tw_Info = GameManager.m_Tower.GetTower_Info(csv_info.Line, csv_info.Counter);
        }
        else
        {
            var csv_info = GameManager.m_Data.CSV_LOAD_Tower_Info("Tower_Info_Eng");
            tw_Info = GameManager.m_Tower.GetTower_Info(csv_info.Line, csv_info.Counter);
        }
        
        
        var csv_status = GameManager.m_Data.CSV_LOAD_Tower_Status("Tower_Status");
        tw_Status = GameManager.m_Tower.GetTower_Status(csv_status.Line, csv_status.Counter);

    }
    void Start()
    {
        BuildAction = AddTowerAction();
        mark = GameManager.m_Resource.Instant_Prefab("Tile/Tile_Select");
        mark.SetActive(false);

        BuildMenu();
    }

    Vector3 startInputPos;
    Vector3 endInputPos;
    int nowFingerID;

    void Update()
    {
        if (GameManager.Instance.PlayState == Define.PlayMode.MapEdit)
        {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            touchCount = 0;
            return;
        }


#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            startInputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            endInputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Vector3.Distance(startInputPos, endInputPos) < 0.75f)
        {
            Debug.Log("타일터치" + Vector3.Distance(startInputPos, endInputPos));
            startInputPos = Vector3.one;
            endInputPos = Vector3.zero;


            if (temp_Tower != null)
            {
                Destroy(temp_Tower.gameObject);
                temp_Tower = null;
            }

            mark.SetActive(false);
            GameManager.Instance.selected_floor = false;
            map = GameManager.m_Map.nowMap;
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            myRay = Physics2D.Raycast(mousePos, Vector3.forward, 10, LayerMask.GetMask("Tower", "Tile"));


            if (myRay.collider != null) //? 타일선택마크
            {
                //Debug.Log(myRay.collider.gameObject);
                //Debug.Log(myRay.collider.gameObject.layer);

                selected = myRay.collider.gameObject;
                selectedPos = new Vector2(selected.transform.position.x, selected.transform.position.y);
                mark.transform.position = selectedPos;
                mark.SetActive(true);
            }


            //? ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ아래는 타워용 raycast
            RaycastHit2D myRay2 = Physics2D.Raycast(mousePos, Vector3.forward, 10, LayerMask.GetMask("Tower"));
            if (selected_Tower != null)
            {
                selected_Tower.InvisibleRange();
                selected_Tower = null;
            }
            if (myRay2.collider != null)
            {
                //Debug.Log("읽긴읽음");
                GameManager.Instance.selected_tower = true;
                SelectTower(myRay2.collider.GetComponent<Tower_Stat>().Dic_KeyNumber);
                return;
            }


            //? ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

            RaycastHit2D myRay3 = Physics2D.Raycast(mousePos, Vector3.forward, 10, LayerMask.GetMask("Tile"));
            if (myRay3.collider != null)
            {
                selected = myRay3.collider.gameObject;
                selectedPos = new Vector2(selected.transform.position.x, selected.transform.position.y);

                //Debug.Log($"Pos : {selectedPos}, Tile : {map[(int)selectedPos.x, (int)selectedPos.y]}");

                if (map[(int)selectedPos.x, (int)selectedPos.y] == Define.TileType.Start)
                {
                    FindObjectOfType<UI_Wave>().ShowWaveInfo();
                }

                if (map[(int)selectedPos.x, (int)selectedPos.y] == Define.TileType.Floor || map[(int)selectedPos.x, (int)selectedPos.y] == Define.TileType.Changeable_Floor)
                {
                    touchCount = 0;
                    GameManager.Instance.selected_floor = true;
                    GameManager.Instance.selected_tower = false;
                    if (GameManager.Instance.Tower_Number != -1)
                    {
                        BuildTower();
                    }
                    return;
                }
                else
                {
                    touchCount++;

                    if (touchCount == 2)
                    {
                        touchCount = 0;
                        GameManager.Instance.Tower_Number = -1;
                        GameManager.Instance.selected_tower = false;
                        GameObject temp = GameObject.Find("Select"); //? 타워 선택된 UI 없애기
                        if (temp)
                        {
                            GameManager.m_Resource.Disable_Prefab(temp);
                        }
                        FindObjectOfType<UI_TowerPanel>().OFF_Button();
                    }
                    else if (GameManager.Instance.Tower_Number == -1)
                    {
                        GameManager.Instance.selected_tower = false;
                        FindObjectOfType<UI_TowerPanel>().OFF_Button();
                    }
                }
            }
        }
#endif




#if UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            nowFingerID = Input.GetTouch(0).fingerId;
            startInputPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && Input.GetTouch(0).fingerId == nowFingerID)
        {
            endInputPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        }

        if (Vector3.Distance(startInputPos, endInputPos) < 0.75f)
        {
            //Debug.Log("모바일터치" + Vector3.Distance(startInputPos, endInputPos));
            startInputPos = Vector3.one;
            endInputPos = Vector3.zero;
            nowFingerID = -1;

            if (temp_Tower != null)
            {
                Destroy(temp_Tower.gameObject);
                temp_Tower = null;
            }

            mark.SetActive(false);
            GameManager.Instance.selected_floor = false;
            map = GameManager.m_Map.nowMap;
            mousePos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            myRay = Physics2D.Raycast(mousePos, Vector3.forward, 10, LayerMask.GetMask("Tower", "Tile"));


            if (myRay.collider != null) //? 타일선택마크
            {
                //Debug.Log(myRay.collider.gameObject);
                //Debug.Log(myRay.collider.gameObject.layer);

                selected = myRay.collider.gameObject;
                selectedPos = new Vector2(selected.transform.position.x, selected.transform.position.y);
                mark.transform.position = selectedPos;
                mark.SetActive(true);
            }


            //? ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ아래는 타워용 raycast
            RaycastHit2D myRay2 = Physics2D.Raycast(mousePos, Vector3.forward, 10, LayerMask.GetMask("Tower"));
            if (selected_Tower != null)
            {
                selected_Tower.InvisibleRange();
                selected_Tower = null;
            }
            if (myRay2.collider != null)
            {
                //Debug.Log("읽긴읽음");
                GameManager.Instance.selected_tower = true;
                SelectTower(myRay2.collider.GetComponent<Tower_Stat>().Dic_KeyNumber);
                return;
            }


            //? ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

            RaycastHit2D myRay3 = Physics2D.Raycast(mousePos, Vector3.forward, 10, LayerMask.GetMask("Tile"));
            if (myRay3.collider != null)
            {
                selected = myRay3.collider.gameObject;
                selectedPos = new Vector2(selected.transform.position.x, selected.transform.position.y);

                //Debug.Log($"Pos : {selectedPos}, Tile : {map[(int)selectedPos.x, (int)selectedPos.y]}");

                if (map[(int)selectedPos.x, (int)selectedPos.y] == Define.TileType.Start)
                {
                    FindObjectOfType<UI_Wave>().ShowWaveInfo();
                }

                if (map[(int)selectedPos.x, (int)selectedPos.y] == Define.TileType.Floor || map[(int)selectedPos.x, (int)selectedPos.y] == Define.TileType.Changeable_Floor)
                {
                    touchCount = 0;
                    GameManager.Instance.selected_floor = true;
                    GameManager.Instance.selected_tower = false;
                    if (GameManager.Instance.Tower_Number != -1)
                    {
                        BuildTower();
                    }
                    return;
                }
                else
                {
                    touchCount++;

                    if (touchCount == 2)
                    {
                        touchCount = 0;
                        GameManager.Instance.Tower_Number = -1;
                        GameManager.Instance.selected_tower = false;
                        GameObject temp = GameObject.Find("Select"); //? 타워 선택된 UI 없애기
                        if (temp)
                        {
                            GameManager.m_Resource.Disable_Prefab(temp);
                        }
                        FindObjectOfType<UI_TowerPanel>().OFF_Button();
                    }
                    else if (GameManager.Instance.Tower_Number == -1)
                    {
                        GameManager.Instance.selected_tower = false;
                        FindObjectOfType<UI_TowerPanel>().OFF_Button();
                    }
                }
            }
        }
#endif

    }


    public void DoEditMode ()
    {
        mark.SetActive(false);
        if (temp_Tower != null)
        {
            Destroy(temp_Tower.gameObject);
        }
        GameManager.Instance.selected_floor = false;

        touchCount = 0;
        if (selected_Tower != null)
        {
            selected_Tower.InvisibleRange();
            selected_Tower = null;
        }
        GameManager.Instance.Tower_Number = -1;
        GameManager.Instance.selected_tower = false;
        GameObject temp = GameObject.Find("Select"); //? 타워 선택된 UI 없애기
        if (temp)
        {
            GameManager.m_Resource.Disable_Prefab(temp);
        }
        FindObjectOfType<UI_TowerPanel>().OFF_Button();
    }


    UI_BuildMenu buildMenu;

    void BuildMenu ()
    {
        buildMenu = GameManager.m_UI.ShowSceneUI<UI_BuildMenu>("BuildMenu_2");

        buildMenu.AddEvent_Info((PointerEventData data) => { ShowInfo(); });
        buildMenu.AddEvent_Sell((PointerEventData data) => RemoveTower());
        buildMenu.AddEvent_Build((PointerEventData data) => BuildConfirm());
        buildMenu.AddEvent_Upgrade((PointerEventData data) => Upgrade());
    }



    void ShowInfo()
    {
        SoundManager.Instance.PlaySound("UI/Info");
        if (selected_Tower != null)
        {
            GameManager.m_UI.ShowPopUp<UI_Tower_Info>("Tower_Info").SetTowetData
                (tw_Info[Define.TowerName[(int)selected_Tower.Tower_Code]].Info, 
                GameManager.m_Info.ShowTowerSprite(Define.TowerName[(int)selected_Tower.Tower_Code]));

            //$"\n Cost : {tw_Info[Define.TowerName[selected_Tower.CSV_Number]].Cost}"); ;


            if (selected_Tower.Level >= 5)
            {
                var a = GameManager.m_UI.ShowPopUp<UI_Active_Info>("Active_Info");

                a.Set_LevelImage(5);

                //a.Set_ActiveInfo
                //($"현재 레벨 : {selected_Tower.Level} \n " +
                //$"Max Level");
            }
            else
            {
                var a = GameManager.m_UI.ShowPopUp<UI_Active_Info>("Active_Info");

                a.Set_LevelImage(selected_Tower.Level);

                //a.Set_ActiveInfo
                // //($"현재 레벨(이건 추후 이미지로 표시) : {selected_Tower.Level} \n " +
                // ($"Next Level : {tw_Info[Define.TowerName[selected_Tower.CSV_Number]].Upgrade[selected_Tower.Level]} \n" +
                // $"판매 가격 : {selected_Tower.AccumCoin * 0.8f}");
            }
        }
        else if (selected_Tower == null && GameManager.Instance.Tower_Number > -1)
        {
            //GameManager.m_UI.ShowPopUp<UI_Tower_Info>("Tower_Info").Set_TowerInfo
            //    ($"{tw_Info[Define.TowerName[GameManager.Instance.Tower_Number]].Name} : {tw_Info[Define.TowerName[GameManager.Instance.Tower_Number]].Info}");
            ////+ $"\n건설 Cost : {tw_Info[Define.TowerName[GameManager.Instance.Tower_Number]].Cost}");

            //Debug.Log(Define.TowerName[GameManager.Instance.Tower_Number] + "@@@@");

            GameManager.m_UI.ShowPopUp<UI_Tower_Info>("Tower_Info").SetTowetData
                (tw_Info[Define.TowerName[GameManager.Instance.Tower_Number]].Info,
                GameManager.m_Info.ShowTowerSprite(Define.TowerName[GameManager.Instance.Tower_Number]));
        }
    }


    void SelectTower (int key_Number)
    {
        GameManager.m_Tower.tower_List.TryGetValue(key_Number, out selected_Tower);
        selected_Tower.VisibleRange();
    }

    public bool TempTowerIsOn()
    {
        if (temp_Tower != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    #region TowerBuildAction
    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    public void BuildTower()
    {
        if (GameManager.Instance.Tower_Number != -1)
        {
            if (temp_Tower != null)
            {
                Destroy(temp_Tower.gameObject);
            }
            //Debug.Log("아마여기?");
            BuildAction[GameManager.Instance.Tower_Number].Invoke();
            temp_Tower.PosSetting();
        }
    }
    public void BuildTower(Vector2 pos, int num)
    {
        selectedPos = pos;
        BuildAction[num].Invoke();
        temp_Tower.PosSetting();
    }


    private List<Action> AddTowerAction()
    {
        List<Action> act_list = new List<Action>();

        act_list.Add(() => FireTower());
        act_list.Add(() => LazerTower());
        act_list.Add(() => SplashTower());
        act_list.Add(() => MultiTower());
        act_list.Add(() => PoisonTower());
        act_list.Add(() => SpiderTower());
        act_list.Add(() => SlowTower());
        act_list.Add(() => ObserverTower());
        act_list.Add(() => SpearTower());
        act_list.Add(() => AirTower());
        act_list.Add(() => ThunderTower());
        act_list.Add(() => ArcaneTower());
        act_list.Add(() => WaterTower());
        act_list.Add(() => LightningTower());
        act_list.Add(() => CoinTower());

        return act_list;
    }


    int Get_ID (string name)
    {
        TowerManager.Tower_Info k;
        tw_Info.TryGetValue(name, out k);
        return k.ID;
    }

    private void FireTower()
    {
        int N = Get_ID("FireTower");

        temp_Tower = Util.GetOrAddComponent<Tower_Stat>(GameManager.m_Tower.Instant_Tower
            (new Tower_Stat.FireTower(tw_Status[N].AttackSpeed, tw_Status[N].AttackRange, tw_Status[N].Attackable, tw_Status[N].Bullet_Type, tw_Status[N].Damage, tw_Status[N].Detecting), 
            "FireTower", selectedPos));
    }
    private void LazerTower()
    {
        int N = Get_ID("LazerTower");

        temp_Tower = Util.GetOrAddComponent<Tower_Stat>(GameManager.m_Tower.Instant_Tower
            (new Tower_Stat.LazerTower(tw_Status[N].AttackSpeed, tw_Status[N].AttackRange, tw_Status[N].Attackable, tw_Status[N].Bullet_Type, tw_Status[N].Damage, tw_Status[N].Detecting), 
            "LazerTower", selectedPos));
    }

    private void SplashTower()
    {
        int N = Get_ID("SplashTower");

        temp_Tower = Util.GetOrAddComponent<Tower_Stat>(GameManager.m_Tower.Instant_Tower
            (new Tower_Stat.SplashTower(tw_Status[N].AttackSpeed, tw_Status[N].AttackRange, tw_Status[N].Attackable, tw_Status[N].Bullet_Type, tw_Status[N].Damage, tw_Status[N].Detecting), 
            "SplashTower", selectedPos));

        (temp_Tower.myTower as Tower_Stat.I_Detail).Detail_Data(tw_Status[N].Detail_Data[0]);
    }
    private void MultiTower()
    {
        int N = Get_ID("MultiTower");

        temp_Tower = Util.GetOrAddComponent<Tower_Stat>(GameManager.m_Tower.Instant_Tower
            (new Tower_Stat.MultishotTower(tw_Status[N].AttackSpeed, tw_Status[N].AttackRange, tw_Status[N].Attackable, tw_Status[N].Bullet_Type, tw_Status[N].Damage, tw_Status[N].Detecting), 
            "MultiTower", selectedPos));

        (temp_Tower.myTower as Tower_Stat.I_Detail).Detail_Data(tw_Status[N].Detail_Data[0]);
    }
    private void PoisonTower()
    {
        int N = Get_ID("PoisonTower");

        temp_Tower = Util.GetOrAddComponent<Tower_Stat>(GameManager.m_Tower.Instant_Tower
            (new Tower_Stat.PoisonTower(tw_Status[N].AttackSpeed, tw_Status[N].AttackRange, tw_Status[N].Attackable, tw_Status[N].Bullet_Type, tw_Status[N].Damage, tw_Status[N].Detecting), 
            "PoisonTower", selectedPos));

        (temp_Tower.myTower as Tower_Stat.I_Detail).Detail_Data(tw_Status[N].Detail_Data[0], tw_Status[N].Detail_Data[1]);
    }
    private void SpiderTower()
    {
        int N = Get_ID("SpiderTower");

        temp_Tower = Util.GetOrAddComponent<Tower_Stat>(GameManager.m_Tower.Instant_Tower
            (new Tower_Stat.SpiderTower(tw_Status[N].AttackSpeed, tw_Status[N].AttackRange, tw_Status[N].Attackable, tw_Status[N].Bullet_Type, tw_Status[N].Damage, tw_Status[N].Detecting), 
            "SpiderTower", selectedPos));

        (temp_Tower.myTower as Tower_Stat.I_Detail).Detail_Data(tw_Status[N].Detail_Data[0]);
    }
    private void SlowTower()
    {
        int N = Get_ID("SlowTower");

        temp_Tower = Util.GetOrAddComponent<Tower_Stat>(GameManager.m_Tower.Instant_Tower
        (new Tower_Stat.SlowTower(tw_Status[N].AttackSpeed, tw_Status[N].AttackRange, tw_Status[N].Attackable, tw_Status[N].Bullet_Type, tw_Status[N].Detecting), 
        "SlowTower", selectedPos));

        (temp_Tower.myTower as Tower_Stat.I_Detail).Detail_Data(tw_Status[N].Detail_Data[0], tw_Status[N].Detail_Data[1]);
    }
    private void ObserverTower()
    {
        int N = Get_ID("BuffTower");

        temp_Tower = Util.GetOrAddComponent<Tower_Stat>(GameManager.m_Tower.Instant_Tower
        (new Tower_Stat.ObserverTower(tw_Status[N].AttackSpeed, tw_Status[N].AttackRange, tw_Status[N].Attackable, tw_Status[N].Bullet_Type, tw_Status[N].Detecting), 
        "BuffTower", selectedPos));

        (temp_Tower.myTower as Tower_Stat.I_Detail).Detail_Data(tw_Status[N].Detail_Data[0], tw_Status[N].Detail_Data[1]);
    }
    private void SpearTower()
    {
        int N = Get_ID("SpearTower");

        temp_Tower = Util.GetOrAddComponent<Tower_Stat>(GameManager.m_Tower.Instant_Tower
            (new Tower_Stat.SpearTower(tw_Status[N].AttackSpeed, tw_Status[N].AttackRange, tw_Status[N].Attackable, tw_Status[N].Bullet_Type, tw_Status[N].Damage, tw_Status[N].Detecting), 
            "SpearTower", selectedPos));
    }
    private void AirTower()
    {
        int N = Get_ID("AirTower");

        temp_Tower = Util.GetOrAddComponent<Tower_Stat>(GameManager.m_Tower.Instant_Tower
            (new Tower_Stat.AirTower(tw_Status[N].AttackSpeed, tw_Status[N].AttackRange, tw_Status[N].Attackable, tw_Status[N].Bullet_Type, tw_Status[N].Damage, tw_Status[N].Detecting), 
            "AirTower", selectedPos));
    }
    private void ThunderTower()
    {
        int N = Get_ID("ThunderTower");

        temp_Tower = Util.GetOrAddComponent<Tower_Stat>(GameManager.m_Tower.Instant_Tower
            (new Tower_Stat.ThunderTower(tw_Status[N].AttackSpeed, tw_Status[N].AttackRange, tw_Status[N].Attackable, tw_Status[N].Bullet_Type, tw_Status[N].Damage, tw_Status[N].Detecting),
            "ThunderTower", selectedPos));
    }
    private void ArcaneTower()
    {
        int N = Get_ID("ArcaneTower");

        temp_Tower = Util.GetOrAddComponent<Tower_Stat>(GameManager.m_Tower.Instant_Tower
            (new Tower_Stat.ArcaneTower(tw_Status[N].AttackSpeed, tw_Status[N].AttackRange, tw_Status[N].Attackable, tw_Status[N].Bullet_Type, tw_Status[N].Damage, tw_Status[N].Detecting),
            "ArcaneTower", selectedPos));

        (temp_Tower.myTower as Tower_Stat.I_Detail).Detail_Data(tw_Status[N].Detail_Data[0]);
    }
    private void WaterTower()
    {
        int N = Get_ID("WaterTower");

        temp_Tower = Util.GetOrAddComponent<Tower_Stat>(GameManager.m_Tower.Instant_Tower
            (new Tower_Stat.WaterTower(tw_Status[N].AttackSpeed, tw_Status[N].AttackRange, tw_Status[N].Attackable, tw_Status[N].Bullet_Type, tw_Status[N].Damage, tw_Status[N].Detecting),
            "WaterTower", selectedPos));

        (temp_Tower.myTower as Tower_Stat.I_Detail).Detail_Data(tw_Status[N].Detail_Data[0]);
    }
    private void LightningTower()
    {
        int N = Get_ID("LightningTower");

        temp_Tower = Util.GetOrAddComponent<Tower_Stat>(GameManager.m_Tower.Instant_Tower
            (new Tower_Stat.LightningTower(tw_Status[N].AttackSpeed, tw_Status[N].AttackRange, tw_Status[N].Attackable, tw_Status[N].Bullet_Type, tw_Status[N].Damage, tw_Status[N].Detecting),
            "LightningTower", selectedPos));

        (temp_Tower.myTower as Tower_Stat.I_Detail).Detail_Data(tw_Status[N].Detail_Data[0]);
    }
    private void CoinTower()
    {
        int N = Get_ID("CoinTower");

        temp_Tower = Util.GetOrAddComponent<Tower_Stat>(GameManager.m_Tower.Instant_Tower
            (new Tower_Stat.CoinTower(tw_Status[N].AttackSpeed, tw_Status[N].AttackRange, tw_Status[N].Attackable, tw_Status[N].Bullet_Type, tw_Status[N].Damage, tw_Status[N].Detecting),
            "CoinTower", selectedPos));

        (temp_Tower.myTower as Tower_Stat.I_Detail).Detail_Data(tw_Status[N].Detail_Data[0]);
    }
    #endregion



    //? ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    public void BuildCancle()
    {
        if (temp_Tower != null)
        {
            Destroy(temp_Tower.gameObject);
            temp_Tower = null;
        }
    }


    private bool PreConfirm() //? 비용체크, 맵수정, 타일수정
    {
        if (temp_Tower == null)
        {
            return false;
        }

        if (GameManager.Instance.PlayState == Define.PlayMode.SaveLoad) //? 불러오기모드일때는 Cost체크를 패싱
        {
            //? 아래 비용체크 안하고 그냥 지나감.
        }
        else if (!GameManager.Instance.SubtractCoin(tw_Info[Define.TowerName[GameManager.Instance.Tower_Number]].Cost)) //? 타워짓는비용 있나 체크
        {
            return false;
        }

        if (map[(int)selectedPos.x, (int)selectedPos.y] == Define.TileType.Floor)
        {
            map[(int)selectedPos.x, (int)selectedPos.y] = Define.TileType.Tower;
        }
        else if (map[(int)selectedPos.x, (int)selectedPos.y] == Define.TileType.Changeable_Floor)
        {
            map[(int)selectedPos.x, (int)selectedPos.y] = Define.TileType.Changeable_Tower;
        }
        GameManager.m_Map.nowMap = map;
        GameManager.Instance.selected_floor = false;
        //GameManager.Instance.selected_tower = false;
        mark.SetActive(false);

        return true;
    }
    public void BuildConfirm()
    {
        if (PreConfirm() == false)
        {
            return;
        }

        GameManager.m_Tower.Instant_Tower_Confirm();
        temp_Tower.transform.position = selectedPos;
        temp_Tower.Confirm_Init();
        temp_Tower.AccumCoin = tw_Info[Define.TowerName[(int)temp_Tower.Tower_Code]].Cost;
        temp_Tower = null;

        SoundManager.Instance.PlaySound("UI/Build");
    }

    public void BuildConfirm_Restart(int upgrade) //? 불러오기모드에서 바로 업그레이드까지할때 - 맵은 처음에 불러온걸로 고정하면되서 맵을 따로 저장하면안됨
    {
        //if (PreConfirm() == false)
        //{
        //    return;
        //}

        GameManager.m_Tower.Instant_Tower_Confirm();
        temp_Tower.transform.position = selectedPos;
        temp_Tower.Confirm_Init();
        temp_Tower.AccumCoin = tw_Info[Define.TowerName[(int)temp_Tower.Tower_Code]].Cost;

        Upgrade(temp_Tower, upgrade);

        temp_Tower = null;

        SoundManager.Instance.PlaySound("UI/Build");
    }


    public void RemoveRestart(Tower_Stat target, Vector2Int pos)
    {
        target.confirm_Check = false;
        target.gameObject.SetActive(false);
        target.Remove_Event();

        //if (map[pos.x, pos.y] == Define.TileType.Tower)
        //{
        //    map[pos.x, pos.y] = Define.TileType.Floor;
        //}
        //else if (map[pos.x, pos.y] == Define.TileType.Changeable_Tower)
        //{
        //    map[pos.x, pos.y] = Define.TileType.Changeable_Floor;
        //}
        //GameManager.m_Map.nowMap = map;
        GameManager.Instance.selected_floor = false;
        mark.transform.position = selectedPos;
        mark.SetActive(false);
    }


    public void RemoveTower()
    {//? 디스트로이는 되돌릴 수가 없으니까 그냥 SetActive로 하는게 나을듯
        //GameManager.m_Resource.Destroy_Prefab(selected_Tower.gameObject);
        if (selected_Tower != null)
        {
            float coin = selected_Tower.AccumCoin * 0.8f;
            GameManager.Instance.AddCoin((int)coin);
            selected_Tower.confirm_Check = false;
            selected_Tower.gameObject.SetActive(false);
            selected_Tower.Remove_Event();
            selected_Tower = null;
            RemoveOver();
        }
    }

    private void RemoveOver()
    {
        if (map[(int)selectedPos.x, (int)selectedPos.y] == Define.TileType.Tower)
        {
            map[(int)selectedPos.x, (int)selectedPos.y] = Define.TileType.Floor;
        }
        else if (map[(int)selectedPos.x, (int)selectedPos.y] == Define.TileType.Changeable_Tower)
        {
            map[(int)selectedPos.x, (int)selectedPos.y] = Define.TileType.Changeable_Floor;
        }
        GameManager.m_Map.nowMap = map;
        GameManager.Instance.selected_floor = false;
        //GameManager.Instance.selected_tower = false;
        mark.transform.position = selectedPos;
        mark.SetActive(false);

        SoundManager.Instance.PlaySound("UI/Sell");
    }




    public void Upgrade ()
    {
        if (selected_Tower != null && selected_Tower.Level < 5)
        {
            if (GameManager.Instance.SubtractCoin(tw_Info[Define.TowerName[(int)selected_Tower.Tower_Code]].Upgrade[selected_Tower.Level]))
            {
                selected_Tower.AccumCoin += tw_Info[Define.TowerName[(int)selected_Tower.Tower_Code]].Upgrade[selected_Tower.Level];
                selected_Tower.Upgrade_Tower();

                SoundManager.Instance.PlaySound("UI/Upgrade");
            }
        }
        else
        {
            SoundManager.Instance.PlaySound("UI/WrongClick");
            Debug.Log("업그레이드 실패 - 이미풀업");
        }
    }

    public void Upgrade(Tower_Stat tower, int level) //? 강제업그레이드 (불러오기로 비용체크없이 업글만할때)
    {
        for (int i = 0; i < level; i++)
        {
            tower.AccumCoin += tw_Info[Define.TowerName[(int)tower.Tower_Code]].Upgrade[tower.Level];
            tower.Upgrade_Tower();
        }
    }


}
