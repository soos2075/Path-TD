using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryTest : MonoBehaviour
{
    public Dictionary<int, GameObject> towerDic = new Dictionary<int, GameObject>();

    public GameObject myTower;
    public GameObject myTower2;
    void Start()
    {


        towerDic.Add(1, myTower);
        towerDic.Add(2, myTower2);

        //towerDic.Remove(2);

        GameObject oTower1;
        towerDic.TryGetValue(1, out oTower1);

        myTower2.SetActive(false);

        GameObject oTower2;
        towerDic.TryGetValue(2, out oTower2);


        Debug.Log("1번" + oTower1.transform.position);
        Debug.Log("2번" + oTower2.transform.position);


    }

    void Update()
    {
        
    }
}
