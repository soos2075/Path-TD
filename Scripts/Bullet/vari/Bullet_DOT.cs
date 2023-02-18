using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_DOT : Bullet_Base
{
    public bool penetrate_Check = true; //? 관통 유무

    public float cycleTime = 0.5f;
    public int count = 5;

    public Collider2D myRange { get; set; }


    //public override void Start()
    //{
    //    _once = false;
    //}

    //public void Update()
    //{
    //    transform.Translate(Vector3.up * Time.deltaTime * _speed);
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision);
        EnemyMove em;
        if (collision.TryGetComponent<EnemyMove>(out em))
        {
            em.DOT(cycleTime, count, _damage);
            if (!penetrate_Check)
            {
                gameObject.SetActive(false);
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == myRange)
        {
            gameObject.SetActive(false);
        }
    }

}
