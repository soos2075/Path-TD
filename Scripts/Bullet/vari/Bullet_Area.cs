using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Area : Bullet_Base
{

    //private int hitNum;

    public override void Start()
    {
        base.Start();
    }
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //hitNum++;
        //Debug.Log(_damage + " 만큼의 피해를 입힘 : " + hitNum + "번째");

        collision.GetComponent<EnemyMove>().Subtract_HP(_damage);

        if (!string.IsNullOrEmpty(_effect))
        {
            GameManager.m_Resource.Instant_Effect(_effect, transform.position);
        }


        gameObject.SetActive(false);

        //if (gameObject.GetComponent<Poolable>() == null)
        //{
        //    gameObject.SetActive(false);
        //}
        //else
        //{
        //    //Debug.Log("총알뒤짐@@@");
        //    GameManager.m_Resource.Disable_Prefab(gameObject);
        //}

    }
}
