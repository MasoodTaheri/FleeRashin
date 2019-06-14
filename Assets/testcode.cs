using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testcode : MonoBehaviour
{

    public class classA
    {
        protected virtual int Add(int a, int b)
        {
            return a + b;
        }

        public int addtest(int a, int b)
        {
            return Add(a, b);
        }
    }

    public class classB : classA
    {
        protected override int Add(int a, int b)
        {
            return (a + b) * 2;
        }
    }


    [ContextMenu("Test")]
    public void Test()
    {
        classB B = new classB();
        Debug.Log(B.addtest(2, 3));
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
