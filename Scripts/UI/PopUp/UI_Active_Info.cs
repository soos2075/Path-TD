using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Active_Info : UI_PopUp
{
    enum GameObjects
    {
        DetectingSign,
    }

    enum Star_Images
    {
        Level_1,
        Level_2,
        Level_3,
        Level_4,
        Level_5,
    }

    public Sprite grayStar;
    public Sprite YellowStar;

    public override void Init()
    {
        base.Init();

        //Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Star_Images));

        Bind<GameObject>(typeof(GameObjects));
    }


    Tower_Builder tb;


    void Awake()
    {
        Init();
        tb = FindObjectOfType<Tower_Builder>();
    }

    private void Update()
    {
        if (tb.selected_Tower != null)
        {
            DetectingMark(tb.selected_Tower.GetComponent<Tower_Stat>().detecting);
        }
    }

    void DetectingMark(bool detect)
    {
        if (detect)
        {
            GetGameObject((int)GameObjects.DetectingSign).GetComponent<Image>().color = Color.white;
        }
        else
        {
            GetGameObject((int)GameObjects.DetectingSign).GetComponent<Image>().color = Color.clear;
        }
        
    }


    //public void Set_ActiveInfo(string info)
    //{
    //    GetText((int)Texts.Active_Info).text = info;
    //}

    public void Set_LevelImage(int level)
    {
        for (int i = 0; i < 5; i++)
        {
            GetImage(i).sprite = grayStar;
        }

        for (int i = 0; i < level; i++)
        {
            GetImage(i).sprite = YellowStar;
        }
    }
}
