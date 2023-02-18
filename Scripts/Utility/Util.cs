using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Util
{
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform tr = FindChild<Transform>(go, name, recursive);
        if (tr == null)
        {
            return null;
        }
        return tr.gameObject;
    }
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null) //? 게임오브젝트가 null이면 바로 리턴
        {
            return null;
        }

        if (recursive) //? 재귀적으로 찾을지 여부
        {
            foreach (T item in go.GetComponentsInChildren<T>())
            {
                if (item.name == name || string.IsNullOrEmpty(name))
                {
                    return item;
                }
            }
        }
        else
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform tr = go.transform.GetChild(i);
                if (tr.name == name || string.IsNullOrEmpty(name))
                {
                    T comp = tr.GetComponent<T>();
                    if (comp != null)
                    {
                        return comp;
                    }
                }
            }
        }

        //Debug.Log($"해당 컴포넌트를 찾지 못했습니다. {go.name} - {name}");
        return null;
    }

    public static List<T> FindsChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null) //? 게임오브젝트가 null이면 바로 리턴
        {
            return null;
        }

        List<T> list = new List<T>();

        if (recursive) //? 재귀적으로 찾을지 여부
        {
            foreach (T item in go.GetComponentsInChildren<T>())
            {
                if (item.name == name || string.IsNullOrEmpty(name))
                {
                    list.Add(item);
                    //return item;
                }
            }
        }
        else
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform tr = go.transform.GetChild(i);
                if (tr.name == name || string.IsNullOrEmpty(name))
                {
                    T comp = tr.GetComponent<T>();
                    if (comp != null)
                    {
                        list.Add(comp);
                        //return comp;
                    }
                }
            }
        }

        //Debug.Log($"해당 컴포넌트를 찾지 못했습니다. {go.name} - {name}");
        return list;
    }


    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
        {
            component = go.AddComponent<T>();
        }
        return component;
    }



    public static T SerializableDeepClone<T>(T obj)
    {
        using (var ms = new MemoryStream())
        {
            var bformatter = new BinaryFormatter();
            bformatter.Serialize(ms, obj);
            ms.Position = 0;

            return (T)bformatter.Deserialize(ms);
        }
    }
}
