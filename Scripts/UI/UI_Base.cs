using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Base : MonoBehaviour
{
    private Dictionary<Type, UnityEngine.Object[]> _objectDic = new Dictionary<Type, UnityEngine.Object[]>(); //? Bind호출시 저장

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objectDic.Add(typeof(T), objects); //? type = enum / objects = component[](T타입의)
//! 여기서 Type에 넣는게 Enum타입이여도 되고 typeof(T)로 T의 타입이여도 상관이 없음. 어차피 Dic에 저장되는건 둘다 동일함
//! 단지 문제는 찾는 Get함수를 불러올 때, 추가적으로 범위를 더 지정해줄 거냐 마냐 정도의 차이 전자의 경우 enum을 지정해주는거고 후자는 T를 지정해주는것
//? 가장 큰 문제를 찾았음. Enum타입을 넣을때는 Image든 Button이든 GameObject든 여러번 Bind가 가능함. 근데 typeof(T)로 넣었을때는
//? 다른 enum이여도 같은 형식이라면 Dic의 Key값이 겹쳐져버려서 Bind가 불가능. 따라서 Button,Image,Text,GameObject 한번씩만 쓸때는 이거로쓰고
//? 만약 좀 더 여러타입의 enum을 쓰고싶으면 typeof(T)대신 type을 넣는게 맞는듯. 그리고 Get할때도 무조건 저장한 컴포넌트 타입으로만 겟할수있음.
        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
            {
                objects[i] = Util.FindChild(gameObject, names[i], true);
            }
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);
            if (objects[i] == null)
            {
                Debug.Log($"해당 컴포넌트를 찾지 못했습니다. {gameObject.name} - {names[i]}");
            }
        }
    }

    protected T Get<T>(int index) where T : UnityEngine.Object
    {
        UnityEngine.Object[] obj = null;

        if (_objectDic.TryGetValue(typeof(T), out obj) == false)
        {
            Debug.Log("Get Component failed");
            return null;
        }

        return obj[index] as T;
    }

    protected Button GetButton(int index) { return Get<Button>(index); }
    protected Text GetText(int index) { return Get<Text>(index); }
    protected Image GetImage(int index) { return Get<Image>(index); }
    protected GameObject GetGameObject(int index) { return Get<GameObject>(index); }




    public static void AddUIEvent(GameObject go, Action<PointerEventData> act, Define.MouseEvent event_type = Define.MouseEvent.Click)
    {
        UI_EventHandler _evnet = Util.GetOrAddComponent<UI_EventHandler>(go);

        switch (event_type)
        {
            case Define.MouseEvent.Click:
                _evnet.OnClickHandler += act;
                break;
        }
    }

    public static void AddUIEvent_TowerSelect(GameObject go, Action<PointerEventData, int> act, int num)
    {
        UI_EventHandler _evnet = Util.GetOrAddComponent<UI_EventHandler>(go);
        _evnet.tower_Number = num;
        _evnet.OnClick_Tower += act;
    }

    public static void AddUIEvent_StageSelect(GameObject go, Action<PointerEventData, int> act, int num)
    {
        UI_EventHandler _evnet = Util.GetOrAddComponent<UI_EventHandler>(go);
        _evnet.stage_Number = num;
        _evnet.OnClick_Stage += act;
    }
}
