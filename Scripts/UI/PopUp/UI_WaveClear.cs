using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_WaveClear : UI_PopUp
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
        GetGameObject((int)GameObjects.Panel).gameObject.AddUIEvent((PointerEventData data) => WaveClear());
    }

    void WaveClear()
    {
        ClosePopUp();
    }
}
