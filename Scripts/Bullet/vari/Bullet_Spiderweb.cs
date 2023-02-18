using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Spiderweb : Bullet_Base
{



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_tracker)
        {
            if (_Target == collision.gameObject)
            {
                //Debug.Log(_damage + " 만큼의 피해를 입힘");
                collision.GetComponent<EnemyMove>().Subtract_HP(_damage);
                //collision.GetComponent<EnemyMove>().AirToGround(5);
                collision.GetComponent<EnemyMove>().AirToAlternative(5);

                GetComponent<StayParticle>().Off_Col();
                //GameManager.m_Resource.Disable_Prefab(gameObject);
                //gameObject.SetActive(false);
            }
            return;
        }
    }
}
