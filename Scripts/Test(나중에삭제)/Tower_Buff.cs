using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Buff : MonoBehaviour
{

    //Tower_Stat myTower;

    List<Tower_Stat> change_List = new List<Tower_Stat>();


    //private void Start()
    //{
    //    myTower = GetComponentInParent<Tower_Stat>();
    //}

    void DamageUp ()
    {

    }
    void AttackSpeedUp()
    {

    }
    void Detecting()
    {

    }

    void Buff_Basic()
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Tower_Stat tw = collision.GetComponent<Tower_Stat>();
        if (tw.detecting == false)
        {
            Debug.Log(collision.name + " - Detecting 부여");
            tw.detecting = true;
            change_List.Add(tw);
        }
    }

    private void OnDisable()
    {
        foreach (Tower_Stat item in change_List)
        {
            item.detecting = false;
        }
        change_List.Clear();
    }
}
