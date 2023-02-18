using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Effect_Volume : MonoBehaviour
{
    public void Awake()
    {
        GetComponent<Slider>().value = SoundManager.Instance.GetVolume(Define.AudioType.Effect);
    }

    public void SetEffectVolume(float vol)
    {
        SoundManager.Instance.SetVolume(Define.AudioType.Effect, vol);
        PlayerPrefs.SetFloat("Effect_Volume", vol);
    }

}
