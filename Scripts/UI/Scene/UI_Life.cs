using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Life : UI_Base
{

    enum Texts
    {
        Life,
        Coin,

    }


    private void Awake()
    {
        Bind<Text>(typeof(Texts));

        GameManager.Instance.Event_AddCoin -= AddCoinAnimation;
        GameManager.Instance.Event_AddCoin += AddCoinAnimation;
        //Debug.Log("@@@@@@@@@@");
    }

    void Start()
    {
        GetText((int)Texts.Coin).text = $"{GameManager.Instance.Coin}$";
    }
    void Update()
    {
        GetText((int)Texts.Life).text = $"♥{GameManager.Instance.Life}";
        //GetText((int)Texts.Coin).text = $"Coin : {GameManager.Instance.Coin}";
    }


    Coroutine runtime_Coroutine;

    

    public void AddCoinAnimation(int coin, bool AddorSub)
    {
        if (coin == 0)
        {
            GetText((int)Texts.Coin).text = $"{GameManager.Instance.Coin}$";
            return;
        }

        if (runtime_Coroutine != null)
        {
            StopCoroutine(runtime_Coroutine);
        }

        if (AddorSub)
        {
            runtime_Coroutine = StartCoroutine(CoinAnimation_Add(coin));
            CoinAni($"+{coin}$");
        }
        else
        {
            runtime_Coroutine = StartCoroutine(CoinAnimation_Sub(coin));
            CoinAni($"-{coin}$");
        }
    }

    IEnumerator CoinAnimation_Add(int addCoin, float time = 1.5f)
    {
        float viewCoin = GameManager.Instance.Coin - addCoin;
        float duration = time;
        float offset = addCoin / duration;
        
        while (viewCoin < GameManager.Instance.Coin)
        {
            viewCoin += offset * Time.deltaTime;
            GetText((int)Texts.Coin).text = $"{(int)viewCoin}$";
            yield return null;
        }
        GetText((int)Texts.Coin).text = $"{GameManager.Instance.Coin}$";
    }

    IEnumerator CoinAnimation_Sub(int subCoin, float time = 1.5f)
    {
        float viewCoin = GameManager.Instance.Coin + subCoin;
        float duration = time;
        float offset = subCoin / duration;
        
        while (viewCoin > GameManager.Instance.Coin)
        {
            viewCoin -= offset * Time.deltaTime;
            GetText((int)Texts.Coin).text = $"{(int)viewCoin}$";
            yield return null;
        }
        GetText((int)Texts.Coin).text = $"{GameManager.Instance.Coin}$";
    }

    void CoinAni(string str)
    {
        var tt = GameManager.m_Resource.Instant_Prefab("UI/Animation/CoinChangeText", GameObject.FindGameObjectWithTag("Canvas_Screen").transform);
        tt.GetComponent<UI_CoinChangeText>().SetText(str);
    }

}
