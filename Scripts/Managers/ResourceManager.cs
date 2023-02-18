using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    private T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            int index = path.LastIndexOf('/');
            string name = path.Substring(index + 1);
            GameObject go = GameManager.m_Pool.GetOriginal(name);
            if (go != null)
            {
                return go as T;
            }
        }

        //Debug.Log($"풀링오브젝트 없음. {path}");
        return Resources.Load<T>(path);
    }


    public void Create_Pool_Init (string path, int count)
    {
        GameObject go = Resources.Load<GameObject>($"Prefabs/{path}");
        if (go == null)
        {
            Debug.Log($"해당 오브젝트를 찾을 수 없습니다 : Resources/Prefabs/{path}");
            return;
        }
        GameManager.m_Pool.CreatePool(go, count);
    }


    public T SearchResourcesData<T>(string path) where T : Object
    {
        T resource = Load<T>($"{path}");

        if (resource == null)
        {
            Debug.Log($"해당 오브젝트를 찾을 수 없습니다 : Resources/{path}");
            return default(T);
        }

        return resource;
    }

    public Sprite SearchUISprite(string path)
    {
        return SearchResourcesData<Sprite>($"UI/{path}");
    }


    public GameObject Instant_Prefab(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if (prefab == null)
        {
            Debug.Log($"해당 오브젝트를 찾을 수 없습니다 : Resources/Prefabs/{path}");
            return null;
        }


        if (prefab.GetComponent<Poolable>() != null)
        {
            //Debug.Log($"Poolable 객체 가져옴 : {prefab.name}");
            return GameManager.m_Pool.Pop(prefab, parent).gameObject;
        }


        //Debug.Log($"객체 새로 생성 : {prefab.name}");
        GameObject go = Object.Instantiate(prefab, parent);
        go.name = prefab.name;
        return go;
    }

    public GameObject Instant_Prefab(string path, Vector3 pos, Transform parent = null) //? 이전까지는 풀링오브젝트는 무조건 Active false니까 여기서 바꿔줘야함
    {
        GameObject go = Instant_Prefab(path, parent);
        go.transform.position = pos;
        go.SetActive(true);
        return go;
    }

    public GameObject Instant_Prefab(string path, Vector3 pos,Quaternion qa, Transform parent = null)
    {
        GameObject go = Instant_Prefab(path, parent);
        go.transform.position = pos;
        go.transform.rotation = qa;
        go.SetActive(true);
        return go;
    }




    public void Disable_Prefab(GameObject go)
    {
        if (go == null)
            return;

        Poolable pool = go.GetComponent<Poolable>();
        if (pool != null)
        {
            //Debug.Log($"Poolable 객체 비활성화 : {pool.name}");
            //pool.gameObject.SetActive(false);
            GameManager.m_Pool.Push(pool);
            return;
        }


        Object.Destroy(go);
    }


    GameObject Effect_Root //? 이펙트 오브젝트들
    {
        get
        {
            GameObject root = GameObject.Find("@Effect_Root");
            if (root == null)
            {
                root = new GameObject { name = "@Effect_Root" };
            }
            return root;
        }
    }


    public GameObject Instant_Effect(string path, Vector3 pos)
    {
        return Instant_Prefab($"Bullet/Effect/{path}", pos, Effect_Root.transform);
    }
    public GameObject Instant_Effect_Enemy(string path, Vector3 pos)
    {
        return Instant_Prefab($"Enemy/Effect/{path}", pos, Effect_Root.transform);
    }
}
