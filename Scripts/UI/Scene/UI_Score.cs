using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Score : UI_Scene
{
    enum Texts
    {
        ScoreText,

    }

    private void Awake()
    {
        Bind<Text>(typeof(Texts));
    }



    private void Update()
    {
        GetText((int)Texts.ScoreText).text = $"★{GameManager.Instance.Score}";
    }



}
