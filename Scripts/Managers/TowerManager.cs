using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager
{
    //? 타워를 실시간으로 관리해줄 배열필요(추가,삭제가 되야하지만 개수가 그렇게 많지않으면 고정배열로 해도 될듯?)
    //public Tower_Stat[] towerList = new Tower_Stat[10];
    public Dictionary<int, Tower_Stat> tower_List = new Dictionary<int, Tower_Stat>();

    public int key_Number { get; set; }
    //?


    GameObject Tower_Root //? 타워 폴더
    {
        get
        {
            GameObject root = GameObject.Find("@Tower_Root");
            if (root == null)
            {
                root = new GameObject { name = "@Tower_Root" };
            }
            return root;
        }
    }
    GameObject Bullet_Root //? 탄 폴더
    {
        get
        {
            GameObject root = GameObject.Find("@Bullet_Root");
            if (root == null)
            {
                root = new GameObject { name = "@Bullet_Root" };
            }
            return root;
        }
    }

    GameObject temp_tower; //? 사전에 등록하기전 임시저장소

    public GameObject Instant_Tower (Tower_Stat.Tower bone, string tower_name)
    {
        GameObject tower = GameManager.m_Resource.Instant_Prefab($"Tower/{tower_name}", Tower_Root.transform);
        if (tower == null)
        {
            Debug.Log("타워를 못찾음"); 
            return null;
        }
        Util.GetOrAddComponent<Tower_Stat>(tower).Init_Tower(bone, key_Number);
        temp_tower = tower;
        return tower;
    }
    public GameObject Instant_Tower(Tower_Stat.Tower bone, string tower_name, Vector3 pos) 
    {
        GameObject go = Instant_Tower(bone, tower_name);
        go.transform.position = pos;
        return go;
    }

    public void Instant_Tower_Confirm()
    {
        if (temp_tower != null)
        {
            Tower_Stat tower = temp_tower.GetComponent<Tower_Stat>();

            Instant_Bullet_Pool(tower.myTower.bullet_type, tower.myTower.PoolCount);

            tower_List.Add(key_Number, tower);
            key_Number++;
            temp_tower = null;
        }
    }


    void Instant_Bullet_Pool(Tower_Base.Bullet_Type bullet, int count)
    {
        GameManager.m_Resource.Create_Pool_Init($"Bullet/{bullet.ToString()}", count);
    }


    //public GameObject Instant_Bullet(string bul_name, Vector3 pos_start, int damage)
    //{
    //    GameObject bul = GameManager.m_Resource.Instant_Prefab($"Bullet/{bul_name}", pos_start, Bullet_Root.transform);
    //    Util.FindChild<Bullet_Base>(bul)._damage = damage;
    //    return bul;
    //}


    //public GameObject Instant_Bullet(string bul_name, Tower_Stat.Bullet_Type bullet_type,  Vector3 pos_start, Vector3 pos_destination, 
    //    float bul_speed, Bullet_Move.Move_Type move_type = Bullet_Move.Move_Type.Straight, float lifeTime = 100)
    //{
    //    GameObject bul = GameManager.m_Resource.Instant_Prefab($"Bullet/{bul_name}", pos_start, Bullet_Root.transform);
    //    Util.GetOrAddComponent<Bullet_Move>(bul).Init_Bullet(pos_destination, bul_speed, move_type, lifeTime);
    //    return bul;
    //}
    public GameObject Instant_Support(string bul_name, Vector3 pos_start) //? 서포트같은거 부를때
    {
        GameObject bul = GameManager.m_Resource.Instant_Prefab($"Bullet/{bul_name}", pos_start, Bullet_Root.transform);
        return bul;
    }





    public GameObject Instant_Bullet(Tower_Base.Bullet_Type bullet, Tower_Base.Attackable attackable, Vector3 pos_start, GameObject target = null) //? 총알생성 베이스함수
    {
        GameObject bul = GameManager.m_Resource.Instant_Prefab($"Bullet/{bullet.ToString()}", pos_start, Bullet_Root.transform);
        bul.GetComponent<Bullet_Base>().Init_Default(target, attackable);
        return bul;
    }
    public GameObject Instant_Bullet(Tower_Base.Bullet_Type bullet, Tower_Base.Attackable attackable, Vector3 pos_start, GameObject target, int damage)
    {
        GameObject bul = Instant_Bullet(bullet, attackable, pos_start, target);
        bul.GetComponent<Bullet_Base>()._damage = damage;
        return bul;
    }
    public GameObject Instant_Bullet(Tower_Base.Bullet_Type bullet, Tower_Base.Attackable attackable, Vector3 pos_start, GameObject target, int damage, string effectName)
    {
        GameObject bul = Instant_Bullet(bullet, attackable, pos_start, target);
        bul.GetComponent<Bullet_Base>()._effect = effectName;
        bul.GetComponent<Bullet_Base>()._damage = damage;
        return bul;
    }







    public GameObject Instant_Bullet_Poison(Vector3 pos_start, GameObject target, float cycle, int count, int damage, GameObject tower)
    {
        GameObject bul = Instant_Bullet(Tower_Base.Bullet_Type.Bullet_Poison, Tower_Base.Attackable.Allrounder, pos_start, target);
        bul.GetComponent<Bullet_Poison>().Init_DOT(cycle, count, damage, tower);
        return bul;
    }
    //public GameObject Instant_Bullet_DOT(Vector3 pos_start, GameObject target, float cycle, int count, Collider2D tower_Range)
    //{
    //    GameObject bul = GameManager.m_Resource.Instant_Prefab($"Bullet/Bullet_DOT", pos_start, Bullet_Root.transform);
    //    //Util.GetOrAddComponent<Bullet_Move>(bul).Init_Bullet(pos_destination, bul_speed, move_type, lifeTime);
    //    bul.GetComponent<Bullet_Base>().Init_Default(target);
    //    bul.GetComponent<Bullet_DOT>().cycleTime = cycle;
    //    bul.GetComponent<Bullet_DOT>().count = count;
    //    bul.GetComponent<Bullet_DOT>().myRange = tower_Range;
    //    return bul;
    //}

    public GameObject Instant_Bullet_Slow (Vector3 pos_start, float effect_Area, float ratio, float duration)
    {
        //GameObject bul = GameManager.m_Resource.Instant_Prefab($"Bullet/Bullet_Slow", pos_start, Bullet_Root.transform);
        GameObject bul = Instant_Bullet(Tower_Base.Bullet_Type.Bullet_Slow, Tower_Base.Attackable.Allrounder, pos_start);
        bul.GetComponent<Bullet_Slow>().Init_Slow(ratio, duration);
        bul.transform.localScale = Vector3.one * effect_Area * 0.01f;
        return bul;
    }
    public GameObject Instant_Bullet_Buff(Vector3 pos_start, float effect_Area, float ratio, float duration)
    {
        GameObject bul = Instant_Bullet(Tower_Base.Bullet_Type.Bullet_Buff, Tower_Base.Attackable.Buff, pos_start);
        bul.GetComponent<Bullet_Buff>().Init_Buff(ratio, duration);
        bul.transform.localScale = Vector3.one * effect_Area * 0.01f;
        return bul;
    }


    public GameObject Instant_Bullet_Splash(Vector3 pos_start, float area_Size, int damage_area, GameObject target)
    {
        GameObject bul = Instant_Bullet(Tower_Base.Bullet_Type.Bullet_Splash, Tower_Base.Attackable.Allrounder, pos_start, target);
        bul.GetComponent<Bullet_Base>()._damage = damage_area;
        bul.GetComponent<Bullet_Splash>().Init_boom(damage_area , area_Size);
        return bul;
    }


    public GameObject Instant_Bullet_Arcane(Vector3 pos_start, List<Collider2D> targetList, int damage, int bounce_Number)
    {
        GameObject bul = 
            Instant_Bullet(Tower_Base.Bullet_Type.Bullet_Arcane, Tower_Base.Attackable.Allrounder, pos_start, targetList[0].gameObject, damage, "Arcane_Explosion");
        bul.GetComponent<Bullet_Arcane>().Arcane_Initialize(targetList, bounce_Number);
        return bul;
    }

    public GameObject Instant_Bullet_Water(Vector3 pos_start, List<Collider2D> targetList, int damage, int quantity)
    {
        int angle = 360 / quantity;

        for (int i = 0; i < quantity; i++)
        {
            GameObject bul = Instant_Bullet(Tower_Base.Bullet_Type.Bullet_Water, Tower_Base.Attackable.Allrounder, pos_start);
            bul.GetComponent<Bullet_Water>().transform.rotation = Quaternion.Euler(0, 0, angle * i);
            bul.GetComponent<Bullet_Base>()._effect = "Water_Explosion";
            bul.GetComponent<Bullet_Base>()._damage = damage;
        }
        SoundManager.Instance.PlaySound($"Effect/Shot/Water");

        return null;
    }

    public GameObject Instant_Bullet_Lightning(Vector3 pos_start, GameObject target, int damage, string effectName, int bounce)
    {
        GameObject bul = Instant_Bullet(Tower_Base.Bullet_Type.Bullet_Lightning, Tower_Base.Attackable.Ground, pos_start, target);
        bul.GetComponent<Bullet_Base>()._effect = effectName;
        bul.GetComponent<Bullet_Base>()._damage = damage;
        bul.GetComponent<Bullet_Lightning>().Bounce_initialize(bounce);
        return bul;
    }











    #region Tower_Status
    public List<Tower_Status> GetTower_Status (string[] data, int counter)
    {
        List<Tower_Status> pattern_List = new List<Tower_Status>();
        //for (int i = 0; i < counter; i++)
        //{
        //    string[] temp = data[i].Split(',');
        //    if (!string.IsNullOrEmpty(temp[10]))
        //    {
        //        pattern_List.Add(new Tower_Status(temp));
        //    }
        //}
        //Debug.Log(data.Length);
        for (int i = 0; i < data.Length - 1; i++)
        {
            string[] temp = data[i].Split(',');
            if (!string.IsNullOrEmpty(temp[10]))
            {
                pattern_List.Add(new Tower_Status(temp));
            }
        }
        return pattern_List;
    }


    public class Tower_Status
    {
        //? TowerName=0, Attack_Speed=1(float), Attack_Range=2(int), Attackable=3(int), Bullet_Type=4(int), Bullet_Damage=5(int), Detecting=6(0,1),
        //? ID=10(int),Memo=11(string),12=(float),13=(float),14=(float)

        public string TowerName { get; }
        public float AttackSpeed { get; }
        public int AttackRange { get; }
        public Tower_Base.Attackable Attackable { get; }
        public Tower_Base.Bullet_Type Bullet_Type { get; }
        public int Damage { get; }
        public bool Detecting { get; }
        public int ID { get; }


        public Tower_Status(string[] tower)
        {
            TowerName = tower[0];
            AttackSpeed = float.Parse(tower[1]);
            AttackRange = int.Parse(tower[2]);
            Attackable = (Tower_Base.Attackable)int.Parse(tower[3]);
            Bullet_Type = (Tower_Base.Bullet_Type)int.Parse(tower[4]);
            Damage = int.Parse(tower[5]);

            int det = int.Parse(tower[6]);

            if (det == 1)
            {
                Detecting = true;
            }
            else if (det == 0)
            {
                Detecting = false;
            }

            ID = int.Parse(tower[10]);

            //? 세부 데이터가 있으면 받아오기 Memo가 Detail_Data에 대한 주석
            if (!string.IsNullOrEmpty(tower[11]))
            {
                Memo_Detail = tower[11];
                Detail_Count = int.Parse(tower[11].Substring(0, 1));
                Detail_Data = new float[Detail_Count];
                for (int i = 0; i < Detail_Count; i++)
                {
                    Detail_Data[i] = float.Parse(tower[12 + i]);
                }
                Debug.Log($"Detail_Data exist : {TowerName} : {Detail_Count} : {Memo_Detail}");
            }
        }

        //? Detail

        public string Memo_Detail { get; }
        public int Detail_Count { get; }
        public float[] Detail_Data { get; set; }
    }
    #endregion


    #region Tower_Info
    public Dictionary<string, Tower_Info> GetTower_Info(string[] data, int counter)
    {
        Dictionary<string, Tower_Info> info_Dic = new Dictionary<string, Tower_Info>();

        //for (int i = 0; i < counter; i++)
        //{
        //    string[] temp = data[i].Split(',');
        //    var tw = new Tower_Info(temp);
        //    info_Dic.Add(tw.Name, tw);
        //}

        for (int i = 0; i < data.Length - 1; i++)
        {
            string[] temp = data[i].Split(',');
            if (!string.IsNullOrEmpty(temp[1]))
            {
                var tw = new Tower_Info(temp);
                info_Dic.Add(tw.Name, tw);
            }
        }
        return info_Dic;
    }

    public class Tower_Info
    {
        //? Name=0  ID=1    Cost=2  Upgrade=3	Info=4	Evolution=5
        public string Name { get; }
        public int ID { get; }
        public int Cost { get; }
        public int[] Upgrade { get; }
        public string Info { get; }
        public int Evolution { get; } //? 풀업그레이드시 진화가능한 타워 ID 를 반환해줄건데 아직 구현안함


        public Tower_Info(string[] tower)
        {
            Name = tower[0];
            ID = int.Parse(tower[1]);
            Cost = int.Parse(tower[2]);
            Upgrade = new int[5];            
            var v = tower[3].Split(':');
            for (int i = 0; i < 5; i++) { Upgrade[i] = int.Parse(v[i]); }
            Info = tower[4];


            //Evolution = int.Parse(tower[5]);
        }
    }

    #endregion
}
