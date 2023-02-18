using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoDataManager
{
    #region TowerData
    public Dictionary<string, Sprite> TowerThumbDict = new Dictionary<string, Sprite>();

    public Sprite ShowTowerSprite(string TowerName)
    {
        Sprite sprite;

        TowerThumbDict.TryGetValue(TowerName, out sprite); //? 일단 캐싱된게 있는지부터 확인

        if (sprite == null)
        {
            sprite = Resources.Load<Sprite>($"Sprites/TowerThumb/{TowerName}"); //? 없으면 리소스에서 직접 찾기
            if (sprite == null)
            {
                Debug.Log($"Sprites/TowerThumb/{TowerName} 를 찾을 수 없습니다.");
                return null;
            }
            else
            {
                TowerThumbDict.Add(TowerName, sprite); //? 한번 찾은건 사전에 등록해놓기
            }
        }
        return sprite;
    }

    #endregion




    #region MonsterData
    public Dictionary<string, Monster> monsterDict = new Dictionary<string, Monster>();

    [Serializable]
    public class Monster
    {
        public int id;              //? Number
        public string initial;      //? 두글자 알파벳
        public string name;         //? Thumbnail Sprite파일 이름과 동일한 이름

        public string name_KR;      //? 한글명 - 필요시 사용
        public string name_EN;      //? 영어명
        
        public string memo;         //? 간략한 설명
        public string memo_EN;      //? 설명 영어

        public string detail_info;  //? 플레이어에게 보여줄 설명
        public string detail_info_EN;

        public int attack;          //? 통과시 라이프 깍이는 개수
        public int score;           //? 잡으면 주는 점수

        
        //? 기타 추가정보가 필요하면 여기에 추가, "Data/InfoData/MonsterData.json"파일 수정 하면댐
        //? 일괄변경은 json파일 전용 에디터같은거 써야되는 것 같고, 일단은 Monster클래스에 내용이 더 있는건 파싱하는데 오류는 안남.
        //? 그래서 만약 새 항목이 추가된다 그러면 역으로 여기서 항목추가하고 해당 MonsterList를 다시 Json파일로 변환시키면 새 항목 추가되서 json파일 만들어질텐데
        //? 그걸로 써도될듯함
    }

    [Serializable]
    public class MonsterList
    {
        public List<Monster> Monsters = new List<Monster>();   //? 여기 이름이 json파일의 가장 바깥그룹의 이름과 같아야함
    }


    public void init()
    {
        LoadMonsterInfoData();
    }


    public void LoadMonsterInfoData()
    {
        var asset = Resources.Load<TextAsset>("Data/InfoData/MonsterData");
        //Debug.Log(asset.text);
        MonsterList monsterList = JsonUtility.FromJson<MonsterList>(asset.text);

        //? 제이슨파일로 재생성할때
        //var asset2 = JsonUtility.ToJson(monsterList);
        //Debug.Log(asset2);
        //System.IO.File.WriteAllText($"{Application.dataPath}/Resources/Data/InfoData/Test222.Json", asset2);

        for (int i = 0; i < monsterList.Monsters.Count; i++)
        {
            monsterDict.Add(monsterList.Monsters[i].initial, monsterList.Monsters[i]);
        }
    }



    private Dictionary<string, Sprite> monsterSprite = new Dictionary<string, Sprite>(); //? 썸네일 이미지 캐싱해놓는곳


    public Sprite ShowMonsterSprite(string initial)
    {
        Sprite sprite;

        monsterSprite.TryGetValue(initial, out sprite); //? 일단 캐싱된게 있는지부터 확인

        if (sprite == null)
        {
            Monster target = CatchTargetMonster(initial);
            if (target == null)
            {
                return null;
            }
            sprite = Resources.Load<Sprite>($"Data/MonsterThumb/{target.name}"); //? 없으면 리소스에서 직접 찾기
            if (sprite == null)
            {
                Debug.Log($"Data/MonsterThumb/{initial} 를 찾을 수 없습니다.");
                return null;
            }
            else
            {
                monsterSprite.Add(initial, sprite); //? 한번 찾은건 사전에 등록해놓기
            } 
        }

        return sprite;
    }

    private Monster CatchTargetMonster(string initial)
    {
        Monster target;
        monsterDict.TryGetValue(initial, out target);

        if (target == null)
        {
            Debug.Log($"등록된 몬스터가 없습니다 : {initial}");
            return null;
        }

        return target;
    }


    public string ShowMonster_Name(string initial)
    {
        if (LanguageManager.Instance.Language == Define.Language.Kor)
        {
            return CatchTargetMonster(initial).name_KR;
        }
        else
        {
            return CatchTargetMonster(initial).name_EN;
        }
    }
    public string ShowMonster_Detail_info(string initial)
    {
        if (LanguageManager.Instance.Language == Define.Language.Kor)
        {
            return CatchTargetMonster(initial).detail_info;
        }
        else
        {
            return CatchTargetMonster(initial).detail_info_EN;
        }
    }
    public string ShowMonster_Ability(string initial)
    {
        if (LanguageManager.Instance.Language == Define.Language.Kor)
        {
            return CatchTargetMonster(initial).memo;
        }
        else
        {
            return CatchTargetMonster(initial).memo_EN;
        }
    }

    public int ShowMonster_Score(string initial)
    {
        return CatchTargetMonster(initial).score;
    }
    public int ShowMonster_Attack(string initial)
    {
        return CatchTargetMonster(initial).attack;
    }
    #endregion



}
