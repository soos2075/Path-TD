using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Tower_Info : UI_PopUp
{
    enum Objects
    {
        Tower_Info,
        Tower_Sprite,
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(Objects));
    }

    void Awake()
    {
        Init();
    }

    public void SetTowetData (string info, Sprite sprite)
    {
        Set_TowerInfo(info);
        Set_TowerSprite(sprite);
    }


    void Set_TowerInfo(string info)
    {
        GetGameObject((int)Objects.Tower_Info).GetComponent<Text>().text = info;
    }
    
    void Set_TowerSprite(Sprite sprite)
    {
        GetGameObject((int)Objects.Tower_Sprite).GetComponent<Image>().sprite = sprite;
    }

}
