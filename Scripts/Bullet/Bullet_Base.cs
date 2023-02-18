using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Base : MonoBehaviour
{
    //? 필수로 가져야하는거 - 데미지, 이동속도, 이동타입
    public int _damage;
    public float _speed;
    public Move_Type _move_Type;

    public bool _tracker;
    public bool _HitCheck;

    public float _homingCount;
    [SerializeField]protected float _timer;

    public string _effect = null;

    public enum Move_Type
    {
        Straight,
        Homing,
        Lazer,
        Circle,
        Accel,
        Decel,
    }
    public Tower_Base.Attackable _attackable;


    public Vector3 _Destination;
    public GameObject _Target;
    public float _speed_rote = 3;
    //public float _disable_timer = 1;
    //public Vector3 start_pos;
    public void Init_Default (GameObject target)//, Tower_Base.Attackable attackable)
    {
        _Target = target;
        //_attackable = attackable;
    }
    public void Init_Default(GameObject target, Tower_Base.Attackable attackable)
    {
        _Target = target;
        _attackable = attackable;
    }


    public virtual void Start()
    {
        StartCoroutine(ResetBulletSelf());

        _once = true;
        _timer = 0;
        //Debug.Log("부모 start");
    }

    protected virtual void OnEnable()
    {
        _once = true;
        _timer = 0;
        _HitCheck = false;
    }

    protected bool _once;
    protected void OnceAngle()
    {
        if (_once)
        {
            _Destination = _Target.transform.position - transform.position;
            transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(_Destination.x, _Destination.y) * -Mathf.Rad2Deg);
            _once = false;
        }
    }

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > 5)
        {
            GameManager.m_Resource.Disable_Prefab(gameObject);
        }

        //if (_Target == null)
        //{
        //    return;
        //}

        switch (_move_Type)
        {
            case Move_Type.Straight: //? 
                OnceAngle();
                transform.Translate(Vector3.up * Time.deltaTime * _speed);
                break;

            case Move_Type.Homing:
                OnceAngle();
                if (_Target.activeSelf && _timer < _homingCount) //? 쫓다가 적이 사라졌을때는 그냥 가던방향으로 가게끔 // SetActive가 false가되도 target이 null이안됨..
                {
                    _Destination = _Target.transform.position - transform.position;
                    Quaternion qa = Quaternion.Euler(0, 0, Mathf.Atan2(_Destination.x, _Destination.y) * -Mathf.Rad2Deg);
                    transform.rotation = Quaternion.Slerp(transform.rotation, qa, Time.deltaTime * _speed_rote);
                }
                transform.Translate(Vector3.up * Time.deltaTime * _speed);
                break;

            case Move_Type.Lazer:
                OnceAngle();
                break;


            case Move_Type.Circle:
                //transform.Translate(Vector3.up * Time.deltaTime * _speed);
                break;


            case Move_Type.Accel: //? 0이랑 _speed순서만 바꾸면 점점 느려지는것도 가능 - 다만 그경우엔 속도가 0이되면 터지는것도 넣어야할듯
                OnceAngle();
                transform.Translate(Vector3.up * Time.deltaTime * Mathf.Lerp(0, _speed, _timer / _homingCount));
                break;


            case Move_Type.Decel:  //? 얘는 속도디셀은 아니고 각도가 점점 줄어들다가 0이되는거임. 그냥 속도에 똑같은거 복붙하면 속도디셀도 됨
                //Debug.Log(Mathf.Lerp(_speed_rote, 0, _timer / _homingCount));
                OnceAngle();
                if (_Target.activeSelf && _timer < _homingCount)
                {
                    _Destination = _Target.transform.position - transform.position;
                    Quaternion qa = Quaternion.Euler(0, 0, Mathf.Atan2(_Destination.x, _Destination.y) * -Mathf.Rad2Deg);
                    transform.rotation = Quaternion.Slerp(transform.rotation, qa, Time.deltaTime * Mathf.Lerp(_speed_rote, 0, _timer / _homingCount));
                }
                transform.Translate(Vector3.up * Time.deltaTime * _speed);
                break;

        }
    }

    



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_HitCheck)
        {
            return;
        }
        if (_attackable == Tower_Base.Attackable.Ground && collision.GetComponent<EnemyMove>().move_State == EnemyMove.Move_State.Air)
        {
            return;
        }
        if (_attackable == Tower_Base.Attackable.Air && collision.GetComponent<EnemyMove>().move_State == EnemyMove.Move_State.Ground)
        {
            return;
        }

        if (_tracker) //? 대상 추적여부(타겟이 아니면 안맞음)
        {
            if (_Target == collision.gameObject)
            {
                //Debug.Log(_damage + " 만큼의 피해를 입힘");
                collision.GetComponent<EnemyMove>().Subtract_HP(_damage);

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

                if (!string.IsNullOrEmpty(_effect))
                {
                    GameManager.m_Resource.Instant_Effect(_effect, transform.position);
                    SoundManager.Instance.PlaySound($"Effect/Explosion/{_effect}");
                }
            }
            return;
        }


        //Debug.Log(_damage + " 만큼의 피해를 입힘");
        collision.GetComponent<EnemyMove>().Subtract_HP(_damage);

        if (!string.IsNullOrEmpty(_effect))
        {
            GameManager.m_Resource.Instant_Effect(_effect, transform.position);
            SoundManager.Instance.PlaySound($"Effect/Explosion/{_effect}");
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
        //gameObject.SetActive(false);
    }





    IEnumerator ResetBulletSelf()
    {
        yield return new WaitUntil(() => GameManager.Instance.PlayState != Define.PlayMode.Play);
        GameManager.m_Resource.Disable_Prefab(gameObject);
        //gameObject.SetActive(false);
    }



}
