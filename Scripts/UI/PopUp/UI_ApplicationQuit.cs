using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ApplicationQuit : UI_PopUp
{
    enum Buttons
    {
        Yes,
        No,
    }

    Text quitText;

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));

        quitText = Util.FindChild(gameObject, "QuitText").GetComponent<Text>();
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
            quitText.text = "게임을 종료할까요?";
        }
        else
        {
            quitText.text = "Quit the game?";
        }
    }


    void B_Yes(PointerEventData data)
    {
        SoundManager.Instance.PlaySound("UI/Click");
        Debug.Log("게임종료");
        Application.Quit();
    }
    void B_No(PointerEventData data)
    {
        SoundManager.Instance.PlaySound("UI/Click");
        ClosePopUp();
        
    }
}
