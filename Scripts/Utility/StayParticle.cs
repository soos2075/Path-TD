using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayParticle : MonoBehaviour
{

    public float delay = 1;

    public void Off_Col ()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<Bullet_Base>().enabled = false;

        var par = Util.FindsChild<ParticleSystem>(gameObject, recursive: true);
        var par2 = Util.FindsChild<TrailRenderer>(gameObject, recursive: true);
        var par3 = Util.FindsChild<LineRenderer>(gameObject, recursive: true);

        foreach (var item in par)
        {
            item.Stop();
        }
        foreach (var item in par2)
        {
            item.enabled = false;
        }
        foreach (var item in par3)
        {
            item.enabled = false;
        }


        Invoke("CallDisable", delay);
    }

    public void On_Col ()
    {
        GetComponent<CircleCollider2D>().enabled = true;
        GetComponent<Bullet_Base>().enabled = true;
    }


    void CallDisable ()
    {
        GameManager.m_Resource.Disable_Prefab(gameObject);
        On_Col();
    }
}
