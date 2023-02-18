using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Lightning : Bullet_Base
{

    //public List<GameObject> targetList = new List<GameObject>();


    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(transform.position, 1.5f);
    //}

    public int bounceCount;
    public void Bounce_initialize(int bounce)
    {
        bounceCount = bounce;
    }


    void Update()
    {
        if (_Target == null || !_Target.activeSelf)
        {
            if (bounceCount > 0)
            {
                bounceCount--;
                Collider2D[] col_temp1 = Physics2D.OverlapCircleAll(transform.position, 1.5f, LayerMask.GetMask("Enemy"));
                foreach (var item in col_temp1)
                {
                    if (item.gameObject != _Target && item.GetComponent<EnemyMove>().move_State == EnemyMove.Move_State.Ground)
                    {
                        _Target = item.gameObject;
                        return;
                        //break;
                    }
                }
                Debug.Log("못찾아서 소멸");
                GameManager.m_Resource.Disable_Prefab(gameObject);
            }
        }

        OnceAngle();
        if (_Target != null && _Target.activeSelf)
        {
            _Destination = _Target.transform.position - transform.position;
            Quaternion qa = Quaternion.Euler(0, 0, Mathf.Atan2(_Destination.x, _Destination.y) * -Mathf.Rad2Deg);
            transform.rotation = Quaternion.Slerp(transform.rotation, qa, Time.deltaTime * _speed_rote);
        }

        transform.Translate(Vector3.up * Time.deltaTime * _speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_Target == collision.gameObject)
        {
            //Debug.Log(_damage + " 만큼의 피해를 입힘");
            collision.GetComponent<EnemyMove>().Subtract_HP(_damage);
            bounceCount--;
            if (!string.IsNullOrEmpty(_effect))
            {
                GameManager.m_Resource.Instant_Effect(_effect, transform.position);
                SoundManager.Instance.PlaySound($"Effect/Explosion/{_effect}", volume: 0.1f);
            }

            if (bounceCount > 0)
            {
                Collider2D[] col_temp1 = Physics2D.OverlapCircleAll(transform.position, 1.5f, LayerMask.GetMask("Enemy"));
                foreach (var item in col_temp1)
                {
                    if (item.gameObject != _Target && item.GetComponent<EnemyMove>().move_State == EnemyMove.Move_State.Ground)
                    {
                        _Target = item.gameObject;
                        return;
                        //break;
                    }
                }
                //Debug.Log("못찾아서 소멸");
                GameManager.m_Resource.Disable_Prefab(gameObject);
            }
            else
            {
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
            }
        }
    }
}
