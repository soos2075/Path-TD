using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpeedTest : MonoBehaviour
{

    public float speed;

    public float limit;

    public float count;
    void Start()
    {
        
    }

    void Update()
    {
        count += Time.deltaTime;


        //transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 10, 0), Time.deltaTime * speed);

        if (limit > count)
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);
        }

        
    }
}
