using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_InfoBox : UI_PopUp
{
    enum Contents
    {
        MonsterSprite,
        Quantity
    }


    public string MonsterInitial;


    public override void Init()
    {
        //base.Init();
        Bind<GameObject>(typeof(Contents));
    }



    private void Awake()
    {
        Init();
        GetGameObject((int)Contents.MonsterSprite).AddUIEvent((PointerEventData data) => ShowInfoDetail());
    }

    private void Start()
    {
        
    }


    void ShowInfoDetail()
    {
        //var go = GameManager.m_Resource.Instant_Prefab("UI/PopUp/InfoBox_Detail", transform);
        var go = GameManager.m_UI.ShowPopUp<UI_InfoBox_Detail>(transform.parent.parent, "InfoBox_Detail");
        go.GetComponent<UI_InfoBox_Detail>().MonsterInitial = this.MonsterInitial;
    }


}
