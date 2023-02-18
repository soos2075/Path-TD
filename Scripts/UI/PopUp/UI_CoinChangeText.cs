using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CoinChangeText : MonoBehaviour
{

    public Text coinText;

    void Start()
    {
        
    }


    public void SetText(string str)
    {
        coinText.text = str;
    }

}
