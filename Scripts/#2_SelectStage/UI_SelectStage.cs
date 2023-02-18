using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SelectStage : UI_Base
{
    enum Stage
    {
        Stage0,
        Stage1,
        Stage2,
        Stage3,
        Stage4,
        Stage5,
    }

    enum Stage_Special
    {
        //? 이건 버튼말고 이미지로 하던지 하고 AddUIEvent를 따로 설정해주면됨.
        //? 아니면 그냥 위에 스테이지 0~10까지 있다고 치면 스페셜스테이지를 +1씩해서 해도 상관없고 아무튼 나중에 편한대로 확장해서 쓰면됨
    }


    void Start()
    {
        Bind<Button>(typeof(Stage));

        Type tp = typeof(Stage);
        string[] names = Enum.GetNames(tp);

        int clearStage = PlayerPrefs.GetInt("Clear") + 1;


        for (int i = (int)Stage.Stage1; i <= clearStage; i++)
        {
            if (i == names.Length) break; //? 만든 스테이지까지만
            GetButton(i).gameObject.AddUIEvent_StageSelect(SelectEvent, i);
            GetButton(i).interactable = true;
        }
        //? Complete 체크
        for (int i = (int)Stage.Stage1; i < clearStage; i++)
        {
            GetButton(i).transform.GetChild(1).gameObject.SetActive(true);
        }
    }


    void SelectEvent (PointerEventData data, int stage)
    {
        SelectStage.Instance.StageButton(stage);
    }



    public void TestMode ()
    {
        Type tp = typeof(Stage);
        string[] names = Enum.GetNames(tp);
        for (int i = (int)Stage.Stage1; i < names.Length; i++)
        {
            GetButton(i).gameObject.AddUIEvent_StageSelect(SelectEvent, i);
            GetButton(i).interactable = true;
        }
    }
}
