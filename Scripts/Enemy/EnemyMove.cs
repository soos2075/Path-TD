using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Navi navi;
    List<Vector3> moveList = new List<Vector3>();

    public float move_Speed;
    float move_Speed_Origin;
    int Movecount = 0;


    [SerializeField] private int priorityPoint = 1000;
    public int PriorityPoint { get { return priorityPoint; } set { priorityPoint = value; } }


    [SerializeField] private int hp; //? 인스펙터에서 보기위한용도고 나중에 없애도됨
    [SerializeField] private int sp; //? 인스펙터에서 보기위한용도고 나중에 없애도됨
    public int Hp { get { return hp; } set { hp = value; } }
    public int SP { get { return sp; } set { sp = value; } }
    public bool Shield
    {
        get
        {
            if (SP > 0) { return true; }
            else { return false; }
        }
    }

    public void Subtract_HP (int damage) //? 앞으로 모든 데미지는 여기서 처리해주는게 좋을듯. 퍼센트로 증감도 여기서 처리가능
    {
        if (Shield)
        {
            SP -= Mathf.Clamp(damage, 1, 25);
        }
        else
        {
            Hp -= damage;
        }
    }

    public void Subtract_SP (int damage)
    {
        if (Shield)
        {
            SP -= damage;
        }
    }

    private float micro;
    public float MicroHp //? 레이저같은 float 데미지를 주는용도
    {
        get { return micro; }
        set { micro = value;
            if (micro > 5)
            {
                Subtract_HP(5);
                micro -= 5;
                if (micro > 5)
                {
                    Subtract_HP(5);
                    micro -= 5;
                }
            }
        }
    }


    public float survivalTime;


    public enum Move_State
    {
        Ground,
        Air,
        Alternative,
    }
    public enum Armor // Light > Medium > Heavy > Light 각각 +-30%의 추가데미지
    {
        Non_Armor,
        Light,
        medium, 
        Heavy,

    }
    public enum Sight_State
    {
        Visible,
        Hiding,
        //NeverSight,

    }

    public Move_State move_State;
    public Armor armor;
    public Sight_State sight_State;


    public bool heal = false;
    public float heal_Ratio = 0.1f;
    private int maxHp;
    private float healTimer;


    void Awake ()
    {
        if (Shield)
        {
            GameManager.m_Resource.Instant_Prefab("UI/Animation/Sp_Bar", transform);
        }
        GameManager.m_Resource.Instant_Prefab("UI/Animation/Hp_Bar", transform);

        maxHp = hp;
        survivalTime = 0;
        move_Speed_Origin = move_Speed;
    }


    void Start()
    {
        Route_Setting();
        if (sight_State == Sight_State.Hiding) //? Hiding유닛 투명하게만들기
        {
            var myColor = GetComponentInChildren<SpriteRenderer>().color;
            GetComponentInChildren<SpriteRenderer>().color = new Color(myColor.r, myColor.g, myColor.b, 0.6f);
        }
    }

    public void Route_Setting ()
    {
        //Hp = 10;
        navi = GameManager.Instance.GetComponent<Navi>();
        //navi = FindObjectOfType<Navi>();
        for (int i = 0; i < navi.moveList.Count; i++)
        {
            moveList.Add(new Vector3(navi.moveList[i].posX, navi.moveList[i].posY));
        }
        StartCoroutine(c_Death_Check());
        StartCoroutine(c_Pass_Check());
    }
    void Update()
    {
        if (Hp > 0)
        {
            survivalTime += Time.deltaTime;
        }

        if (Movecount + 1 < moveList.Count)
        {
            //transform.position += moveList[Movecount + 1] * Time.deltaTime * move_Speed;
            //? 무브포인트로 이동하기
            transform.position = Vector3.MoveTowards(transform.position, moveList[Movecount + 1], Time.deltaTime * move_Speed);

            //? 스프라이트 돌려주기
            if (moveList[Movecount + 1].x - transform.position.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (moveList[Movecount + 1].x - transform.position.x == 0)
            {
                for (int i = Movecount + 2; i < moveList.Count; i++)
                {
                    if (moveList[i].x - transform.position.x > 0)
                    {
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                        break;
                    }
                    else if (moveList[i].x - transform.position.x < 0)
                    {
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                        break;
                    }
                }
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }


            //? 무브포인트 바꿔주기
            float dis = Vector3.Distance(moveList[Movecount + 1], transform.position);
            if (dis < 0.05f)
            {
                Movecount++;
            }
        }


        if (heal && Hp < maxHp)
        {
            healTimer += Time.deltaTime;
            if (healTimer >= 2.5f)
            {
                healTimer = 0;
                Hp += (int)(maxHp * heal_Ratio);
                if (Hp > maxHp)
                {
                    Hp = maxHp;
                }
            }
        }
    }

    IEnumerator c_Death_Check ()
    {
        yield return new WaitUntil(() => Hp <= 0);

        string initial = gameObject.name.Substring(0, 2);
        int level = int.Parse(gameObject.name.Substring(3, 1));
        int score = GameManager.m_Info.ShowMonster_Score(initial);

        float timeBonus = Mathf.Clamp(survivalTime, 5, 25);
        float weightedValue = 5 / timeBonus;
        float result = score * (level + 1) * weightedValue;

        GameManager.Instance.AddScore((int)result);

        //Debug.Log("DeathTrigger = " + gameObject.name);
        DeathTrigger();
    }
    IEnumerator c_Pass_Check()
    {
        yield return new WaitUntil(() => Vector3.Distance(moveList[moveList.Count - 1], transform.position) < 0.05f);

        string initial = gameObject.name.Substring(0, 2);
        GameManager.Instance.SubtractLife(GameManager.m_Info.ShowMonster_Attack(initial));

        //Debug.Log("Pass = " + gameObject.name);
        DeathTrigger();
    }


    public void DeathTrigger()
    {
        //? StopAllCoroutine이 behavior안의 모든 코루틴을 정지시키는데 behavior가 mono가 상속받은 애니까 요 스크립트 안에있는거만 멈추는거인듯
        //? 따라서 여기서 쓰면 위에 Check스크립트 두개만 정지시키는거 --라고 일단 이해하고 버그도 없으니 그렇게 사용
        StopAllCoroutines();
        GameManager.m_Enemy.stage_enemy_Count--;
        gameObject.SetActive(false);
        GameManager.m_Resource.Instant_Effect_Enemy("Default", transform.position);
        SoundManager.Instance.PlaySound("Effect/genericimpact", volume:0.5f);
    }

    #region DOT 시스템
    //private IEnumerator dot_cour;
    private int dotCount;
    private Coroutine co_DOT;

    public void DOT(float cycleTime, int count, int damage)
    {
        Debug.Log("DOT 호출");
        if (co_DOT != null)
        {
            dotCount = 0;
            return;
        }
        co_DOT = StartCoroutine(DOT_System(cycleTime, count, damage));
    }
    IEnumerator DOT_System(float cycleTime, int count, int damage)
    {
        for (dotCount = 0; dotCount < count; dotCount++)
        {
            yield return new WaitForSeconds(cycleTime);
            //Debug.Log(damage + " 만큼의 도트딜" + dotCount + " 번째");
            //Hp -= damage;
            Subtract_HP(damage);
            GameManager.m_Resource.Instant_Effect("DOT_Poison", transform.position);
        }
        co_DOT = null;
    }
    #endregion

    #region 상태변화
    private Coroutine co_Change_State;
    public void AirToGround (float duration)
    {
        if (co_Change_State != null) { return; }
        co_Change_State = StartCoroutine(Change_State(Move_State.Ground, duration));
    }
    public void GroundToAir(float duration)
    {
        if (co_Change_State != null) { return; }
        co_Change_State = StartCoroutine(Change_State(Move_State.Air, duration));
    }
    public void AirToAlternative(float duration)
    {
        if (co_Change_State != null) { return; }
        co_Change_State = StartCoroutine(Change_State(Move_State.Alternative, duration));
    }
    IEnumerator Change_State (Move_State state, float duration)
    {
        GameObject go = GameManager.m_Resource.Instant_Prefab("Bullet/Effect/Spider_Web", transform.position, transform);
        SoundManager.Instance.PlaySound("Effect/Explosion/Spider_Web");

        float pre_speed = move_Speed;
        Move_State pre = move_State;

        move_Speed *= 0.5f;
        move_State = state;
        for (int i = 0; i < duration * 5; i++)
        {
            move_Speed = pre_speed * 0.5f;
            yield return new WaitForSeconds(0.2f);
        }
        //yield return new WaitForSeconds(duration);
        move_Speed = move_Speed_Origin;
        move_State = pre;
        co_Change_State = null;

        //Destroy(go);
        GameManager.m_Resource.Disable_Prefab(go);
    }


    private Coroutine co_Change_Speed;
    private int reset_Count;
    public void Slow_Fix (float speed, float duration)
    {
        if (co_Change_Speed != null) { reset_Count = 0; return; }
        co_Change_Speed = StartCoroutine(Change_Speed_Fix(speed, duration));
    }
    public void Slow_Ratio(float ratio, float duration)
    {
        if (co_Change_Speed != null) { reset_Count = 0; return; }
        co_Change_Speed = StartCoroutine(Change_Speed_Ratio(ratio, duration));
    }
    IEnumerator Change_Speed_Fix (float speed, float duration)
    {
        float pre_speed = move_Speed;

        move_Speed = speed;
        yield return new WaitForSeconds(duration);
        move_Speed = pre_speed;
        co_Change_Speed = null;
    }
    IEnumerator Change_Speed_Ratio(float ratio, float duration)
    {
        float pre_speed = move_Speed;

        for (reset_Count = 0; reset_Count < duration * 5; reset_Count++)
        {
            move_Speed = pre_speed * ratio;
            yield return new WaitForSeconds(0.2f);
        }
        move_Speed = move_Speed_Origin;
        co_Change_Speed = null;
    }
    #endregion


}
