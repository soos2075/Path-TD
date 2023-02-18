using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStage : MonoBehaviour
{
    private static SelectStage _instance;
    public static SelectStage Instance { get { Initialize(); return _instance; } }

    private static void Initialize()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<SelectStage>();
            if (_instance == null)
            {
                GameObject go = new GameObject { name = "@SelectStage" };
                go.AddComponent<SelectStage>();
                _instance = go.GetComponent<SelectStage>();
            }
        }
    }



    public int Stage = 0;


    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void StageButton (int StageNumber)
    {
        Stage = StageNumber;
        SceneChange.Instance.Change("#3_Stage");
    }

    public void SavePlayerPrefs ()
    {
        PlayerPrefs.SetInt("Clear", Stage);
    }
}
