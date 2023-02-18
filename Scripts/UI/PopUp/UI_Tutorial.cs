using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Tutorial : UI_PopUp
{


    enum PopupObjects
    {
        Popup_1,
        Popup_2,
        Popup_3,
        Popup_4,
        Popup_5,
        Popup_6,
        Popup_7,
        Popup_8,
        Popup_9,
        Popup_10,
        Popup_11,
        Popup_12,
        Popup_13,
        Popup_14,
        Popup_15,
        Popup_16,
    }

    public GameObject nowSign;


    GameObject tuto_Square;
    GameObject tuto_Circle;


    Transform canvas_World;
    Transform canvas_Main;
    Transform tower;
    Transform select_Panel;


    private void Awake()
    {
        Bind<GameObject>(typeof(PopupObjects));

        tuto_Circle = Resources.Load<GameObject>("Prefabs/UI/Tuto/Tuto_Circle_size_1");
        tuto_Square = Resources.Load<GameObject>("Prefabs/UI/Tuto/Tuto_Square_size_1");
    }
    void Start()
    {
        canvas_World = GameObject.FindGameObjectWithTag("Canvas_World").transform;
        canvas_Main = GameObject.FindGameObjectWithTag("Canvas_Screen").transform;
        tower = canvas_Main.GetChild(11); // GameObject.Find("Tower_Select_Panel").transform;
        select_Panel = canvas_Main.GetChild(12);  //GameObject.Find("TowerPanel_Switch").transform;

        StartCoroutine(Tuto2());
    }

    IEnumerator Tuto()
    {
        GetGameObject((int)PopupObjects.Popup_1).GetComponentInChildren<Image>().enabled = true;
        yield return new WaitForSeconds(1);

        yield return new WaitUntil(() => FindObjectOfType<UI_TowerSelect>() != null);           //? 타워패널이 켜짐

        GetGameObject((int)PopupObjects.Popup_1).GetComponentInChildren<Image>().enabled = false;
        GetGameObject((int)PopupObjects.Popup_2).GetComponentInChildren<Image>().enabled = true;

        yield return new WaitUntil(() => GameManager.Instance.Tower_Number == 0);               //? 파이어타워 선택

        GetGameObject((int)PopupObjects.Popup_2).GetComponentInChildren<Image>().enabled = false;
        GetGameObject((int)PopupObjects.Popup_3).GetComponentInChildren<Image>().enabled = true;

        yield return new WaitUntil(() => FindObjectOfType<Tower_Builder>().TempTowerIsOn());    //? 임시타워 활성화될때

        GetGameObject((int)PopupObjects.Popup_3).GetComponentInChildren<Image>().enabled = false;
        GetGameObject((int)PopupObjects.Popup_4).GetComponentInChildren<Image>().enabled = true;

        yield return new WaitUntil(() => GameManager.m_Tower.key_Number > 0);                   //? 타워 확정됐을때

        GetGameObject((int)PopupObjects.Popup_4).GetComponentInChildren<Image>().enabled = false;
        GetGameObject((int)PopupObjects.Popup_5).GetComponentInChildren<Image>().enabled = true;

        yield return new WaitUntil(() => GameManager.Instance.PlayState == Define.PlayMode.Play); //? 시작버튼 눌렀을 때

        GetGameObject((int)PopupObjects.Popup_5).GetComponentInChildren<Image>().enabled = false;

        yield return new WaitUntil(() => GameManager.Instance.PlayState == Define.PlayMode.TowerBuild); //? 1웨이브 끝났을때

        GetGameObject((int)PopupObjects.Popup_6).GetComponentInChildren<Image>().enabled = true;

        yield return new WaitUntil(() => FindObjectOfType<Tower_Builder>().selected_Tower != null); //? 타워 선택했을 때

        GetGameObject((int)PopupObjects.Popup_6).GetComponentInChildren<Image>().enabled = false;
        GetGameObject((int)PopupObjects.Popup_7).GetComponentInChildren<Image>().enabled = true;

        yield return new WaitUntil(() => FindObjectOfType<Tower_Builder>().selected_Tower.myTower.level > 0); //? 업그레이드 했을 때

        GetGameObject((int)PopupObjects.Popup_7).GetComponentInChildren<Image>().enabled = false;
        GetGameObject((int)PopupObjects.Popup_8).GetComponentInChildren<Image>().enabled = true;

        yield return new WaitUntil(() => GameManager.Instance.PlayState == Define.PlayMode.Play); //? 시작버튼 눌렀을 때

        GetGameObject((int)PopupObjects.Popup_8).GetComponentInChildren<Image>().enabled = false;

        yield return new WaitUntil(() => GameManager.Instance.PlayState == Define.PlayMode.TowerBuild); //? 2웨이브 끝났을때

        GetGameObject((int)PopupObjects.Popup_9).GetComponentInChildren<Image>().enabled = true;

        yield return new WaitUntil(() => GameManager.Instance.PlayState == Define.PlayMode.MapEdit); //? 에딧 On

        GetGameObject((int)PopupObjects.Popup_9).GetComponentInChildren<Image>().enabled = false;
        GetGameObject((int)PopupObjects.Popup_10).GetComponentInChildren<Image>().enabled = true;
        GetGameObject((int)PopupObjects.Popup_11).GetComponentInChildren<Image>().enabled = true;
        GetGameObject((int)PopupObjects.Popup_12).GetComponentInChildren<Image>().enabled = true;
        
        yield return new WaitUntil(() => GameManager.Instance.Platform == 7);                       //? 플랫폼 3개 설치

        GetGameObject((int)PopupObjects.Popup_10).GetComponentInChildren<Image>().enabled = false;
        GetGameObject((int)PopupObjects.Popup_11).GetComponentInChildren<Image>().enabled = false;
        GetGameObject((int)PopupObjects.Popup_12).GetComponentInChildren<Image>().enabled = false;
        GetGameObject((int)PopupObjects.Popup_13).GetComponentInChildren<Image>().enabled = true;

        yield return new WaitUntil(() => GameManager.Instance.PlayState == Define.PlayMode.TowerBuild); //? 에딧 off

        GetGameObject((int)PopupObjects.Popup_13).GetComponentInChildren<Image>().enabled = false;
        GetGameObject((int)PopupObjects.Popup_14).GetComponentInChildren<Image>().enabled = true;

        yield return new WaitUntil(() => FindObjectOfType<Tower_Builder>().selected_Tower != null); //? 타워 선택했을 때

        GetGameObject((int)PopupObjects.Popup_14).GetComponentInChildren<Image>().enabled = false;
        GetGameObject((int)PopupObjects.Popup_15).GetComponentInChildren<Image>().enabled = true;

        yield return new WaitUntil(() => FindObjectOfType<Tower_Builder>().selected_Tower.myTower.level > 1); //? 업그레이드 했을 때

        GetGameObject((int)PopupObjects.Popup_15).GetComponentInChildren<Image>().enabled = false;
        GetGameObject((int)PopupObjects.Popup_16).GetComponentInChildren<Image>().enabled = true;

        yield return new WaitUntil(() => GameManager.Instance.PlayState == Define.PlayMode.Play); //? 3웨이브 시작

        GetGameObject((int)PopupObjects.Popup_16).GetComponentInChildren<Image>().enabled = false;

        //? 끝
    }


    IEnumerator Tuto2()
    {
        nowSign = GameObject.Instantiate(tuto_Circle, select_Panel);

        yield return new WaitForSeconds(1);

        yield return new WaitUntil(() => FindObjectOfType<UI_TowerSelect>() != null);           //? 타워패널이 켜짐

        Destroy(nowSign);
        nowSign = GameObject.Instantiate(tuto_Square, tower.GetChild(0).GetChild(0));

        yield return new WaitUntil(() => GameManager.Instance.Tower_Number == 0);               //? 파이어타워 선택

        Destroy(nowSign);
        nowSign = GameObject.Instantiate(tuto_Square, new Vector3(4, 2, 0), Quaternion.identity, canvas_World);

        yield return new WaitUntil(() => FindObjectOfType<Tower_Builder>().TempTowerIsOn() &&
            FindObjectOfType<Tower_Builder>().temp_Tower.GetComponent<Tower_Stat>().Position == new Vector2Int(4,2));    //? 임시타워 활성화될때

        GetGameObject((int)PopupObjects.Popup_4).GetComponentInChildren<Image>().enabled = true;
        Destroy(nowSign);

        yield return new WaitUntil(() => GameManager.m_Tower.key_Number > 0);                   //? 타워 확정됐을때

        GetGameObject((int)PopupObjects.Popup_4).GetComponentInChildren<Image>().enabled = false;
        nowSign = GameObject.Instantiate(tuto_Circle, canvas_Main.GetChild(3));

        yield return new WaitUntil(() => GameManager.Instance.PlayState == Define.PlayMode.Play); //? 시작버튼 눌렀을 때

        Destroy(nowSign);

        yield return new WaitUntil(() => GameManager.Instance.PlayState == Define.PlayMode.TowerBuild); //? 1웨이브 끝났을때

        nowSign = GameObject.Instantiate(tuto_Square, new Vector3(4, 2, 0), Quaternion.identity, canvas_World);

        yield return new WaitUntil(() => FindObjectOfType<Tower_Builder>().selected_Tower != null); //? 타워 선택했을 때

        GetGameObject((int)PopupObjects.Popup_7).GetComponentInChildren<Image>().enabled = true;
        Destroy(nowSign);

        yield return new WaitUntil(() => FindObjectOfType<Tower_Builder>().selected_Tower.myTower.level > 0); //? 업그레이드 했을 때

        GetGameObject((int)PopupObjects.Popup_7).GetComponentInChildren<Image>().enabled = false;
        nowSign = GameObject.Instantiate(tuto_Circle, canvas_Main.GetChild(3));

        yield return new WaitUntil(() => GameManager.Instance.PlayState == Define.PlayMode.Play); //? 시작버튼 눌렀을 때

        Destroy(nowSign);

        yield return new WaitUntil(() => GameManager.Instance.PlayState == Define.PlayMode.TowerBuild); //? 2웨이브 끝났을때

        nowSign = GameObject.Instantiate(tuto_Circle, canvas_Main.GetChild(2));

        yield return new WaitUntil(() => GameManager.Instance.PlayState == Define.PlayMode.MapEdit); //? 에딧 On

        Destroy(nowSign);
        var sign1 = GameObject.Instantiate(tuto_Square, new Vector3(2, 3, 0), Quaternion.identity, canvas_World);
        var sign2 = GameObject.Instantiate(tuto_Square, new Vector3(4, 3, 0), Quaternion.identity, canvas_World);
        var sign3 = GameObject.Instantiate(tuto_Square, new Vector3(4, 4, 0), Quaternion.identity, canvas_World);

        yield return new WaitUntil(() => GameManager.Instance.Platform == 7);                       //? 플랫폼 3개 설치

        Destroy(sign1);
        Destroy(sign2);
        Destroy(sign3);
        nowSign = GameObject.Instantiate(tuto_Circle, canvas_Main.GetChild(2));

        yield return new WaitUntil(() => GameManager.Instance.PlayState == Define.PlayMode.TowerBuild); //? 에딧 off

        Destroy(nowSign);
        nowSign = GameObject.Instantiate(tuto_Square, new Vector3(4, 2, 0), Quaternion.identity, canvas_World);

        yield return new WaitUntil(() => FindObjectOfType<Tower_Builder>().selected_Tower != null); //? 타워 선택했을 때

        Destroy(nowSign);
        GetGameObject((int)PopupObjects.Popup_15).GetComponentInChildren<Image>().enabled = true;

        yield return new WaitUntil(() => FindObjectOfType<Tower_Builder>().selected_Tower.myTower.level > 1); //? 업그레이드 했을 때

        GetGameObject((int)PopupObjects.Popup_15).GetComponentInChildren<Image>().enabled = false;
        nowSign = GameObject.Instantiate(tuto_Circle, canvas_Main.GetChild(3));

        yield return new WaitUntil(() => GameManager.Instance.PlayState == Define.PlayMode.Play); //? 3웨이브 시작

        Destroy(nowSign);

        yield return new WaitUntil(() => GameManager.Instance.PlayState == Define.PlayMode.TowerBuild); //? 3웨이브 끝났을때

        nowSign = GameObject.Instantiate(tuto_Circle, select_Panel);

        yield return new WaitUntil(() => FindObjectOfType<UI_TowerSelect>() != null);           //? 타워패널이 켜짐

        Destroy(nowSign);
        nowSign = GameObject.Instantiate(tuto_Square, tower.GetChild(0).GetChild(1));

        yield return new WaitUntil(() => GameManager.Instance.Tower_Number == 3);               //? 멀티타워 선택

        Destroy(nowSign);
        nowSign = GameObject.Instantiate(tuto_Square, new Vector3(2, 2, 0), Quaternion.identity, canvas_World);

        yield return new WaitUntil(() => FindObjectOfType<Tower_Builder>().TempTowerIsOn() &&
            FindObjectOfType<Tower_Builder>().temp_Tower.GetComponent<Tower_Stat>().Position == new Vector2Int(2, 2));    //? 임시타워 활성화될때

        Destroy(nowSign);
        GetGameObject((int)PopupObjects.Popup_4).GetComponentInChildren<Image>().enabled = true;

        yield return new WaitUntil(() => GameManager.m_Tower.key_Number > 1);                   //? 타워개수 2개 확정(파이어,멀티)

        GetGameObject((int)PopupObjects.Popup_4).GetComponentInChildren<Image>().enabled = false;
        nowSign = GameObject.Instantiate(tuto_Circle, canvas_Main.GetChild(3));

        yield return new WaitUntil(() => GameManager.Instance.PlayState == Define.PlayMode.Play); //? 4웨이브 시작

        Destroy(nowSign);

        yield return new WaitUntil(() => GameManager.Instance.PlayState == Define.PlayMode.TowerBuild); //? 4웨이브 끝났을때

        nowSign = GameObject.Instantiate(tuto_Circle, select_Panel);

        yield return new WaitUntil(() => FindObjectOfType<UI_TowerSelect>() != null);           //? 타워패널이 켜짐

        Destroy(nowSign);
        nowSign = GameObject.Instantiate(tuto_Square, tower.GetChild(0).GetChild(2));

        yield return new WaitUntil(() => GameManager.Instance.Tower_Number == 9);               //? 에어타워 선택

        Destroy(nowSign);
        nowSign = GameObject.Instantiate(tuto_Square, new Vector3(6, 2, 0), Quaternion.identity, canvas_World);

        yield return new WaitUntil(() => FindObjectOfType<Tower_Builder>().TempTowerIsOn() &&
            FindObjectOfType<Tower_Builder>().temp_Tower.GetComponent<Tower_Stat>().Position == new Vector2Int(6, 2));    //? 임시타워 활성화될때

        Destroy(nowSign);
        GetGameObject((int)PopupObjects.Popup_4).GetComponentInChildren<Image>().enabled = true;

        yield return new WaitUntil(() => GameManager.m_Tower.key_Number > 2);                   //? 타워개수 3개 확정(파이어,멀티,에어)

        GetGameObject((int)PopupObjects.Popup_4).GetComponentInChildren<Image>().enabled = false;
        nowSign = GameObject.Instantiate(tuto_Circle, canvas_Main.GetChild(3));

        yield return new WaitUntil(() => GameManager.Instance.PlayState == Define.PlayMode.Play); //? 5웨이브 시작

        Destroy(nowSign);

        yield return new WaitUntil(() => GameManager.Instance.PlayState == Define.PlayMode.TowerBuild); //? 5웨이브 끝났을때

        nowSign = GameObject.Instantiate(tuto_Circle, select_Panel);

        yield return new WaitUntil(() => FindObjectOfType<UI_TowerSelect>() != null);           //? 타워패널이 켜짐

        Destroy(nowSign);
        nowSign = GameObject.Instantiate(tuto_Circle, select_Panel.GetChild(3));

        yield return new WaitUntil(() => tower.GetChild(1).gameObject.activeSelf);       //? 2페이지 타워창 켜짐

        Destroy(nowSign);
        nowSign = GameObject.Instantiate(tuto_Square, tower.GetChild(1).GetChild(0));

        yield return new WaitUntil(() => GameManager.Instance.Tower_Number == 7);               //? 버프타워 선택

        Destroy(nowSign);
        nowSign = GameObject.Instantiate(tuto_Square, new Vector3(5, 2, 0), Quaternion.identity, canvas_World);

        yield return new WaitUntil(() => FindObjectOfType<Tower_Builder>().TempTowerIsOn() &&
            FindObjectOfType<Tower_Builder>().temp_Tower.GetComponent<Tower_Stat>().Position == new Vector2Int(5, 2));    //? 임시타워 활성화될때

        Destroy(nowSign);
        GetGameObject((int)PopupObjects.Popup_4).GetComponentInChildren<Image>().enabled = true;

        yield return new WaitUntil(() => GameManager.m_Tower.key_Number > 3);                   //? 타워개수 4개 확정(파이어,멀티,에어,버프)

        GetGameObject((int)PopupObjects.Popup_4).GetComponentInChildren<Image>().enabled = false;
        nowSign = GameObject.Instantiate(tuto_Circle, canvas_Main.GetChild(3));

        yield return new WaitUntil(() => GameManager.Instance.PlayState == Define.PlayMode.Play); //? 5웨이브 시작

        Destroy(nowSign);

        //? 끝
    }


}
