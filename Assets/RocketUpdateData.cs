using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketUpdateData : networkRigidbody2
{

    //// Use this for initialization
    //void Start () {
    //       base.Start();

    //}

    //// Update is called once per frame
    //void Update () {
    //       base.Update();

    //}
    new void FixedUpdate()
    {
        //return;
        //if (!pv.IsMine)
        //    if (IsNeedForceSync())
        //    {
        //        ForceSync();
        //        return;
        //    }
        base.FixedUpdate();
    }

    private bool IsNeedForceSync()
    {
        //return ((ErrorInPos.x > ErrorInPos.y) || (ErrorInRot.x > ErrorInRot.y));
        return ((ErrorInPos.x > ErrorInPos.y));
    }

    private void ForceSync()
    {
        //if (pv.IsMine)
        //    return;
        //if (ErrorInPos.x > ErrorInPos.y) Debug.LogError(gameObject.name +" Pos delta is too long" + ErrorInPos.x);
        //if (ErrorInRot.x > ErrorInRot.y) Debug.LogError(gameObject.name + " rot delta is too long " + ErrorInRot.x);
        rigidbody.position = pos;
        rigidbody.rotation = rot;

        rigidbody.velocity = velocity;
        rigidbody.angularVelocity = angularVelocity;

        CalculateError();
    }
}
