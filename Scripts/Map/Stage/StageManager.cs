using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private static StageManager _instance;
    public static StageManager Instance { get { Initialize(); return _instance; } }

    private static void Initialize()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<StageManager>();
            if (_instance == null)
            {
                GameObject go = new GameObject { name = "@StageManager" };
                go.AddComponent<StageManager>();
                _instance = go.GetComponent<StageManager>();
            }
        }
    }


    public delegate void WaveStartDel();
    public event WaveStartDel Event_WaveStart;


    public void ResetEvent()
    {
        Event_WaveStart = null;
        Event_WaveOver = null;
    }

    public void WaveStartCallback ()
    {
        Event_WaveStart();
    }

    public delegate void WaveOverDel();
    public event WaveOverDel Event_WaveOver;

    public void WaveOverCallback()
    {
        Event_WaveOver();
    }



}
