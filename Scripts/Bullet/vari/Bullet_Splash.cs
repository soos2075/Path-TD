using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Splash : Bullet_Base
{


    private GameObject boom;



    //private void Awake()
    //{
    //    //boom = GameManager.m_Resource.Instant_Prefab("Bullet/Support/Support_Splash");
    //    boom = GameManager.m_Tower.Instant_Support("Support/Support_Splash", transform.position);
    //    //boom = GameManager.m_Tower.Instant_Bullet("Support/Support_Splash", transform.position, damage_area);
    //    boom.SetActive(false);
    //    Util.FindChild<Bullet_Base>(boom)._damage = _dmg;
    //    boom.transform.localScale = new Vector3(_area, _area, _area);
    //}

    protected override void OnEnable()
    {
        base.OnEnable();
    }




    public void Init_boom (int damage_area, float effectArea)
    {
        boom = GameManager.m_Tower.Instant_Support("Support/Support_Splash", transform.position);
        boom.SetActive(false);

        Util.FindChild<Bullet_Area>(boom)._damage = damage_area;
        boom.transform.localScale = new Vector3(effectArea, effectArea, effectArea);
    }

    void Active_boom ()
    {
        boom.transform.position = transform.position;
        boom.SetActive(true);
        boom.transform.GetChild(0).gameObject.SetActive(true);
        //boom.GetComponent<ParticleSystem>().Play();
        //var va = boom.GetComponentsInChildren<ParticleSystem>();
        //foreach (var item in va)
        //{
        //    item.Play();
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_HitCheck)
        {
            return;
        }

        collision.GetComponent<EnemyMove>().Subtract_HP(_damage); //? 부모데미지 후 범위피격체 소환
        Active_boom();
        SoundManager.Instance.PlaySound("Effect/Explosion/Splash_Explosion");

        GameManager.m_Resource.Disable_Prefab(gameObject);
        //gameObject.SetActive(false);
        _HitCheck = true;
    }

}
