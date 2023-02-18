using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDisable_PlayState : SelfDisable
{
    void Start()
    {
        
    }

    public override void OnActive()
    {
        base.OnActive();
        StartCoroutine(Disable_State());
    }


    IEnumerator Disable_State()
    {
        yield return new WaitUntil(() => GameManager.Instance.PlayState != Define.PlayMode.Play);
        GameManager.m_Resource.Disable_Prefab(gameObject);
    }
}
