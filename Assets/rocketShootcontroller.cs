using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocketShootcontroller : PowerupButtons
{

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}
    public override void DoAction()
    {
        //throw new System.NotImplementedException();
        playermanager.PlanePlayer.ShootRocket();
    }

    public override int getvar()
    {
        //throw new System.NotImplementedException();
        return 10;
    }
}
