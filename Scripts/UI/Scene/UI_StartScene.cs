using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StartScene : UI_Scene
{


    Image image;
    void Start()
    {
        image = GetComponent<Image>();

        StartCoroutine(StartDelay());
    }


    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(2.0f);
        image.enabled = true;
    }
}
