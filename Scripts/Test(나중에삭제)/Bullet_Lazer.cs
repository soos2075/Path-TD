using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Lazer : Bullet_Base
{

    private SpriteRenderer spr_Lazer;
    private CapsuleCollider2D col_Lazer;

    private void Awake()
    {
        _move_Type = Move_Type.Lazer;
    }
    public override void Start()
    {
        spr_Lazer = gameObject.GetComponent<SpriteRenderer>();
        col_Lazer = gameObject.GetComponent<CapsuleCollider2D>();

        base.Start();
        //Debug.Log("start");
        spr_Lazer.enabled = false;
    }


    private void Update() //? 일단은 레이져타입만사용할꺼라 override가 아닌 그냥 아예 대체해버리는걸로 씀
    {
        //Debug.Log("update");
        OnceAngle();
        spr_Lazer.enabled = true;

        spr_Lazer.size = spr_Lazer.size + new Vector2(0, _speed * Time.deltaTime);
        col_Lazer.size += new Vector2(0, _speed * Time.deltaTime);
        col_Lazer.offset += new Vector2(0, _speed/2 * Time.deltaTime);
    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.GetComponent<EnemyMove>().MicroHp += (_damage * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //? 이건 빈칸이라고 없애면 안되고 부모의 TriggerEnter를 호출하지 않기위해 하이딩(오버라이딩이랑은 살짝 다른개념) 하는용
    }
}
