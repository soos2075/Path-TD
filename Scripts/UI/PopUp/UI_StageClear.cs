using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_StageClear : UI_PopUp
{
    enum GameObjects
    {
        Panel,
        //Clear
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
        GetGameObject((int)GameObjects.Panel).gameObject.AddUIEvent((PointerEventData data) => StageClearAfter(data));
    }



    void StageClearAfter (PointerEventData data)
    {
        //CloseSelf(1.5f);
        GameManager.m_Save.DeleteSaveFile();

        SceneChange.Instance.Change("#2_SelectStage");
    }


}
