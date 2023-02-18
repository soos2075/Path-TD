using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    //? 포인터클릭은 버튼에는 안먹히는듯? 드래그는 패널 오브젝트에 넣어도 버튼 드래그에도 디버그찍힘
    public Action<PointerEventData> OnClickHandler;

    public Action<PointerEventData, int> OnClick_Tower;
    public int tower_Number;

    public Action<PointerEventData, int> OnClick_Stage;
    public int stage_Number;


    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("드래그중");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log($"{gameObject}클릭중");
        if (OnClickHandler != null)
        {
            Debug.Log("클릭이벤트 발생중 _ 일반");
            OnClickHandler(eventData);
        }

        if (OnClick_Tower != null)
        {

            Debug.Log("클릭이벤트 발생중 _ 타워");
            OnClick_Tower(eventData, tower_Number);

            GameObject temp = GameObject.Find("Select"); //? 이것만 태그로바꾸든 아님 위치지정을 하든 해서 비용덜들게 바꾸면될듯
            if (temp)
            {
                GameManager.m_Resource.Disable_Prefab(temp);
            }
            GameManager.m_Resource.Instant_Prefab("UI/Select", gameObject.transform);
        }

        if (OnClick_Stage != null)
        {
            Debug.Log("클릭이벤트 발생중 _ 스테이지");
            OnClick_Stage(eventData, stage_Number);
        }
    }
    //public void OnPointerClick(PointerEventData eventData, int num)
    //{
    //    Debug.Log("클릭중");
    //    if (OnClick_Tower != null)
    //    {
    //        Debug.Log("클릭이벤트 발생중 _ 타워");
    //        OnClick_Tower(eventData, num);
    //    }
    //}

}
