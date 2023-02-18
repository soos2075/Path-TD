using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_GameOver : UI_PopUp
{
    enum GameObjects
    {
        Panel,
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));

    }

    void Awake()
    {
        Init();
    }


    void Start()
    {
        GetGameObject((int)GameObjects.Panel).gameObject.AddUIEvent((PointerEventData data) => GameOver());
    }

    void GameOver()
    {
        ClosePopUp();
        //재시작,종료,미션초기화 UI 띄워주기
        GameManager.m_UI.ShowPopUp<UI_Pause>("PausePopUp");
    }
}
