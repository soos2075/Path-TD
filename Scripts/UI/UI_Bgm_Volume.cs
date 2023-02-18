using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Bgm_Volume : MonoBehaviour
{
    public void Awake()
    {
        GetComponent<Slider>().value = SoundManager.Instance.GetVolume(Define.AudioType.BGM);
    }


    public void SetBgmVolume(float vol)
    {
        SoundManager.Instance.SetVolume(Define.AudioType.BGM, vol);
        PlayerPrefs.SetFloat("BGM_Volume", vol);
    }
}
