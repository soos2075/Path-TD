using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
public class Tower_Stat : Tower_Base
{
    public int Dic_KeyNumber { get; private set; } //? TowerManager에서 관리할 타워의 사전키. 짓는 순서에 따라 부여되는 ID로 보면 될듯
    public TowerCode Tower_Code { get; private set; } //? 타워의 고유 넘버. CSV데이터테이블에서 가져올 번호이며 변하지않음.
    public Tower myTower { get; private set; }
    public int Level { get; set; }
    public int AccumCoin { get; set; }


    public Vector2Int Position { get; set; } //? 타워의 위치



    [SerializeField] float attack_Speed; //? 공격속도 - 높을수록 빠름
    [SerializeField] float attack_Range; //? 공격사거리 - 높을수록 넓음
    float Attack_Range { get { return attack_Range * 0.01f; } }

    [SerializeField] Attackable attackable; //? 공격가능대상

    public bool detecting; //? 본인이 디텍팅기능을 가졌는지
    public bool Buff_Basic { get; set; } = false;


    [SerializeField] List<Collider2D> col_List;


    Transform tr; //? 공격범위 알려주는 Sprite Image
    Vector3 tr_default;
    CircleCollider2D rg; //? 공격범위가진타워

    [SerializeField] private bool enemy_Check; // 적이 유효범위내에 들어왔는지 여부 or 타워의 활성화 여부

    public bool confirm_Check = false; //? 확실히 지어졌는지 체크 // 처음 지은 이후에는 타워가 제거되었는지 활성화체크용으로도 씀


    public void Init_Tower(Tower tower, int dic_key)
    {
        tower.Origin = gameObject;

        Dic_KeyNumber = dic_key;
        myTower = tower;
        Tower_Code = (TowerCode)tower.TowerNumber_CSV;

        attack_Speed = tower.attack_Speed;
        attack_Range = tower.attack_Range;
        attackable = tower.attackable;
        detecting = tower.detecting;


        //? 다형성 이용하기. Self인터페이스를 상속받았을때만 실행되는 부분
        I_SelfDisappear iSelf = (myTower as I_SelfDisappear);
        if (iSelf != null)
        {
            Debug.Log("I_SelfDisappear_Init");
            iSelf.Disappear(gameObject.GetComponentInChildren<CircleCollider2D>());
        }
    }
    #region 타워스탯복구 - 지금은 안씀
    //public void RestoreStat (int _lev)
    //{
    //    Tower_Builder tb = GameManager.Instance.GetComponent<Tower_Builder>();

    //    myTower.level = 0;
    //    Level = 0;

    //    transform.GetComponentInChildren<Tower_Effect>().Init_Dot();

    //    AccumCoin = tb.tw_Info[Define.TowerName[(int)Tower_Code]].Cost;

    //    myTower.attack_Range = tb.tw_Status[(int)Tower_Code].AttackRange;
    //    attack_Range = myTower.attack_Range;

    //    myTower.attack_Speed = tb.tw_Status[(int)Tower_Code].AttackSpeed;
    //    attack_Speed = myTower.attack_Speed;

    //    myTower.damage = tb.tw_Status[(int)Tower_Code].Damage;
    //    myTower.damagePlus = 0;

    //    myTower.detecting = tb.tw_Status[(int)Tower_Code].Detecting;



    //    I_Detail iDetail = (myTower as I_Detail);
    //    if (iDetail != null)
    //    {
    //        iDetail.RestoreData();
    //        Debug.Log("디테일 복구");
    //    }

    //    for (int i = 0; i < _lev; i++)
    //    {
    //        AccumCoin += tb.tw_Info[Define.TowerName[(int)Tower_Code]].Upgrade[Level];
    //        Upgrade_Tower();
    //    }
    //}
    #endregion

    public void Upgrade_Tower ()
    {
        if (myTower.level >= 5)
        {
            Debug.Log("최대 업그레이드 입니다.");
            return;
        }
        myTower.Upgrade();
        attack_Speed = myTower.attack_Speed;
        attack_Range = myTower.attack_Range;
        tr.transform.localScale = tr_default * Attack_Range;
        Level = myTower.level;

        transform.GetComponentInChildren<Tower_Effect>().Add_Dot();
    }


    public void Confirm_Init() //? 짓는게 확정되었을 때 부르는 함수 
    {
        confirm_Check = true;
        InvisibleRange();
        GetComponent<BoxCollider2D>().enabled = true;


        if ((myTower as PoisonTower) != null)
        {
            StartCoroutine(Shoot_Poison());
        }
        else
        {
            StartCoroutine(Shoot_Refeat());
        }

        I_Buff iSelf = (myTower as I_Buff);
        if (iSelf != null)
        {
            transform.GetChild(2).gameObject.SetActive(true);
        }
        I_AddCoin iCoin = (myTower as I_AddCoin);
        if (iCoin != null)
        {
            //GameManager.m_Enemy.E_WaveOver += WaveOverTowerCallBack;
            GameManager.m_Enemy.WaveOverEventList.Add(WaveOverTowerCallBack);
        }
    }
    public void Remove_Event()
    {
        I_AddCoin iCoin = (myTower as I_AddCoin);
        if (iCoin != null)
        {
            //GameManager.m_Enemy.E_WaveOver -= WaveOverTowerCallBack;
            GameManager.m_Enemy.WaveOverEventList.Remove(WaveOverTowerCallBack);
        }
    }

    public void PosSetting ()
    {
        tr = transform.Find("_Range_Circle");
        tr_default = tr.transform.localScale;
        tr.transform.localScale = tr.transform.localScale * Attack_Range;
        GetComponent<BoxCollider2D>().enabled = false;

        Position = new Vector2Int((int)transform.position.x, (int)transform.position.y);
    }



    [System.Obsolete] void TestFuntion() //? 타워 개별 테스트할 때 쓰는 함수
    {
        Init_Tower(new LazerTower(attack_Speed, attack_Range, attackable, Bullet_Type.Bullet_Lazer, 100, false), 1);
        Confirm_Init();
    }



    EnemyManager.EnemyPriorityPointComparer comparer; //? 타겟 우선순위 비교시 필요
    EnemyManager.DistanceEnemyAndTowerComparer comparer_Distance_Tower; //? 타워와 적과 거리비교
    EnemyManager.DistanceEnemyAndGoalComparer comparer_Distance_Goal;  //? 타워와 골과의 거리비교(실제로는 이동거리로 판단)
    private void Awake()
    {
        comparer = new EnemyManager.EnemyPriorityPointComparer();
        comparer_Distance_Tower = new EnemyManager.DistanceEnemyAndTowerComparer();
        comparer_Distance_Goal = new EnemyManager.DistanceEnemyAndGoalComparer();
    }

    void Start()
    {
        //tr = transform.GetChild(1); //? 차라리 Find류 함수로 하는게 나을수도있고 그냥 이대로 해도 상관없을수도있고... 암튼 표시
        //PosSetting();
        //InvisibleRange();

        //TestFuntion();
    }
    void Update()
    {
        if (!confirm_Check) //? 건설 확정이 안됐으면 바로 리턴
        {
            return;
        }

        if (GameManager.Instance.PlayState != Define.PlayMode.Play) //? Play상태가 아니면 바로 리턴
        {
            enemy_Check = false;
            return;
        }



        col_List.Clear();

        switch (attackable)
        {
            case Attackable.Ground:
                Collider2D[] col_temp1 = Physics2D.OverlapCircleAll(transform.position, Attack_Range, LayerMask.GetMask("Enemy"));
                foreach (Collider2D item in col_temp1)
                {
                    if (item.GetComponent<EnemyMove>().move_State == EnemyMove.Move_State.Ground || item.GetComponent<EnemyMove>().move_State == EnemyMove.Move_State.Alternative)
                    {
                        if (item.GetComponent<EnemyMove>().sight_State == EnemyMove.Sight_State.Visible || detecting)
                        {
                            col_List.Add(item);
                        }
                    }
                }
                break;
            case Attackable.Air:
                Collider2D[] col_temp2 = Physics2D.OverlapCircleAll(transform.position, Attack_Range, LayerMask.GetMask("Enemy"));
                foreach (Collider2D item in col_temp2)
                {
                    if (item.GetComponent<EnemyMove>().move_State == EnemyMove.Move_State.Air || item.GetComponent<EnemyMove>().move_State == EnemyMove.Move_State.Alternative)
                    {
                        if (item.GetComponent<EnemyMove>().sight_State == EnemyMove.Sight_State.Visible || detecting)
                        {
                            col_List.Add(item);
                        }
                    }
                }
                break;
            case Attackable.Allrounder:
                Collider2D[] col_temp3 = Physics2D.OverlapCircleAll(transform.position, Attack_Range, LayerMask.GetMask("Enemy"));
                foreach (Collider2D item in col_temp3)
                {
                    if (item.GetComponent<EnemyMove>().sight_State == EnemyMove.Sight_State.Visible || detecting)
                    {
                        col_List.Add(item);
                    }
                }
                break;

            case Attackable.Buff:
                Collider2D[] col_temp4 = Physics2D.OverlapCircleAll(transform.position, Attack_Range, LayerMask.GetMask("Tower"));
                foreach (Collider2D item in col_temp4)
                {
                    col_List.Add(item);
                }
                break;

            case Attackable.OnlyAir:
                Collider2D[] col_temp5 = Physics2D.OverlapCircleAll(transform.position, Attack_Range, LayerMask.GetMask("Enemy"));
                foreach (Collider2D item in col_temp5)
                {
                    if (item.GetComponent<EnemyMove>().move_State == EnemyMove.Move_State.Air)
                    {
                        if (item.GetComponent<EnemyMove>().sight_State == EnemyMove.Sight_State.Visible || detecting)
                        {
                            col_List.Add(item);
                        }
                    }
                }
                break;

            case Attackable.OnlySP:
                Collider2D[] col_temp6 = Physics2D.OverlapCircleAll(transform.position, Attack_Range, LayerMask.GetMask("Enemy"));
                foreach (Collider2D item in col_temp6)
                {
                    if (item.GetComponent<EnemyMove>().Shield == true)
                    {
                        if (item.GetComponent<EnemyMove>().sight_State == EnemyMove.Sight_State.Visible || detecting)
                        {
                            col_List.Add(item);
                        }
                    }
                }
                break;
            case Attackable.Non:
                break;
        }

        if (col_List.Count > 0)
        {
            enemy_Check = true;
            //Debug.Log(myCol2.Length);

            switch (Tower_Code)
            {
                //case TowerCode.FireTower:
                //    break;

                //case TowerCode.LazerTower:
                //    break;

                //case TowerCode.SplashTower:
                //    break;

                //case TowerCode.MultiTower:
                //    break;

                case TowerCode.PoisonTower:
                    comparer_Distance_Tower.InitTowerTransform(Position); //? 거리가 가까울수록 (작을수록) 오름차순 
                    col_List.Sort(comparer_Distance_Tower);
                    break;

                //case TowerCode.SpiderTower:
                //    break;

                //case TowerCode.SlowTower:
                //    break;

                case TowerCode.BuffTower:
                    break;

                case TowerCode.SpearTower:
                    col_List.Sort(comparer_Distance_Goal); //? 몹의 이동거리가 높을수록 우선순위(골에 가까울수록)
                    break;

                //case TowerCode.AirTower:
                //    break;

                //case TowerCode.ThunderTower:
                //    break;

                case TowerCode.ArcaneTower:
                    comparer_Distance_Tower.InitTowerTransform(Position); //? 거리가 가까울수록 (작을수록) 오름차순 
                    col_List.Sort(comparer_Distance_Tower);
                    break;

                //case TowerCode.WaterTower:
                //    break;

                //case TowerCode.LightningTower:
                //    break;

                case TowerCode.CoinTower:
                    break;

                default: //? 위의 분기문을 안탔을땐 얘를 읽음
                    col_List.Sort(comparer); //? EnemyMove.PriorityPoint 순으로 리스트 정렬 (내림차순) 0번째에 젤높은점수가오고 마지막이 젤 낮은점수
                    //col_List.Sort((a,b) => a.GetComponent<EnemyMove>().PriorityPoint.CompareTo(b.GetComponent<EnemyMove>().PriorityPoint)) ;
                    //? 위의 람다식방법으로도 가능함. 여기서 인자는 col_List의 요소이고(여기선 Collider2D) 변수이름만 넣어주면 되는거인듯.
                    break;
            }

        }
        else
            enemy_Check = false;

    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(transform.position, Attack_Range);
    //}



    IEnumerator Shoot_Refeat ()
    {
        while (enemy_Check)
        {
            myTower.Shoooot(transform.position, col_List);
            yield return new WaitForSeconds(attack_Speed);
        }

        //Debug.Log("while문 빠져나옴");
        yield return new WaitWhile(() => !enemy_Check);
        //Debug.Log("다시시작");
        StartCoroutine(Shoot_Refeat());
    }

    //? Poison 전용 // 나중에 소환 - 재소환 말고 타겟만 바꿔주는용도로 사용가능할듯
    Bullet_Poison poison;
    IEnumerator Shoot_Poison()
    {
        PoisonTower tw = (myTower as PoisonTower);
        while (enemy_Check)
        {
            if (!poison)
            {
                poison = tw.PoisonShot(transform.position, col_List);
            }
            else if (tw.upgradeCheck)
            {
                //GameManager.m_Resource.Disable_Prefab(poison.gameObject);
                poison.StopParticle();
                poison = tw.PoisonShot(transform.position, col_List);
                tw.upgradeCheck = false;
            }
            poison.PlayParticle(col_List[0].gameObject);
            yield return new WaitForSeconds(attack_Speed);
        }
        if (poison)
        {
            poison.StopParticle();
        }

        yield return new WaitWhile(() => !enemy_Check);
        StartCoroutine(Shoot_Poison());
    }

    //? CallBack 함수 전용. 웨이브 끝날때 부르는 용도
    void WaveOverTowerCallBack()
    {
        I_AddCoin iCoin = (myTower as I_AddCoin);
        if (iCoin != null)
        {
            int towerCount = 0;
            Collider2D[] twList = Physics2D.OverlapCircleAll(transform.position, Attack_Range, LayerMask.GetMask("Tower"));
            foreach (Collider2D item in twList)
            {
                if (item.GetComponent<Tower_Stat>().Tower_Code != TowerCode.CoinTower)
                {
                    towerCount++;
                    var coin = GameManager.m_Resource.Instant_Prefab("UI/Animation/CoinTowerText", 
                        item.transform.position, GameObject.FindGameObjectWithTag("Canvas_World").transform);

                    coin.GetComponent<UI_CoinTowerText>().SetText(iCoin.Coin);
                }
            }
            iCoin.AddTowerCoin(towerCount);
            Debug.Log($"{towerCount}개의 타워 감지.");
            GameManager.m_Resource.Instant_Prefab("UI/Animation/CoinTowerActive", transform.position, GameObject.FindGameObjectWithTag("Canvas_World").transform);
        }
    }


    public void VisibleRange ()
    {
        //tr.gameObject.SetActive(true);
        tr.GetComponent<SpriteRenderer>().enabled = true;
    }
    public void InvisibleRange()
    {
        //tr.gameObject.SetActive(false);
        tr.GetComponent<SpriteRenderer>().enabled = false;
    }



    //? ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    #region 타워 클래스 정보
    public class Tower
    {
        #region 타워 기본 Stat
        public float attack_Speed; // 공격속도
        public float attack_Range; // 공격 사거리
        public Attackable attackable;
        public Bullet_Type bullet_type;
        public bool detecting; //? 감지능력 유무


        public int Damage { get { return damage + damagePlus; } protected set { damage = value; } }
        public int damage;
        public int damagePlus;

        public int TowerNumber_CSV { get; set; }
        public int PoolCount { get; set; }
        public GameObject Origin;


        public Attack_Type attack_Type;
        public Attack_Property element;

        protected void Init_essential(float at_speed, float at_range, Attackable at_able = Attackable.Allrounder, Bullet_Type bul_type = Bullet_Type.Bullet_Basic, int dam = 0, bool detect = false)
        {
            attack_Speed = at_speed;
            attack_Range = at_range;
            attackable = at_able;
            bullet_type = bul_type;
            damage = dam;
            detecting = detect;
        }
        protected void Init_ID_PoolCount (int num, int poolCount)
        {
            TowerNumber_CSV = num;
            PoolCount = poolCount;
        }

        protected void Shoot(Vector3 pos, List<Collider2D> cols, int multi = 1, int damage = 0, string effectName = null, string shotSound = null)
        {
            for (int i = 0; i < cols.Count; i++)
            {
                if (multi == i)
                {
                    break;
                }
                GameManager.m_Tower.Instant_Bullet(bullet_type, attackable, pos, cols[i].gameObject, damage, effectName);
                if (shotSound != null)
                {
                    SoundManager.Instance.PlaySound($"Effect/Shot/{shotSound}");
                }
                //Debug.Log("탄생성-발사");
            }
        }
        //? 여기까지가 원래 밖에 있던 코드
        #endregion

        public virtual void Init_detail()
        {
            Init_ID_PoolCount(0,1);
        }
        public Tower()
        {
            Init_detail();
            //Debug.Log("베이스 생성자");
        }

        public virtual void Shoooot(Vector3 pos, List<Collider2D> cols, int multi = 1)
        {
            Shoot(pos, cols, 1, Damage);
            //Debug.Log("Attack_Normal");
        }

        public int level = 0;
        public virtual void Upgrade ()
        {
            attack_Speed *= 0.9f;
            attack_Range *= 1.05f;
            damage += (int)(damage * 0.2f);
            level++;
        }
    }



    public class FireTower : Tower
    {
        public FireTower(float at_speed, float at_range, Attackable at_able, Bullet_Type bul_type, int dam, bool det) : base()
        {
            Init_essential(at_speed, at_range, at_able, bul_type, dam, det);
        }
        public override void Init_detail()
        {
            Init_ID_PoolCount(0, 10);
            Debug.Log("Init_FireTower");
        }
        public override void Shoooot(Vector3 pos, List<Collider2D> cols, int multi = 1)
        {
            Shoot(pos, cols, 1, Damage, "Fire_Explosion");
            //SoundManager.Instance.PlaySound("Effect/Fire");
        }

        public override void Upgrade()
        {
            attack_Speed *= 0.95f;
            attack_Range *= 1.05f;
            damage += (int)(damage * 0.45f);
            level++;
        }
    }
    public class LazerTower : Tower
    {
        public LazerTower(float at_speed, float at_range, Attackable at_able, Bullet_Type bul_type, int dam, bool det) : base()
        {
            Init_essential(at_speed, at_range, at_able, bul_type, dam, det);
        }
        public override void Init_detail()
        {
            Init_ID_PoolCount(1, 1);
            Debug.Log("Init_LazerTower");
        }
        public override void Shoooot(Vector3 pos, List<Collider2D> cols, int multi = 1)
        {
            for (int i = 0; i < cols.Count; i++)
            {
                if (multi == i) { break; }
                GameManager.m_Tower.Instant_Bullet(bullet_type,attackable, pos, cols[i].gameObject, Damage);
                SoundManager.Instance.PlaySound("Effect/Shot/Light", volume:0.5f);
            }
        }
        public override void Upgrade()
        {
            attack_Speed *= 0.95f;
            attack_Range *= 1.05f;
            damage += (int)(damage * 0.4f);
            level++;
        }
    }

    public class SplashTower : Tower, I_Detail
    {
        public SplashTower(float at_speed, float at_range, Attackable at_able, Bullet_Type bul_type, int dam, bool det) : base()
        {
            Init_essential(at_speed, at_range, at_able, bul_type, dam, det);
        }
        public float effectArea;
        protected float effectArea_Origin;

        public void Detail_Data(float value1 = 0, float value2 = 0, float value3 = 0, float value4 = 0)
        {
            effectArea = value1;
            effectArea_Origin = value1;
        }
        public void RestoreData()
        {
            effectArea = effectArea_Origin;
        }
        public override void Init_detail()
        {
            Init_ID_PoolCount(2, 5);
            Debug.Log("Init_SplashTower");
        }

        public override void Upgrade()
        {
            attack_Speed *= 0.9f;
            attack_Range *= 1.05f;
            damage += (int)(damage * 0.25f);
            level++;
            effectArea += 0.02f;
        }

        public override void Shoooot(Vector3 pos, List<Collider2D> cols, int multi = 1)
        {
            GameManager.m_Tower.Instant_Bullet_Splash(pos, effectArea, Damage, cols[0].gameObject);
        }
    }


    public class MultishotTower : Tower, I_MultiShot, I_Detail
    {
        public MultishotTower(float at_speed, float at_range, Attackable at_able, Bullet_Type bul_type, int dam, bool det) : base()
        {
            Init_essential(at_speed, at_range, at_able, bul_type, dam, det);
        }


        public int Multi_Numbers { get; set; }
        protected int multi_Numbers_Origin;

        public void Detail_Data(float value1 = 0, float value2 = 0, float value3 = 0, float value4 = 0)
        {
            Multi_Numbers = (int)value1;
            multi_Numbers_Origin = (int)value1;
        }
        public void RestoreData()
        {
            Multi_Numbers = multi_Numbers_Origin;
        }

        public override void Init_detail()
        {
            Init_ID_PoolCount(3, 20);
            Debug.Log("Init_MultiTower");
        }
        public override void Shoooot(Vector3 pos, List<Collider2D> cols, int multi = 1)
        {
            Shoot(pos, cols, Multi_Numbers,Damage, effectName: "Multi_Explosion");
        }
        public override void Upgrade()
        {
            attack_Speed *= 0.9f;
            attack_Range *= 1.05f;
            damage += (int)(damage * 0.15f);
            level++;
            Multi_Numbers++;
        }
    }

    public class PoisonTower : Tower, I_DOT, I_Detail
    {
        public PoisonTower(float at_speed, float at_range, Attackable at_able, Bullet_Type bul_type, int dam, bool det) : base()
        {
            Init_essential(at_speed, at_range, at_able, bul_type, dam, det);
        }

        public float CycleTime { get; set; }
        protected float CycleTime_Origin;
        public int Count { get; set; }
        protected int Count_Origin;

        public bool upgradeCheck;

        public void Detail_Data(float value1 = 0, float value2 = 0, float value3 = 0, float value4 = 0)
        {
            CycleTime = value1;
            Count = (int)value2;
            CycleTime_Origin = value1;
            Count_Origin = (int)value2;
        }
        public void RestoreData()
        {
            CycleTime = CycleTime_Origin;
            Count = Count_Origin;
        }

        public override void Init_detail()
        {
            TowerNumber_CSV = 4;
            Debug.Log("Init_PoisonTower");
        }

        public Bullet_Poison PoisonShot (Vector3 pos, List<Collider2D> cols, int multi = 1)
        {
            return GameManager.m_Tower.Instant_Bullet_Poison(pos, cols[0].gameObject, CycleTime, Count, Damage, Origin).GetComponent<Bullet_Poison>();
        }

        public override void Shoooot(Vector3 pos, List<Collider2D> cols, int multi = 1)
        {
            Debug.Log("잘못된 공격입니다 : PoisonTower");
        }

        public override void Upgrade()
        {
            level++;
            damage += (int)(damage * 0.5f);
            upgradeCheck = true;
        }
    }



    public class SpiderTower : Tower, I_MultiShot, I_Detail
    {
        public SpiderTower(float at_speed, float at_range, Attackable at_able, Bullet_Type bul_type, int dam, bool det) : base()
        {
            Init_essential(at_speed, at_range, at_able, bul_type, dam, det);
        }
        public int Multi_Numbers { get; set; }
        int multi_Numbers_origin;
        public override void Init_detail()
        {
            Init_ID_PoolCount(5, 10);
            Debug.Log("Init_SpiderwebTower");
        }
        public override void Shoooot(Vector3 pos, List<Collider2D> cols, int multi = 1)
        {
            Shoot(pos, cols, Multi_Numbers, Damage);
        }

        public override void Upgrade()
        {
            attack_Range *= 1.05f;
            damage += (int)(damage * 0.25f);
            attack_Speed *= 0.95f;
            level++;
            if (level == 3 || level == 5)
            {
                Multi_Numbers++;
            }
        }

        public void Detail_Data(float value1 = 0, float value2 = 0, float value3 = 0, float value4 = 0)
        {
            Multi_Numbers = (int)value1;
            multi_Numbers_origin = (int)value1;
        }

        public void RestoreData()
        {
            Multi_Numbers = multi_Numbers_origin;
        }
    }



    public class SlowTower : Tower, I_Detail
    {
        public SlowTower(float at_speed, float at_range, Attackable at_able, Bullet_Type bul_type, bool det) : base()
        {
            Init_essential(at_speed, at_range, at_able, bul_type, detect: det);
        }

        float Slow_ratio;
        protected float Slow_ratio_Origin;

        float Slow_duration;
        protected float Slow_duration_Origin;
        public void Detail_Data(float value1 = 0, float value2 = 0, float value3 = 0, float value4 = 0)
        {
            Slow_ratio = value1;
            Slow_ratio_Origin = value1;

            Slow_duration = value2;
            Slow_duration_Origin = value2;
        }
        public void RestoreData()
        {
            Slow_ratio = Slow_ratio_Origin;
            Slow_duration = Slow_duration_Origin;
        }

        public override void Init_detail()
        {
            TowerNumber_CSV = 6;
            Debug.Log("Init_SlowTower");
        }
        public override void Shoooot(Vector3 pos, List<Collider2D> cols, int multi = 1)
        {
            GameManager.m_Tower.Instant_Bullet_Slow(pos, attack_Range, Slow_ratio, Slow_duration);
        }

        public override void Upgrade()
        {
            base.Upgrade();
            Slow_ratio -= 0.05f;
        }
    }




    public class ObserverTower : Tower, I_Buff, I_Detail
    {
        public ObserverTower(float at_speed, float at_range, Attackable at_able, Bullet_Type bul_type, bool det) : base()
        {
            Init_essential(at_speed, at_range, at_able, bul_type, detect: det);
        }
        public void Buff()
        {
            //GameManager.m_Resource.Instant_Prefab("Tower/Support/Buff_Collider");
        }

        float Buff_DamageRatio;
        protected float Buff_DamageRatio_Origin;
        float Buff_Duration;
        protected float Buff_Duration_Origin;
        public void Detail_Data(float value1 = 0, float value2 = 0, float value3 = 0, float value4 = 0)
        {
            Buff_DamageRatio = value1;
            Buff_DamageRatio_Origin = value1;
            Buff_Duration = value2;
            Buff_Duration_Origin = value2;
        }
        public void RestoreData()
        {
            Buff_DamageRatio = Buff_DamageRatio_Origin;
            Buff_Duration = Buff_Duration_Origin;
        }

        public override void Init_detail()
        {
            TowerNumber_CSV = 7;
            Debug.Log("Init_ObserverTower");
        }
        public override void Shoooot(Vector3 pos, List<Collider2D> cols, int multi = 1)
        {
            Debug.Log("버프공격!");
            GameManager.m_Tower.Instant_Bullet_Buff(pos, attack_Range, Buff_DamageRatio, Buff_Duration);
        }

        public override void Upgrade()
        {
            Buff_DamageRatio += Buff_DamageRatio * 0.1f;
            //Buff_DamageRatio += 0.08f;
            Buff_Duration += 1f;
            //attack_Range *= 1.05f;
            level++;
        }
    }

    public class SpearTower : Tower
    {
        public SpearTower(float at_speed, float at_range, Attackable at_able, Bullet_Type bul_type, int dam, bool det) : base()
        {
            Init_essential(at_speed, at_range, at_able, bul_type, dam, det);
        }

        public override void Init_detail()
        {
            Init_ID_PoolCount(8, 3);
        }

        GameObject targeting;
        int attackCount;

        public override void Shoooot(Vector3 pos, List<Collider2D> cols, int multi = 1)
        {
            if (targeting == null)
            {
                targeting = cols[0].gameObject;
            }
            else if (targeting == cols[0].gameObject)
            {
                attackCount++;
                if (attackCount > 4) attackCount = 4;
            }
            else
            {
                targeting = cols[0].gameObject;
                attackCount = 0;
            }

            GameManager.m_Tower.Instant_Bullet(bullet_type, attackable, pos, cols[0].gameObject, Damage + (int)(attackCount * 0.25 * Damage), "Spear_Explosion");
        }

        public override void Upgrade()
        {
            attack_Speed *= 0.85f;
            attack_Range *= 1.05f;
            damage += (int)(damage * 0.5f);
            level++;
        }
    }

    public class AirTower : Tower
    {
        public AirTower(float at_speed, float at_range, Attackable at_able, Bullet_Type bul_type, int dam, bool det) : base()
        {
            Init_essential(at_speed, at_range, at_able, bul_type, dam, det);
        }

        public override void Init_detail()
        {
            Init_ID_PoolCount(9, 5);
        }

        public override void Shoooot(Vector3 pos, List<Collider2D> cols, int multi = 1)
        {
            Shoot(pos, cols, multi, Damage, "Air_Explosion");
        }
    }

    public class ThunderTower : Tower
    {
        public ThunderTower(float at_speed, float at_range, Attackable at_able, Bullet_Type bul_type, int dam, bool det) : base()
        {
            Init_essential(at_speed, at_range, at_able, bul_type, dam, det);
        }

        public override void Init_detail()
        {
            Init_ID_PoolCount(10, 10);
        }

        public override void Shoooot(Vector3 pos, List<Collider2D> cols, int multi = 1)
        {
            Shoot(pos, cols, cols.Count, Damage, shotSound : "Thunder");
        }

        public override void Upgrade()
        {
            //attack_Speed *= 0.85f;
            attack_Range *= 1.1f;
            damage += (int)(damage * 0.5f);
            level++;
        }
    }

    public class ArcaneTower : Tower, I_Detail
    {
        public ArcaneTower(float at_speed, float at_range, Attackable at_able, Bullet_Type bul_type, int dam, bool det) : base()
        {
            Init_essential(at_speed, at_range, at_able, bul_type, dam, det);
        }


        public int Bounce_Numbers { get; set; }

        protected int Bounce_Numbers_Origin;

        public void Detail_Data(float value1 = 0, float value2 = 0, float value3 = 0, float value4 = 0)
        {
            Bounce_Numbers = (int)value1;
            Bounce_Numbers_Origin = (int)value1;
        }
        public void RestoreData()
        {
            Bounce_Numbers = Bounce_Numbers_Origin;
        }

        public override void Init_detail()
        {
            Init_ID_PoolCount(11, 5);
            Debug.Log("Init_ArcaneTower");
        }
        public override void Shoooot(Vector3 pos, List<Collider2D> cols, int multi = 1)
        {
            GameManager.m_Tower.Instant_Bullet_Arcane(pos, cols, damage, Bounce_Numbers);
        }
        public override void Upgrade()
        {
            base.Upgrade();
            Bounce_Numbers++;
        }
    }

    public class WaterTower : Tower, I_Detail
    {
        public WaterTower(float at_speed, float at_range, Attackable at_able, Bullet_Type bul_type, int dam, bool det) : base()
        {
            Init_essential(at_speed, at_range, at_able, bul_type, dam, det);
        }

        int Bullet_quantity { get; set; }
        int bullet_quantity_origin;

        public override void Init_detail()
        {
            Init_ID_PoolCount(12, 24);
            Debug.Log("Init_WaterTower");
        }
        public override void Shoooot(Vector3 pos, List<Collider2D> cols, int multi = 1)
        {
            GameManager.m_Tower.Instant_Bullet_Water(pos, cols, damage, Bullet_quantity);
        }
        public override void Upgrade()
        {
            attack_Speed *= 0.95f;
            attack_Range *= 1.05f;
            damage += (int)(damage * 0.2f);
            level++;

            if (level == 3 || level == 5)
            {
                Bullet_quantity += 4;
            }
        }

        public void Detail_Data(float value1 = 0, float value2 = 0, float value3 = 0, float value4 = 0)
        {
            Bullet_quantity = (int)value1;
            bullet_quantity_origin = (int)value1;
        }

        public void RestoreData()
        {
            Bullet_quantity = bullet_quantity_origin;
        }
    }

    public class LightningTower : Tower, I_Detail
    {
        public LightningTower(float at_speed, float at_range, Attackable at_able, Bullet_Type bul_type, int dam, bool det) : base()
        {
            Init_essential(at_speed, at_range, at_able, bul_type, dam, det);
        }


        public int Bounce_Numbers { get; set; }

        protected int Bounce_Numbers_Origin;

        public void Detail_Data(float value1 = 0, float value2 = 0, float value3 = 0, float value4 = 0)
        {
            Bounce_Numbers = (int)value1;
            Bounce_Numbers_Origin = (int)value1;
        }
        public void RestoreData()
        {
            Bounce_Numbers = Bounce_Numbers_Origin;
        }

        public override void Init_detail()
        {
            Init_ID_PoolCount(13, 5);
            Debug.Log("Init_LightningTower");
        }
        public override void Shoooot(Vector3 pos, List<Collider2D> cols, int multi = 1)
        {
            GameManager.m_Tower.Instant_Bullet_Lightning(pos, cols[0].gameObject, Damage, "Lightning_Explosion", Bounce_Numbers);
        }
        public override void Upgrade()
        {
            attack_Speed *= 0.95f;
            attack_Range *= 1.05f;
            damage += (int)(damage * 0.15f);
            level++;
            Bounce_Numbers += 2;
        }
    }

    public class CoinTower : Tower, I_AddCoin, I_Detail
    {
        public CoinTower(float at_speed, float at_range, Attackable at_able, Bullet_Type bul_type, int dam, bool det) : base()
        {
            Init_essential(at_speed, at_range, at_able, bul_type, dam, det);
        }

        public int Coin { get; set; }
        int coinOrigin;

        public override void Shoooot(Vector3 pos, List<Collider2D> cols, int multi = 1)
        {
            Debug.Log("공격안함");
        }
        public override void Init_detail()
        {
            Init_ID_PoolCount(14, 0);
            Debug.Log("Init_CoinTower");
        }
        public override void Upgrade()
        {
            Coin += 3;
            attack_Range *= 1.1f;
            level++;
        }

        public void AddTowerCoin(int Quantity)
        {
            GameManager.Instance.AddCoin(Coin * Quantity);
            Debug.Log($"보너스 인컴 = {Coin * Quantity}");
        }

        public void Detail_Data(float value1 = 0, float value2 = 0, float value3 = 0, float value4 = 0)
        {
            Coin = (int)value1;
            coinOrigin = (int)value1;
        }

        public void RestoreData()
        {
            Coin = coinOrigin;
        }
    }


    #endregion



}
