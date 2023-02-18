using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Buff : Bullet_Base
{
    float Bullet_Duration = 0.1f;

    public override void Start()
    {
        base.Start();
        Invoke("ActiveFalse", Bullet_Duration);
    }

    float Buff_Duration;
    float Buff_Damage;
    public void Init_Buff (float ratio, float dura)
    {
        Buff_Damage = ratio;
        Buff_Duration = dura;
    }


    void ActiveFalse()
    {
        //Debug.Log("스톱");

        //GetComponent<CircleCollider2D>().enabled = false;
        //gameObject.SetActive(false);
        GameManager.m_Resource.Disable_Prefab(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_HitCheck)
        {
            return;
        }

        Tower_Stat tw = collision.GetComponent<Tower_Stat>();

        if (tw.Buff_Basic == false && tw.Tower_Code != Tower_Base.TowerCode.BuffTower 
            && tw.Tower_Code != Tower_Base.TowerCode.CoinTower && tw.Tower_Code != Tower_Base.TowerCode.SlowTower)
        {
            GameObject part = GameManager.m_Resource.Instant_Effect("Buff_Effect", tw.transform.position);
            SoundManager.Instance.PlaySound("Effect/Shot/Buff");
            part.transform.parent = tw.transform;
            tw.Buff_Basic = true;
            CoroutineSimulator.Instance.CoroutineStarter(Buff_Basic(tw, Buff_Damage, Buff_Duration, part));
            _HitCheck = true;
        }
    }


    IEnumerator Buff_Basic(Tower_Stat tower, float ratio, float duration, GameObject particle)
    {
        bool detect_Check = false;

        if (tower.detecting == false)
        {
            detect_Check = true;
            Debug.Log(tower.name + " - Detecting 부여");
            tower.detecting = true;
        }

        int dmg_Plus = (int)(tower.myTower.damage * (ratio - 1));
        tower.myTower.damagePlus += dmg_Plus;


        yield return new WaitForSeconds(duration);

        if (detect_Check)
        {
            tower.detecting = false;
        }
        tower.myTower.damagePlus -= dmg_Plus;
        tower.Buff_Basic = false;
        GameManager.m_Resource.Disable_Prefab(particle);
        Debug.Log("코루틴 종료 - 버프종료");
    }


}
