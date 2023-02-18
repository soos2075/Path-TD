using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class linqTest : MonoBehaviour
{

    List<Num> myNumber = new List<Num>();


    public class Num
    {
        public int id;
        public string name;

        public Num (int i, string n)
        {
            id = i;
            name = n;
        }
    }

    public class myComparer : IComparer<Num>
    {
        public int Compare(Num x, Num y)
        {
            if (x.id > y.id)
            {
                return 1;
            }
            else if (x.id < y.id)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }

    

    void Start()
    {
        myNumber.Add(new Num(3, "첫번째"));

        myNumber.Add(new Num(1, "두번째"));

        myNumber.Add(new Num(7, "세번째"));

        myNumber.Add(new Num(5, "네번째"));

        myNumber.Add(new Num(3, "다섯번째"));

        myNumber.Add(new Num(3, "여섯번째"));

        myNumber.Add(new Num(7, "일곱번째"));


        //myNumber.Sort();

        myComparer test = new myComparer();


        myNumber.Sort(test);

        for (int i = 0; i < myNumber.Count; i++)
        {
            Debug.Log($"{myNumber[i].name} = {myNumber[i].id}");
        }

        Debug.Log("ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ구분선ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ");

        IEnumerable myLinq = from number in myNumber
                     orderby number.id
                     select number;
        //myLinq.
        foreach (Num item in myLinq)
        {
            Debug.Log(item.id);
        }

        Debug.Log("ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ구분선ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ");

        //var bbbbb = myNumber.Where((a) => a.id > 5).Count();

        Debug.Log(myNumber.Where((a) => a.id > 5).Count());





    }


    void Update()
    {
        
    }
}
