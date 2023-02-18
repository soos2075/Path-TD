using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_CoinTowerText : MonoBehaviour
{

    //public TextMeshPro tmp;
    //public Text tmp2;

    //public TextMeshProUGUI tmp3;
    private void Awake()
    {
        
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void SetText(int coin)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = $"{coin}$";
        //tmp3.text = $"{coin}$";
    }
}
