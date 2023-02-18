using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Language : UI_PopUp
{

    enum Buttons
    {
        English,
        Korean,
        Close,

    }


    private void Awake()
    {
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.Korean).gameObject.AddUIEvent((PointerEventData data) => LanguageSelect_Korean());
        GetButton((int)Buttons.English).gameObject.AddUIEvent((PointerEventData data) => LanguageSelect_English());

        GetButton((int)Buttons.Close).gameObject.AddUIEvent((PointerEventData data) => ClosePopUp());
    }

    void Start()
    {
        
    }


    void LanguageSelect_Korean()
    {
        Time.timeScale = 1;
        LanguageManager.Instance.Language = Define.Language.Kor;
        PlayerPrefs.SetInt("Language", (int)SystemLanguage.Korean);
        SoundManager.Instance.PlaySound("UI/Click");
        SceneChange.Instance.Change("#2_SelectStage");
    }

    void LanguageSelect_English()
    {
        Time.timeScale = 1;
        LanguageManager.Instance.Language = Define.Language.Eng;
        PlayerPrefs.SetInt("Language", (int)SystemLanguage.English);
        SoundManager.Instance.PlaySound("UI/Click");
        SceneChange.Instance.Change("#2_SelectStage");
    }



}
