using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    Stack<UI_PopUp> _popupStack = new Stack<UI_PopUp>();

    int _sortOrder = 10;

    GameObject UI_Root //? UI들 한 폴더에 몰아넣는 느낌
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
            {
                root = new GameObject { name = "@UI_Root" };
            }
            return root;
        }
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = _sortOrder;
            _sortOrder++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    public T ShowSceneUI<T>(string name = null) where T : Component
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }
        GameObject go = GameManager.m_Resource.Instant_Prefab($"UI/Scene/{name}", UI_Root.transform);
        T uiComponent = Util.GetOrAddComponent<T>(go);

        return uiComponent;
    }


    public T ShowPopUp<T>(string name, bool setCanvasOrder) where T : UI_PopUp
    {
        var uiComponent = ShowPopUp<T>(name);
        if (setCanvasOrder)
        {
            SetCanvas(uiComponent.gameObject, true);
        }
        return uiComponent;
    }
    public T ShowPopUp<T>(string name = null) where T : UI_PopUp
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }
        GameObject go = GameManager.m_Resource.Instant_Prefab($"UI/PopUp/{name}", UI_Root.transform);
        T uiComponent = Util.GetOrAddComponent<T>(go);
        _popupStack.Push(uiComponent);

        return uiComponent;
    }

    public T ShowPopUp<T>(Transform parents , string name = null) where T : UI_PopUp
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }
        GameObject go = GameManager.m_Resource.Instant_Prefab($"UI/PopUp/{name}", parents);
        T uiComponent = Util.GetOrAddComponent<T>(go);
        _popupStack.Push(uiComponent);

        return uiComponent;
    }

    public void ClosePopUp()
    {
        if (_popupStack.Count == 0)
        {
            return;
        }

        UI_PopUp uiObject = _popupStack.Pop();
        GameManager.m_Resource.Disable_Prefab(uiObject.gameObject);
        uiObject = null;
        //_sortOrder--;
    }
    public void ClosePopUp(UI_PopUp popup)
    {
        if (_popupStack.Count == 0)
        {
            return;
        }

        if (_popupStack.Peek() != popup)
        {
            Debug.Log($"Close Popup Failed : {popup.name}");
            return;
        }

        ClosePopUp();
    }




    public void CloseAll()
    {
        while (_popupStack.Count > 0)
        {
            ClosePopUp();
        }
    }
}
