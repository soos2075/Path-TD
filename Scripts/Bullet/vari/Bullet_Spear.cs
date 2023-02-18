using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Spear : Bullet_Base
{


    float spear_timer;
    public float accelCount;


    protected override void OnEnable()
    {
        base.OnEnable();
        spear_timer = 0;
    }

    void Update()
    {
        spear_timer += Time.deltaTime;


        OnceAngle();
        if (_Target.activeSelf) //? 쫓다가 적이 사라졌을때는 그냥 가던방향으로 가게끔 // SetActive가 false가되도 target이 null이안됨..
        {
            _Destination = _Target.transform.position - transform.position;
            Quaternion qa = Quaternion.Euler(0, 0, Mathf.Atan2(_Destination.x, _Destination.y) * -Mathf.Rad2Deg);
            transform.rotation = Quaternion.Slerp(transform.rotation, qa, Time.deltaTime * _speed_rote);
        }
        transform.Translate(Vector3.up * Time.deltaTime * Mathf.Lerp(0, _speed, spear_timer / accelCount));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_Target == collision.gameObject)
        {
            collision.GetComponent<EnemyMove>().Subtract_HP(_damage);
            Debug.Log(_damage + " 만큼의 피해 + ");


            //gameObject.SetActive(false);
            GameManager.m_Resource.Disable_Prefab(gameObject);


            if (!string.IsNullOrEmpty(_effect))
            {
                GameManager.m_Resource.Instant_Effect(_effect, transform.position);
                SoundManager.Instance.PlaySound("Effect/Explosion/Spear_Explosion");
            }
        }
    }

}
