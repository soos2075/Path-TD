using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSizeManager : MonoBehaviour
{

    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        if (Screen.height / Screen.width > 2)
        {

        }
        
    }

    void Update()
    {
        
    }
}
