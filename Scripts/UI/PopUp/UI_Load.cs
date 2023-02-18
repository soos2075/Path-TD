using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Load : UI_PopUp
{
    enum Buttons
    {
        Yes,
        No,
    }

    Text loadText;


    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));

        loadText = Util.FindChild(gameObject, "LoadText").GetComponent<Text>();
    }

    void Awake()
    {
        Init();
    }

    void Start()
    {
        GetButton((int)Buttons.Yes).gameObject.AddUIEvent((PointerEventData data) => B_Yes(data));
        GetButton((int)Buttons.No).gameObject.AddUIEvent((PointerEventData data) => B_No(data));

        if (LanguageManager.Instance.Language == Define.Language.Kor)
        {
            loadText.text = "플레이하던 데이터를 불러올까요?";
        }
        else
        {
            loadText.text = "Load existing data?";
        }
    }


    void B_Yes(PointerEventData data)
    {
        FindObjectOfType<Stage_1>().LoadAutoSave();
        ClosePopUp();
        SoundManager.Instance.PlaySound("UI/Click");
    }
    void B_No(PointerEventData data)
    {
        FindObjectOfType<Stage_1>().Stage_Reset();
        ClosePopUp();
        SoundManager.Instance.PlaySound("UI/Click");
    }

}
