using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    #region Overlap범위테스트
    //public Vector2 pos;

    //public float radius;
    //void Start()
    //{

    //}
    //void Update()
    //{
    //    Collider2D[] col_List = Physics2D.OverlapCircleAll(pos, radius);

    //    foreach (Collider2D item in col_List)
    //    {
    //        Debug.Log(item.name);
    //    }


    //}
    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(pos, radius);
    //}
    #endregion

    //? 비트연산 테스트
    //int a = 2048;
    //int b = 10;
    //int c = 0;
    private void Start()
    {
        //c = a >> b;
        Debug.Log(gameObject.layer); // 0~31
        Debug.Log(LayerMask.GetMask("Buff") >> 12); // 4096 , 1


    }

    Vector3 mousePos;
    RaycastHit2D myRay;
    private void Update()
    {

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        myRay = Physics2D.Raycast(mousePos, Vector3.forward, 10, LayerMask.GetMask("Tower", "Tile"));


        if (myRay.collider != null) //? 타일선택마크
        {
            Debug.Log(myRay.collider.gameObject);
            //Debug.Log(myRay.collider.gameObject.layer);
            //Debug.Log(myRay.collider.gameObject);
        }
    }

}
