using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //List<GameObject> enemy_List = new List<GameObject>();
    //static public int stage_enemy_Count = 0;
    //void Start()
    //{
        
    //}
    //void Update()
    //{

    //}

    //public void Stage_Start (int wave_number)
    //{
    //    switch (wave_number)
    //    {
    //        case 0:
    //            StartCoroutine(Stage_Test_1());
    //            break;

    //        case 1:
    //            Stage_1();
    //            break;

    //        case 2:
    //            Stage_2();
    //            break;

    //        case 3:
    //            Stage_3();
    //            break;

    //        case 4:
    //            Stage_4();
    //            break;
    //        case 5:
    //            Stage_5();
    //            break;
    //    }
    //}
    //IEnumerator c_Stage_End(int quantity, float time)
    //{
    //    yield return new WaitForSeconds(quantity * time);
    //    yield return new WaitUntil(() => GameManager.m_Enemy.stage_enemy_Count <= 0);
    //    GameManager.Instance.PlayState = Define.PlayMode.TowerBuild;
    //    Debug.Log("Wave 종료");
    //}
    //IEnumerator c_Stage_End()
    //{
    //    yield return new WaitUntil(() => GameManager.m_Enemy.stage_enemy_Count <= 0);
    //    GameManager.Instance.PlayState = Define.PlayMode.TowerBuild;
    //    Debug.Log("Wave 종료");
    //}

    //void Stage_1()
    //{
    //    StartCoroutine(GameManager.m_Enemy.Instant_Wave(20, 0.5f, "Enemy_A"));
    //    GameManager.Instance.PlayState = Define.PlayMode.Play;
    //    Debug.Log("Wave 시작");
    //    StartCoroutine(c_Stage_End(20, 0.5f));
    //}
    //void Stage_2()
    //{
    //    StartCoroutine(GameManager.m_Enemy.Instant_Wave(20, 0.5f, "Enemy_B"));
    //    GameManager.Instance.PlayState = Define.PlayMode.Play;
    //    Debug.Log("Wave 시작");
    //    StartCoroutine(c_Stage_End(20, 0.5f));
    //}
    //void Stage_3()
    //{
    //    StartCoroutine(GameManager.m_Enemy.Instant_Wave(20, 0.5f, "Enemy_C"));
    //    GameManager.Instance.PlayState = Define.PlayMode.Play;
    //    Debug.Log("Wave 시작");
    //    StartCoroutine(c_Stage_End(20, 0.5f));
    //}

    //void Stage_4()
    //{
    //    StartCoroutine(GameManager.m_Enemy.Instant_Wave(20, 0.5f, "Enemy_D"));
    //    GameManager.Instance.PlayState = Define.PlayMode.Play;
    //    Debug.Log("Wave 시작");
    //    StartCoroutine(c_Stage_End(20, 0.5f));
    //}

    //void Stage_5()
    //{
    //    StartCoroutine(GameManager.m_Enemy.Instant_Wave(20, 0.5f, "Enemy_E"));
    //    GameManager.Instance.PlayState = Define.PlayMode.Play;
    //    Debug.Log("Wave 시작");
    //    StartCoroutine(c_Stage_End(20, 0.5f));
    //}


    //IEnumerator Stage_Test_1 ()
    //{
    //    GameManager.Instance.PlayState = Define.PlayMode.Play;
    //    Debug.Log("Wave 시작");

    //    StartCoroutine(GameManager.m_Enemy.Instant_Wave(5, 0.5f, "Enemy_A"));
    //    yield return new WaitForSeconds(3);
    //    StartCoroutine(GameManager.m_Enemy.Instant_Wave(5, 0.5f, "Enemy_B"));
    //    yield return new WaitForSeconds(3);
    //    StartCoroutine(GameManager.m_Enemy.Instant_Wave(5, 0.5f, "Enemy_C"));
    //    yield return new WaitForSeconds(3);
    //    StartCoroutine(GameManager.m_Enemy.Instant_Wave(5, 0.5f, "Enemy_D"));
    //    yield return new WaitForSeconds(3);
    //    StartCoroutine(GameManager.m_Enemy.Instant_Wave(5, 0.5f, "Enemy_E"));
    //    yield return new WaitForSeconds(3);

    //    StartCoroutine(c_Stage_End());
    //    //StartCoroutine(GameManager.m_Enemy.c_Stage_End());
    //}



}
