using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleTest : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Test());
    }

    float count = 0;
    bool stoper = false;

    void Update()
    {
        Debug.Log(Time.deltaTime);
        if (stoper)
        {
            count += Time.deltaTime;
        }
    }


    IEnumerator TestCase1 ()
    {
        Time.timeScale = 1.0f;
        count = 0;
        stoper = true;
        yield return new WaitForSeconds(2);
        stoper = false;
        Debug.Log($"Case1 : {count}");
    }

    IEnumerator TestCase2()
    {
        Time.timeScale = 2.0f;
        count = 0;
        stoper = true;
        yield return new WaitForSeconds(2);
        stoper = false;
        Debug.Log($"Case2 : {count}");
    }


    IEnumerator Test ()
    {
        StartCoroutine(TestCase1());

        yield return new WaitForSeconds(3);

        StartCoroutine(TestCase2());
    }

}
