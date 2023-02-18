using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Arcane : Bullet_Base
{


    [SerializeField]List<GameObject> targetList;

    [SerializeField] int targetNumber;
    [SerializeField] int limiter;


    public override void Start()
    {
        base.Start();
        targetNumber = 0;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        targetNumber = 0;
    }

    public void Arcane_Initialize(List<Collider2D> enemys, int bounce_limit)
    {
        targetList = new List<GameObject>();

        limiter = enemys.Count > bounce_limit ? bounce_limit : enemys.Count;

        for (int i = 0; i < limiter; i++)
        {
            targetList.Add(enemys[i].gameObject);
        }
    }



    void Update()
    {
        if (!_Target.activeSelf && targetList.Count > (targetNumber + 1))
        {
            targetNumber++;
            _Target = targetList[targetNumber].gameObject;
        }


        OnceAngle();

        if (_Target.activeSelf) //? 쫓다가 적이 사라졌을때는 그냥 가던방향으로 가게끔 // SetActive가 false가되도 target이 null이안됨..
        {
            _Destination = _Target.transform.position - transform.position;
            Quaternion qa = Quaternion.Euler(0, 0, Mathf.Atan2(_Destination.x, _Destination.y) * -Mathf.Rad2Deg);
            transform.rotation = Quaternion.Slerp(transform.rotation, qa, Time.deltaTime * _speed_rote);
        }
        transform.Translate(Vector3.up * Time.deltaTime * _speed);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (_Target == collision.gameObject)
        //{
        //    //Debug.Log($"{targetNumber + 1} 번째 적 공격");
        //    collision.GetComponent<EnemyMove>().Subtract_HP(_damage);
        //    targetNumber++;

        //    if (!string.IsNullOrEmpty(_effect))
        //    {
        //        GameManager.m_Resource.Instant_Effect(_effect, transform.position);

        //        //SoundManager.Instance.PlaySound($"Effect/Explosion/{_effect}");
        //    }

        //    if (targetList.Count > targetNumber)
        //    {
        //        _Target = targetList[targetNumber].gameObject;
        //    }
        //    else
        //    {
        //        if (gameObject.GetComponent<StayParticle>() != null)
        //        {
        //            gameObject.GetComponent<StayParticle>().Off_Col();
        //        }
        //        else if (gameObject.GetComponent<Poolable>() != null)
        //        {
        //            GameManager.m_Resource.Disable_Prefab(gameObject);
        //        }
        //        else
        //        {
        //            gameObject.SetActive(false);
        //        }
        //        //Debug.Log($"터짐");
        //    }
        //}
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_Target == collision.gameObject)
        {
            //Debug.Log($"{targetNumber + 1} 번째 적 공격");
            collision.GetComponent<EnemyMove>().Subtract_HP(_damage);
            targetNumber++;

            if (!string.IsNullOrEmpty(_effect))
            {
                GameManager.m_Resource.Instant_Effect(_effect, transform.position);

                SoundManager.Instance.PlaySound($"Effect/Explosion/{_effect}");
            }

            if (targetList.Count > targetNumber)
            {
                _Target = targetList[targetNumber].gameObject;

                if (targetList.Count == targetNumber + 1 && !_Target.activeSelf)
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
                //Debug.Log($"터짐");
            }
        }
    }

}
