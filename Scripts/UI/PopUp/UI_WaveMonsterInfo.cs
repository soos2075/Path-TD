using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_WaveMonsterInfo : UI_PopUp
{
    enum Info
    {
        CloseArea,
        MobImage,
        //MobText,
    }

    public override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(Info));
    }

    private void Awake()
    {
        Init();
    }



    void Start()
    {
        GetGameObject((int)Info.CloseArea).gameObject.AddUIEvent((PointerEventData data) => ClosePanel());
    }

    void ClosePanel()
    {
        ClosePopUp();
    }


    public void ShowMonsterInfo()
    {
        Stage_1 stageInfo = FindObjectOfType<Stage_1>();
        Transform layoutParents = GetGameObject((int)Info.MobImage).transform;

        Dictionary<string, int> tempWaveInfo = new Dictionary<string, int>();

        for (int i = 0; i < stageInfo.targetStage[GameManager.m_Enemy.Wave].PatternCount; i++)
        {
            string mob_Code = stageInfo.targetStage[GameManager.m_Enemy.Wave].monsterList[i].Name;

            if (mob_Code.IndexOf('/') > 0)
            {
                string[] nameCode = mob_Code.Split('/');
                string initial = nameCode[2].Substring(0, 2);
                int quantity = stageInfo.targetStage[GameManager.m_Enemy.Wave].monsterList[i].Quantity;

                if (tempWaveInfo.ContainsKey(initial))
                {
                    int tempQuantity;
                    tempWaveInfo.TryGetValue(initial, out tempQuantity);
                    tempWaveInfo.Remove(initial);
                    tempWaveInfo.Add(initial, tempQuantity + quantity);
                }
                else
                {
                    tempWaveInfo.Add(initial, quantity);
                }
            }
        }


        foreach (var info in tempWaveInfo)
        {
            Sprite mobImage = GameManager.m_Info.ShowMonsterSprite(info.Key);
            var go = GameManager.m_Resource.Instant_Prefab("UI/PopUp/InfoBox", layoutParents);

            //var aa = GameManager.m_UI.ShowPopUp<UI_InfoBox>("InfoBox");
            go.GetComponentInChildren<Image>().sprite = mobImage;
            go.GetComponentInChildren<Text>().text = $"x {info.Value}";
            go.GetComponentInChildren<UI_InfoBox>().MonsterInitial = info.Key;
        }
    }





}
