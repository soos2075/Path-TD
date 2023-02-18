using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PopUp : UI_Base
{

    //private void Start()
    //{
    //    Init();
    //}


    public virtual void Init()
    {
        GameManager.m_UI.SetCanvas(gameObject, true);
    }

    public void SetCanvasSortOrder(bool onoff)
    {
        GameManager.m_UI.SetCanvas(gameObject, onoff);
    }


    public virtual void ClosePopUp()
    {
        GameManager.m_UI.ClosePopUp(this);
        //Debug.Log($"Close Popup : {gameObject.name}");
    }

    public void CloseAll ()
    {
        GameManager.m_UI.CloseAll();
        Debug.Log("Close Popup All");
    }

    public void CloseSelf(float delay)
    {
        Invoke("ClosePopUp", delay);
    }
}
