using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameSpeed : MonoBehaviour
{
    public Image speed_btn;

    public Sprite speed_n;
    public Sprite speed_x1;
    public Sprite speed_x2;



    private void Update()
    {
        if (Time.timeScale == 1)
        {
            speed_btn.sprite = speed_n;
        }
    }


    public void GameSpeedDouble()
    {
        SoundManager.Instance.PlaySound("UI/Click");

        if (Time.timeScale == 1)
        {
            Time.timeScale = 1.5f;
            speed_btn.sprite = speed_x1;
        }
        else if (Time.timeScale == 1.5f)
        {
            Time.timeScale = 2;
            speed_btn.sprite = speed_x2;
        }
        else
        {
            Time.timeScale = 1;
            speed_btn.sprite = speed_n;
        }
    }
}
