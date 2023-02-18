using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class testestsetse : MonoBehaviour
{

    ParticleSystem[] par = new ParticleSystem[4];

    void Start()
    {
        //var data = GameManager.m_Data.CSV_LOAD_Stage("Stage1");
        //var contents = GameManager.m_Enemy.SearchWave(data.Line, 1);

        //var data2 = GameManager.m_Data.CSV_LOAD_Tower("Tower");
        ////var contents2 = GameManager.m_Enemy.SearchWave(data2.Line, 1);

        //Debug.Log(data.Memo);
        //Debug.Log(data2.Memo);

        //var towerList = GameManager.m_Tower.SearchTower(data2.Line, data2.Counter);
        //Debug.Log(towerList[1].AttackRange);


        par = GetComponentsInChildren<ParticleSystem>();
        StartCoroutine(playper());
    }
    void Update()
    {
        
    }

    IEnumerator stoper ()
    {
        yield return new WaitForSeconds(3);
        for (int i = 0; i < 4; i++)
        {
            par[i].Stop();
        }
    }
    IEnumerator playper()
    {
        yield return new WaitForSeconds(3);
        for (int i = 0; i < 4; i++)
        {
            par[i].Play();
        }
    }
}
