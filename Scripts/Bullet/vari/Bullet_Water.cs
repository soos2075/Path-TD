using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Water : Bullet_Base
{

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(transform.position, 1.5f);
    //}

    public void Water_initialize()
    {

    }

    public override void Start()
    {
        base.Start();
    }


    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > 8)
        {
            GameManager.m_Resource.Disable_Prefab(gameObject);
        }

        if (_Target == null || !_Target.activeSelf)
        {
            Collider2D col_temp1 = Physics2D.OverlapCircle(transform.position, 1.0f, LayerMask.GetMask("Enemy"));
            if (col_temp1 != null)
            {
                _Target = col_temp1.gameObject;
            }
        }


        //OnceAngle();
        if (_Target != null && _Target.activeSelf && _timer < _homingCount && _timer > 0.2f)
        {
            _Destination = _Target.transform.position - transform.position;
            Quaternion qa = Quaternion.Euler(0, 0, Mathf.Atan2(_Destination.x, _Destination.y) * -Mathf.Rad2Deg);
            transform.rotation = Quaternion.Slerp(transform.rotation, qa, Time.deltaTime * _speed_rote);
        }

        transform.Translate(Vector3.up * Time.deltaTime * _speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_HitCheck)
        {
            return;
        }
        //Debug.Log(_damage + " 만큼의 피해를 입힘");
        collision.GetComponent<EnemyMove>().Subtract_HP(_damage);

        if (!string.IsNullOrEmpty(_effect))
        {
            GameManager.m_Resource.Instant_Effect(_effect, transform.position);
            SoundManager.Instance.PlaySound($"Effect/Explosion/{_effect}", volume: 0.4f);
        }


        if (gameObject.GetComponent<StayParticle>() != null)
        {
            gameObject.GetComponent<StayParticle>().Off_Col();
        }
        else if (gameObject.GetComponent<Poolable>() != null)
        {
            GameManager.m_Resource.Disable_Prefab(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }

        _HitCheck = true;
    }
}
