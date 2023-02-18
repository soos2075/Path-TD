using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }

        Queue<Poolable> _poolStack = new Queue<Poolable>();


        public void init (GameObject origin, int count)
        {
            Original = origin;

            GameObject a = GameObject.Find($"{origin.name}_Root");
            if (a == null)
            {
                Root = new GameObject().transform;
                Root.name = $"{origin.name}_Root";
            }
            else
            {
                Root = a.transform;
            }

            //Root = new GameObject().transform;
            //Root.name = $"{origin.name}_Root";

            for (int i = 0; i < count; i++)
            {
                Push(Create());
            }
        }

        Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;
            return go.GetOrAddComponent<Poolable>();
        }


        public void Push (Poolable poolable) //? 게임오브젝트가 비활성화 될때 푸쉬가 됨 = 디스트로이 시킬때 poolable오브젝트라면 push를 하는거
        {
            poolable.gameObject.SetActive(false);
            poolable.transform.parent = Root;

            _poolStack.Enqueue(poolable);
        }
        public Poolable Pop (Transform parent) //? 게임오브젝트가 활성화 될 때 팝을 통해 빠져나감
        {
            Poolable prefab;

            if (_poolStack.Count > 0)
            {
                prefab = _poolStack.Dequeue();
            }
            else
            {
                prefab = Create();
            }

            prefab.transform.parent = parent;
            //prefab.gameObject.SetActive(true);
            //Debug.Log("@@@@@@@@@@@@@@"+_poolStack.Count);

            return prefab;
        }
    }





    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    Transform _root
    { 
        get
        {
            GameObject root = GameObject.Find("@Pool_Root");
            if (root == null)
            {
                root = new GameObject { name = "@Pool_Root" };
            }
            return root.transform;
        }
    }

    //public void Init()
    //{
    //    if (_root == null)
    //    {
    //        _root = new GameObject { name = "@Pool_Root" }.transform;
    //        Object.DontDestroyOnLoad(_root);
    //    }
    //}

    public void CreatePool(GameObject origin, int count = 1)
    {
        if (_pool.ContainsKey(origin.name) == true)
        {
            _pool[origin.name].init(origin, count);
            return;
        }

        Pool pool = new Pool();
        pool.init(origin, count);
        pool.Root.parent = _root;

        _pool.Add(origin.name, pool);
    }

    public void Push (Poolable poolable) //? Destroy 대신
    {
        string name = poolable.gameObject.name;
        if (_pool.ContainsKey(name) == false)
        {
            Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@에러");
            return;
        }


        _pool[name].Push(poolable);
    }

    public Poolable Pop (GameObject origin, Transform parent = null) //? instantiate 대신
    {
        if (_pool.ContainsKey(origin.name) == false)
        {
            CreatePool(origin);
        }

        return _pool[origin.name].Pop(parent);
    }

    public GameObject GetOriginal (string name)
    {
        if (_pool.ContainsKey(name) == false)
        {
            return null;
        }

        return _pool[name].Original;
    }



    public void Clear ()
    {

    }

}
