using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_LazerNew : Bullet_Base
{
    //[SerializeField]
    //public GameObject lazerHead;
    private GameObject lazerHead;

    public float scaleSpeed;
    public float offsetSpeed;

    private LineRenderer line;
    private CapsuleCollider2D col_Lazer;


    private void Awake()
    {
        _move_Type = Move_Type.Lazer;
        
    }
    public override void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        col_Lazer = gameObject.GetComponent<CapsuleCollider2D>();

        base.Start();
        OnceAngle();

        //lazerHead = Instantiate(lazerHead, transform.position, transform.rotation);
        //lazerHead = GameManager.m_Resource.Instant_Prefab("Bullet/Support/Support_LazerHead");
        lazerHead = GameManager.m_Tower.Instant_Support("Support/Support_LazerHead", transform.position);
        lazerHead.transform.rotation = transform.rotation;

        OnActive();
    }

    float tempDistance;
    private void Update() //? 일단은 레이져타입만사용할꺼라 override가 아닌 그냥 아예 대체해버리는걸로 씀
    {
        lazerHead.transform.Translate(new Vector2(0, _speed * Time.deltaTime));
        tempDistance += _speed * Time.deltaTime;
        line.SetPosition(1, new Vector3(0, tempDistance, 0));
        col_Lazer.size += new Vector2(0, _speed * Time.deltaTime);
        col_Lazer.offset += new Vector2(0, _speed / 2 * Time.deltaTime);

        float distance = Vector3.Distance(transform.position, lazerHead.transform.position);
        line.material.mainTextureScale = new Vector2(distance / scaleSpeed, 1);
        line.material.mainTextureOffset -= new Vector2(Time.deltaTime * offsetSpeed, 0);

        
        count -= Time.deltaTime;
        if (count < 0.5f)
        {
            line.widthMultiplier = (count / 0.5f);
        }
    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_attackable == Tower_Base.Attackable.Ground && collision.GetComponent<EnemyMove>().move_State == EnemyMove.Move_State.Air)
        {
            return;
        }
        if (_attackable == Tower_Base.Attackable.Air && collision.GetComponent<EnemyMove>().move_State == EnemyMove.Move_State.Ground)
        {
            return;
        }
        collision.GetComponent<EnemyMove>().MicroHp += (_damage * Time.deltaTime * 0.75f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //? 이건 빈칸이라고 없애면 안되고 부모의 TriggerEnter를 호출하지 않기위해 하이딩(오버라이딩이랑은 살짝 다른개념) 하는용
    }


    public float disableTime;
    float count;
    public void OnActive()
    {
        gameObject.SetActive(true);
        StartCoroutine(Disable());
        count = disableTime;
    }
    IEnumerator Disable()
    {
        yield return new WaitForSeconds(disableTime);
        gameObject.SetActive(false);
    }
}
