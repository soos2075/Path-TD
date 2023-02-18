using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EditTool : MonoBehaviour
{
    public bool editMode;
    public Toggle toggleSwitch;
    //public GameObject warning;


    RaycastHit2D myRay;
    Vector3 mousetPos;
    GameObject selected;
    Vector2 selectPos;
    Navi na;

    //bool change = false;
    int changeCount = 0;

    private Define.TileType[,] map;
    GameObject folder;

    //public Toggle toggle;
    //public UI_TowerPanel towerPanel;

    void Start()
    {
        //map = GameManager.m_Map.nowMap;
        //folder = GameObject.Find("Tile_Folder");
    }
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            //Debug.Log("UI이벤트 발생중");
            return;
        }


        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            //Debug.Log("터치중@@@@@@@@@@@@@@");
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                if (editMode)
                {
                    map = GameManager.m_Map.nowMap;
                    folder = GameObject.Find("@Tile_Root");
                    mousetPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                    myRay = Physics2D.Raycast(mousetPos, Vector3.forward, 10, LayerMask.GetMask("Tile"));

                    RouteCheck();
                }
            }
        }
#if UNITY_EDITOR
        else //? 에디터상에서 작동시 필요한 코드 - 모바일빌드할땐 필요없음
        {
            if (Input.GetMouseButtonDown(0) && editMode)
            {
                map = GameManager.m_Map.nowMap;
                folder = GameObject.Find("@Tile_Root");
                mousetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                myRay = Physics2D.Raycast(mousetPos, Vector3.forward, 10, LayerMask.GetMask("Tile"));

                RouteCheck();
            }
        }
#endif

    }


    public void Edit(Toggle sender)
    {
        if (GameManager.Instance.PlayState == Define.PlayMode.Play)
        {
            SoundManager.Instance.PlaySound("UI/WrongClick");
            sender.isOn = false;
            return;
        }

        editMode = (editMode) ? false : true;

        GameManager.Instance.PlayState = editMode ? Define.PlayMode.MapEdit : Define.PlayMode.TowerBuild;
        GameManager.Instance.GetComponent<Tower_Builder>().DoEditMode();
        SoundManager.Instance.PlaySound("UI/Switch");
    }

    public void Edit_Reset()
    {
        toggleSwitch.isOn = false;
        editMode = false;
        GameManager.Instance.PlayState = Define.PlayMode.TowerBuild;
        GameManager.Instance.GetComponent<Tower_Builder>().DoEditMode();
    }


    private void RouteCheck()
    {
        if (myRay.collider != null)
        {
            selected = myRay.collider.gameObject;
            selectPos = new Vector2(selected.transform.position.x, selected.transform.position.y);
            na = GameManager.Instance.GetComponent<Navi>();

            if (map[(int)selectPos.x, (int)selectPos.y] == Define.TileType.Changeable_Ground)
            {
                if (GameManager.Instance.SubtractPlatform(1)) //? 설치가능한 타일이 있을때만
                {
                    map[(int)selectPos.x, (int)selectPos.y] = Define.TileType.Changeable_Floor;
                    ChangeApply();
                }
            }
            else if (map[(int)selectPos.x, (int)selectPos.y] == Define.TileType.Changeable_Floor)
            {
                GameManager.Instance.AddPlatform(1); //? 플랫폼제거 = 설치가능플랫폼 +1
                SoundManager.Instance.PlaySound("UI/PlatformRecall");

                map[(int)selectPos.x, (int)selectPos.y] = Define.TileType.Changeable_Ground;
                GameManager.m_Resource.Instant_Prefab("Tile/Tile_Changeable_Ground", new Vector3((int)selectPos.x, (int)selectPos.y), folder.transform);
                changeCount--;
                Destroy(selected);
                GameManager.m_Map.nowMap = map;
                na.SearchMap(true);
            }
        }
    }

    private void ChangeApply()
    {
        GameManager.m_Map.nowMap = map;
        na.SearchMap(false);
        //? 길을 못찾았으면
        if(!na.path_correct_Check)
        {
            map[(int)selectPos.x, (int)selectPos.y] = Define.TileType.Changeable_Ground;
            GameManager.m_Map.nowMap = map;
            Debug.Log($"해당 지점을 막을 수 없습니다 : {(int)selectPos.x} , {(int)selectPos.y}");
            SoundManager.Instance.PlaySound("UI/WrongClick");
            //warning.GetComponent<SelfDisable>().OnActive();
            GameManager.Instance.AddPlatform(1);
            return;
        }


        //? 길은 찾았는데 경로상에 벽을 놓을경우
        SoundManager.Instance.PlaySound("UI/Platform");
        for (int i = 0; i < na.moveList.Count; i++)
        {
            if (na.moveList[i].posX == selectPos.x && na.moveList[i].posY == selectPos.y)
            {
                GameManager.m_Resource.Instant_Prefab("Tile/Tile_Changeable_Floor", new Vector3((int)selectPos.x, (int)selectPos.y), folder.transform);
                changeCount++;
                Destroy(selected);
                na.SearchMap(true);
                return;
            }
        }
        //? 길 찾았는데 경로상에 놓는게 아닐경우
        GameManager.m_Resource.Instant_Prefab("Tile/Tile_Changeable_Floor", new Vector3((int)selectPos.x, (int)selectPos.y), folder.transform);
        changeCount++;
        Destroy(selected);
    }
}
