using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Slow : Bullet_Base
{

    public float Bullet_Duration = 0.5f;

    public override void Start()
    {
        base.Start();
        Invoke("ActiveFalse", Bullet_Duration);
    }
    void ActiveFalse()
    {
        Debug.Log("스톱");


        gameObject.SetActive(false);
        GetComponent<CircleCollider2D>().enabled = false;
        //var par = transform.GetChild(0).GetComponentsInChildren<ParticleSystem>();
        //foreach (var item in par)
        //{
        //    item.Stop();
        //}
        //par[1].Clear();
    }




    [SerializeField] private float slow_Ratio;
    [SerializeField] private float slow_Duration;

    public void Init_Slow (float rat, float dur)
    {
        slow_Ratio = rat;
        slow_Duration = dur;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<EnemyMove>().Slow_Ratio(slow_Ratio, slow_Duration);
    }



}
