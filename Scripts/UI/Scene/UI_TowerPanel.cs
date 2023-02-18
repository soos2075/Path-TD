using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TowerPanel : UI_Scene
{
    enum Buttons
    {
        On,
        Off,
    }

    public enum ActivePage
    {
        Page1,
        Page2,
    }

    public ActivePage page;

    Button on;
    Button off;

    public GameObject tower_Panel;

    Image page1;
    Image page2;


    private void Awake()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(ActivePage));

    }

    void Init_TowerPanel()
    {
        on = GetButton((int)Buttons.On);
        off = GetButton((int)Buttons.Off);

        page1 = GetImage((int)ActivePage.Page1);
        page2 = GetImage((int)ActivePage.Page2);

        page1.gameObject.AddUIEvent((PointerEventData data) => PressPage_1());
        page2.gameObject.AddUIEvent((PointerEventData data) => PressPage_2());


        PageInitialize(page);
        OFF_Button();
    }
    void Start()
    {
        Init_TowerPanel();
    }


    public void ON_Button ()
    {
        if (GameManager.Instance.PlayState == Define.PlayMode.MapEdit)
        {
            SoundManager.Instance.PlaySound("UI/WrongClick");
            return;
        }

        SoundManager.Instance.PlaySound("UI/Select");
        on.gameObject.SetActive(false);
        off.gameObject.SetActive(true);
        tower_Panel.SetActive(true);
        page1.gameObject.SetActive(true);
        page2.gameObject.SetActive(true);

    }

    public void OFF_Button()
    {
        SoundManager.Instance.PlaySound("UI/Select");
        on.gameObject.SetActive(true);
        off.gameObject.SetActive(false);
        tower_Panel.SetActive(false);
        page1.gameObject.SetActive(false);
        page2.gameObject.SetActive(false);
    }


    void PageInitialize(ActivePage active)
    {
        switch (active)
        {
            case ActivePage.Page1:
                page1.GetComponent<Image>().sprite = GameManager.m_Resource.SearchUISprite("Btn_SquareOrange_d");
                page2.GetComponent<Image>().sprite = GameManager.m_Resource.SearchUISprite("Btn_SquareOrange_a");
                tower_Panel.GetComponent<UI_TowerSelect>().PageSelect_1();
                break;
            case ActivePage.Page2:
                page1.GetComponent<Image>().sprite = GameManager.m_Resource.SearchUISprite("Btn_SquareOrange_a");
                page2.GetComponent<Image>().sprite = GameManager.m_Resource.SearchUISprite("Btn_SquareOrange_d");
                tower_Panel.GetComponent<UI_TowerSelect>().PageSelect_2();
                break;
        }
    }

    void PressPage_1()
    {
        page = ActivePage.Page1;
        PageInitialize(page);
    }

    void PressPage_2()
    {
        page = ActivePage.Page2;
        PageInitialize(page);
    }
}
