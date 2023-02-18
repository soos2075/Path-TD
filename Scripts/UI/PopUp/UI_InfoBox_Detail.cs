using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_InfoBox_Detail : UI_PopUp
{

    enum Contents
    {
        sprite,
        name,
        memo,
        CloseArea

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
        GetGameObject((int)Contents.CloseArea).gameObject.AddUIEvent((PointerEventData data) => ClosePopUp());
    }

    private void Start()
    {
        DetailSprite(GameManager.m_Info.ShowMonsterSprite(MonsterInitial));
        DetailName(GameManager.m_Info.ShowMonster_Name(MonsterInitial));
        DatailMemo(GameManager.m_Info.ShowMonster_Detail_info(MonsterInitial));

        
    }
    void DetailSprite(Sprite _sprite)
    {
        GetGameObject((int)Contents.sprite).GetComponent<Image>().sprite = _sprite;
    }

    void DetailName(string _name)
    {
        GetGameObject((int)Contents.name).GetComponent<Text>().text = _name;
    }

    void DatailMemo(string _memo)
    {
        GetGameObject((int)Contents.memo).GetComponent<Text>().text = $"{GameManager.m_Info.ShowMonster_Ability(MonsterInitial)} \n\n{_memo}";
    }


}
