using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    private static LanguageManager _instance;
    public static LanguageManager Instance { get { Initialize(); return _instance; } }

    private static void Initialize()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<LanguageManager>();
            if (_instance == null)
            {
                GameObject go = new GameObject { name = "@LanguageManager" };
                go.AddComponent<LanguageManager>();
                _instance = go.GetComponent<LanguageManager>();
                DontDestroyOnLoad(go);
            }
        }
    }


    public Define.Language Language;


    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        SystemLanguage lan = SystemLanguage.English;

        if (PlayerPrefs.GetInt("Language", 0) == 0)
        {
            lan = Application.systemLanguage;
        }
        else
        {
            lan = (SystemLanguage)PlayerPrefs.GetInt("Language", 0);
        }


        switch (lan)
        {
            case SystemLanguage.Korean:
                Language = Define.Language.Kor;
                break;

            default:
                Language = Define.Language.Eng;
                break;
        }

    }

    void Update()
    {
        
    }
}
