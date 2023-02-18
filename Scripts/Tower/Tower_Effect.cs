using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Effect : MonoBehaviour
{
    public Vector3 rotateVector = Vector3.zero;

    public enum spaceEnum { Local, World };
    public spaceEnum rotateSpace;



    GameObject[] dot = new GameObject[5];
    Tower_Stat tower;

    void Awake()
    {
        tower = transform.GetComponentInParent<Tower_Stat>();

        for (int i = 0; i < transform.childCount; i++)
        {
            dot[i] = transform.GetChild(i).gameObject;
        }

        Init_Dot();
    }

    public void Init_Dot ()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            dot[i].gameObject.SetActive(false);
        }

        Add_Dot();
    }

    public void Add_Dot ()
    {
        if (tower.Level > 0 && tower.Level < 5)
        {
            for (int i = 0; i < tower.Level; i++)
            {
                dot[i].SetActive(true);
            }
        }
        else if (tower.Level == 5)
        {
            for (int i = 0; i < 4; i++)
            {
                dot[i].SetActive(false);
            }
            dot[4].SetActive(true);
        }
    }

    void Update()
    {
        if (rotateSpace == spaceEnum.Local)
            transform.Rotate(rotateVector * Time.unscaledDeltaTime);
        if (rotateSpace == spaceEnum.World)
            transform.Rotate(rotateVector * Time.unscaledDeltaTime, Space.World);
    }
}
