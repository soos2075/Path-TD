using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Platform : UI_Base
{

    Text platform;

    void Start()
    {
        platform = GetComponent<Text>();
    }

    void Update()
    {
        platform.text = $"▣{GameManager.Instance.Platform.ToString()}";
    }
}
