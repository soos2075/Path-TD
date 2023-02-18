using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Pause : UI_PopUp
{

    enum Buttons
    {
        Continue,
        Restart,
        Reset,
        Select,
        Tutorial,
        Language,

    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
    }

    void Awake()
    {
        Init();
    }

    void Start()
    {
        LanguageSetting();

        GetButton((int)Buttons.Continue).gameObject.AddUIEvent((PointerEventData data) => B_Continue(data));
        GetButton((int)Buttons.Restart).gameObject.AddUIEvent((PointerEventData data) => B_Restart(data));
        GetButton((int)Buttons.Reset).gameObject.AddUIEvent((PointerEventData data) => B_Reset(data));
        GetButton((int)Buttons.Select).gameObject.AddUIEvent((PointerEventData data) => B_Select(data));

        //GetButton((int)Buttons.Tutorial).gameObject.AddUIEvent((PointerEventData data) => B_Tutorial(data));

        GetButton((int)Buttons.Language).gameObject.AddUIEvent((PointerEventData data) => B_LanguageSelect());

        if (GameManager.Instance.Life <= 0)
        {
            GetButton((int)Buttons.Continue).gameObject.SetActive(false);
        }
    }


    void LanguageSetting()
    {
        if (LanguageManager.Instance.Language == Define.Language.Kor)
        {
            GetButton((int)Buttons.Continue).GetComponentInChildren<Text>().text = "계속하기";
            GetButton((int)Buttons.Restart).GetComponentInChildren<Text>().text = "다시하기";
            GetButton((int)Buttons.Reset).GetComponentInChildren<Text>().text = "스테이지 초기화";
            GetButton((int)Buttons.Select).GetComponentInChildren<Text>().text = "스테이지 선택";
            GetButton((int)Buttons.Language).GetComponentInChildren<Text>().text = "언어설정";
        }
    }




    void B_Continue (PointerEventData data)
    {
        Time.timeScale = 1;
        ClosePopUp();
        SoundManager.Instance.PlaySound("UI/Click");
    }

    void B_Restart (PointerEventData data)
    {
        Time.timeScale = 1;
        ClosePopUp();
        FindObjectOfType<Stage_1>().Stage_Restart();
        SoundManager.Instance.PlaySound("UI/Click");
    }

    void B_Reset(PointerEventData data)
    {
        Time.timeScale = 1;
        ClosePopUp();
        FindObjectOfType<Stage_1>().Stage_Reset(); //? 중간데이터 삭제
        SceneChange.Instance.Change("#3_Stage");
        SoundManager.Instance.PlaySound("UI/Click");
    }

    void B_Select(PointerEventData data)
    {
        Time.timeScale = 1;
        ClosePopUp();
        SceneChange.Instance.Change("#2_SelectStage");
        SoundManager.Instance.PlaySound("UI/Click");
    }

    void B_Tutorial(PointerEventData data)
    {
        GameManager.m_UI.ShowPopUp<UI_PopUp>("Tutorial", true);
        SoundManager.Instance.PlaySound("UI/Click");
    }

    void B_LanguageSelect()
    {
        GameManager.m_UI.ShowPopUp<UI_PopUp>("LanguagePopup", true);
        SoundManager.Instance.PlaySound("UI/Click");
    }
}
