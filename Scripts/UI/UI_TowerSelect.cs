using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UI_TowerSelect : UI_Base
{
    enum I_Tower_Select //? 유니티 씬 화면에서의 순서가 아닌 이 코드의 enum순서에 따라서 저장되는것에 유의
    {
        //? Page1 - 0~9
        Fire = 0,
        Lazer = 1,
        Splash = 2,
        Multi = 3,
        Poison = 4,

        Spear = 8,
        Air = 9,
        Arcane = 11,
        Water = 12,
        Lightning = 13,


        //? Page2 - 10~19
        Buff = 7,
        Thunder = 10,
        Spider = 5,
        Slow = 6,
        Coin = 14,

    }

    enum Page
    {
        Page1,
        Page2,
    }


    private void Awake()
    {
        Bind<Button>(typeof(I_Tower_Select));
        Bind<GameObject>(typeof(Page));
    }


    public int select_number;

    void Start()
    {
        Automation_UIEvent();
    }

    void Automation_UIEvent ()
    {
        Type tp = typeof(I_Tower_Select);
        string[] names = Enum.GetNames(tp);
        for (int i = 0; i < names.Length; i++)
        {
            GetButton(i).gameObject.AddUIEvent_TowerSelect(Select_Num, i);
        }
    }

    void Select_Num(PointerEventData data, int num) { Select_Tower(num); }


    void Select_Tower(int num)
    {
        //GameManager.Instance.selected_tower = true;
        GameManager.Instance.Tower_Number = num;
        Debug.Log($"{num} 번 선택됨");
        SoundManager.Instance.PlaySound("UI/Select");
        if (GameManager.Instance.selected_floor)
        {
            GameManager.Instance.GetComponent<Tower_Builder>().BuildTower();
            //FindObjectOfType<Tower_Builder>().BuildTower();
        }
    }

    public void PageSelect_1()
    {
        GetGameObject((int)Page.Page1).SetActive(true);
        GetGameObject((int)Page.Page2).SetActive(false);
    }
    public void PageSelect_2()
    {
        GetGameObject((int)Page.Page1).SetActive(false);
        GetGameObject((int)Page.Page2).SetActive(true);
    }


}
