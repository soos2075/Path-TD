using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Move : MonoBehaviour
{

    float _speed;
    Vector3 _Destination;
    float _lifeCount;
    Move_Type _move_type;


    public enum Move_Type
    {
        Straight,
        Homing,
        Lazer,
        Circle,
        Accel,
        Decel,
    }



    public void Init_Bullet(Vector3 pos_destination, float bul_speed = 5, Move_Type move_type = Move_Type.Straight, float lifeTime = 100)
    {
        _speed = bul_speed;
        _Destination = (pos_destination - transform.position).normalized;
        _move_type = move_type;
        _lifeCount = lifeTime;
    }



    void Start()
    {
        Invoke("SelfDisappear", _lifeCount);
    }
    void Update()
    {
        switch (_move_type)
        {
            case Move_Type.Straight:
                transform.Translate(_Destination * Time.deltaTime * _speed);
                break;
            case Move_Type.Homing:
                break;
            case Move_Type.Lazer:
                break;
            case Move_Type.Accel:
                break;
            case Move_Type.Decel:
                break;
        }
    }

    void SelfDisappear()
    {
        gameObject.SetActive(false);
    }



    private void OnDisable()
    {
        Debug.Log("Bullet_Disable");
    }

}
