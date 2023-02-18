using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Homing : MonoBehaviour
{

    public GameObject target;
    public float speed;

    void Start()
    {
        
    }
    void Update()
    {
        Vector3 minus = target.transform.position - transform.position;
        Quaternion qa = Quaternion.Euler(0, 0, Mathf.Atan2(minus.x, minus.y) * -Mathf.Rad2Deg);
        transform.rotation = Quaternion.Slerp(transform.rotation, qa, Time.deltaTime * 2);

        transform.Translate(Vector3.up * Time.deltaTime * speed);
    }
}
