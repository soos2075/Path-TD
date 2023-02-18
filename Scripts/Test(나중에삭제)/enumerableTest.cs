using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enumerableTest : MonoBehaviour
{
    class test1 : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return 1;
            yield return "apple";
        }
    }

    IEnumerator myEnumerator()
    {
        yield return new WaitForSeconds(1);
    }


    void Start()
    {
        test1 myClass = new test1();

        IEnumerable myClass2 = new test1();

        //myClass2.
    }

    void Update()
    {
        
    }
}
