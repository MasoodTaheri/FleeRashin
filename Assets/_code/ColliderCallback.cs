using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCallback : MonoBehaviour
{

    public delegate void Enter(Collision other, GameObject me);
    public Enter enter;
    public delegate void Exit(Collision other, GameObject me);
    public Exit exit;
    public delegate void Stay(Collision other, GameObject me);
    public Stay stay;
    void Awake()
    {
        enter += DoNothing;
        exit += DoNothing;
        stay += DoNothing;
    }
    void OnCollisionEnter(Collision other)
    {
        enter(other,this.gameObject);
    }
    void OnCollisionExit(Collision other)
    {
        exit(other, this.gameObject);
    }
    void OnCollisionStay(Collision other)
    {
        stay(other, this.gameObject);
    }
    void DoNothing(Collision other, GameObject me) { }



    public new  delegate void Destroy();
    public Destroy destroy;
    void OnDestroy()
    {
        //Debug.Log("OnDestroy "+gameObject.name);
        if (destroy!=null)
        destroy();
    }

}
