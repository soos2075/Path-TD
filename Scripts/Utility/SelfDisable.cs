using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDisable : MonoBehaviour
{
    public float disableTime;


    void Start()
    {
        
    }

    private void OnEnable()
    {
        OnActive();
        //Debug.Log(gameObject.name + "이거되는중11111");
    }

    public virtual void OnActive()
    {
        gameObject.SetActive(true);
        StartCoroutine(Disable());
    }


    IEnumerator Disable()
    {
        yield return new WaitForSeconds(disableTime);
        //gameObject.SetActive(false);
        GameManager.m_Resource.Disable_Prefab(gameObject);
    }
}
