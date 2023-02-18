using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineSimulator : MonoBehaviour
{
    private static CoroutineSimulator _instance;
    public static CoroutineSimulator Instance { get { Initialize(); return _instance; } }

    private static void Initialize()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<CoroutineSimulator>();
            if (_instance == null)
            {
                GameObject go = new GameObject { name = "@CoroutineSimulator" };
                go.AddComponent<CoroutineSimulator>();
                _instance = go.GetComponent<CoroutineSimulator>();
            }
        }
    }




    public Coroutine CoroutineStarter (IEnumerator enumerator)
    {
        return StartCoroutine(enumerator);
    }

    public void CoroutineStoper (Coroutine coroutine)
    {
        StopCoroutine(coroutine);
    }


}
