using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BuildMenu : UI_Scene
{



    enum Buttons
    {
        Info,
        Build,
        Sell,
        Upgrade,
    }





    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));

        builder = GameManager.Instance.GetComponent<Tower_Builder>();
    }


    void Awake()
    {
        Init();

        if (LanguageManager.Instance.Language == Define.Language.Kor)
        {
            GetButton((int)Buttons.Info).GetComponentInChildren<Text>().text = "정보";
        }
        else
        {
            GetButton((int)Buttons.Info).GetComponentInChildren<Text>().text = "Info";
        }
    }

    Tower_Builder builder;

    private void Update()
    {
        if (GameManager.Instance.selected_tower || GameManager.Instance.Tower_Number != -1)
        {
            GetButton((int)Buttons.Info).gameObject.SetActive(true);
        }
        else
        {
            GetButton((int)Buttons.Info).gameObject.SetActive(false);
        }


        if (GameManager.Instance.selected_floor && GameManager.Instance.Tower_Number != -1)
        {
            GetButton((int)Buttons.Build).GetComponentInChildren<Text>().text =
                builder.tw_Info[Define.TowerName[GameManager.Instance.Tower_Number]].Cost.ToString();

            GetButton((int)Buttons.Build).gameObject.SetActive(true);
        }
        else
        {
            GetButton((int)Buttons.Build).gameObject.SetActive(false);
        }


        if (builder.selected_Tower != null)
        {
            float coin = builder.selected_Tower.AccumCoin * 0.8f;
            GetButton((int)Buttons.Sell).GetComponentInChildren<Text>().text = ((int)coin).ToString();

            GetButton((int)Buttons.Sell).gameObject.SetActive(true);
        }
        else
        {
            GetButton((int)Buttons.Sell).gameObject.SetActive(false);
        }


        if (builder.selected_Tower != null && builder.selected_Tower.Level < 5)
        {
            GetButton((int)Buttons.Upgrade).GetComponentInChildren<Text>().text = 
                builder.tw_Info[Define.TowerName[(int)builder.selected_Tower.Tower_Code]].Upgrade[builder.selected_Tower.Level].ToString();

            GetButton((int)Buttons.Upgrade).gameObject.SetActive(true);
            GetButton((int)Buttons.Upgrade).enabled = true;
        }
        else if (builder.selected_Tower != null && builder.selected_Tower.Level == 5)
        {
            GetButton((int)Buttons.Upgrade).enabled = false;
            GetButton((int)Buttons.Upgrade).GetComponentInChildren<Text>().text = "";
        }
        else
        {
            GetButton((int)Buttons.Upgrade).gameObject.SetActive(false);
        }
    }




    public void AddEvent_Info (Action<PointerEventData> act)
    {
        GetButton((int)Buttons.Info).gameObject.AddUIEvent(act, Define.MouseEvent.Click);
    }
    public void AddEvent_Sell(Action<PointerEventData> act)
    {
        GetButton((int)Buttons.Sell).gameObject.AddUIEvent(act, Define.MouseEvent.Click);
    }
    public void AddEvent_Build(Action<PointerEventData> act)
    {
        GetButton((int)Buttons.Build).gameObject.AddUIEvent(act, Define.MouseEvent.Click);
    }
    public void AddEvent_Upgrade(Action<PointerEventData> act)
    {
        GetButton((int)Buttons.Upgrade).gameObject.AddUIEvent(act, Define.MouseEvent.Click);
    }



    public void SellButton (int cost)
    {
        GetButton((int)Buttons.Sell).enabled = true;

        int sell = (int)(cost * 0.8f);

        GetButton((int)Buttons.Sell).GetComponentInChildren<Text>().text = sell.ToString();
    }

    public void BuildButton (int cost)
    {
        GetButton((int)Buttons.Upgrade).enabled = false;
        GetButton((int)Buttons.Build).enabled = true;

        GetButton((int)Buttons.Build).GetComponentInChildren<Text>().text = cost.ToString();
    }

    public void UpgradeButton (int cost)
    {
        GetButton((int)Buttons.Upgrade).enabled = true;
        GetButton((int)Buttons.Build).enabled = false;

        GetButton((int)Buttons.Upgrade).GetComponentInChildren<Text>().text = cost.ToString();
    }

}
