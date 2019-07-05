using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class EnemyLab : EnemyPlane
{
    public List<Collider> deactive_list;
    public TextMesh data;
    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}
    public override void Start()
    {
        base.Start();
        forwardSpeed = 0;
        transform.position = new Vector3(
            ((pv.ViewID > 1000) ?
            (pv.ViewID / 1000) :
            (-1 * (pv.ViewID - 1))


            ) * 1.5f, -6, 0);
        foreach (var item in deactive_list)
        {
            item.enabled = false;
        }
        data.text = "";
    }
    protected override void rotate()
    {
        //base.rotate();
    }

    public override void OnTriggerEnter(Collider collision)
    {
        base.OnTriggerEnter(collision);
        if (collision.gameObject.tag == "LOg1")
        {
            Debug.Log("palne" + pv.ViewID + "  " + DateTime.Now + ":" + DateTime.Now.Millisecond);

            //deltatimeClass temp = new deltatimeClass();
            //temp.pvViewId = pv.ViewID;
            //temp.time = CurrentMillis.Millis - StartGame.instance.InitMil;
            //temp.delta = temp.time / 1000.0f;
            //StartGame.instance.deltatime.Add(temp);
            StartGame.instance.deltatime.Add(deltatimeClass.init(pv.ViewID));
            data.text = string.Format("{0}:{1}", DateTime.Now.Second, DateTime.Now.Millisecond);
        }
    }

    //[PunRPC]
    //private void StartGame_Rpc()
    //{
    //            forwardSpeed = 3;
    //}

    //public void ShootRocket_Rpc()
    //{
    //    if (PhotonNetwork.IsMasterClient)
    //    pv.RPC("ShootRocket_Rpc", RpcTarget.All);
    //}
}
