using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_VersionText : UI_Scene
{
    

    void Start()
    {
        Debug.Log(Application.version);
        GetComponent<Text>().text = Application.version;
    }

    void Update()
    {
        
    }
}
