using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Wave : UI_Scene
{

    Stage_1 stage_date;

    int wave_Now;
    int wave_All;

    enum Texts
    {
        Wave,

    }

    private void Awake()
    {
        Bind<Text>(typeof(Texts));
    }

    void Start()
    {
        stage_date = FindObjectOfType<Stage_1>();

        GetText((int)Texts.Wave).gameObject.AddUIEvent((PointerEventData data) => ShowWaveInfo(data));
    }


    public void WaveInfo()
    {
        wave_All = stage_date.targetStage.Count;
        wave_Now = GameManager.m_Enemy.Wave;

        GetText((int)Texts.Wave).text = $"Wave : {wave_Now} / {wave_All}";

        
    }

    private void Update()
    {
        if (stage_date.targetStage != null)
        {
            GetText((int)Texts.Wave).text = $"{GameManager.m_Enemy.Wave + 1} / {stage_date.targetStage.Count}";

            if (GameManager.m_Enemy.Wave + 1 > stage_date.targetStage.Count)
            {
                GetText((int)Texts.Wave).text = $"{GameManager.m_Enemy.Wave} / {stage_date.targetStage.Count}";
            }
        }
    }

    void ShowWaveInfo(PointerEventData data)
    {
        var uiinfo = FindObjectOfType<UI_WaveMonsterInfo>();
        if (uiinfo != null)
        {
            uiinfo.ClosePopUp();
            return;
        }


        var info = GameManager.m_UI.ShowPopUp<UI_WaveMonsterInfo>("WaveMonsterInfo");
        SoundManager.Instance.PlaySound("UI/Click");
        info.ShowMonsterInfo();
    }

    public void ShowWaveInfo()
    {
        var uiinfo = FindObjectOfType<UI_WaveMonsterInfo>();
        if (uiinfo != null)
        {
            uiinfo.ClosePopUp();
            return;
        }

        var info = GameManager.m_UI.ShowPopUp<UI_WaveMonsterInfo>("WaveMonsterInfo");
        SoundManager.Instance.PlaySound("UI/Click");
        info.ShowMonsterInfo();
    }
}
