using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class flareDropController : PowerupButtons
{
    public override void DoAction()
    {
        StartCoroutine(playermanager.PlanePlayer.flareDropIE());
    }

    public override int getvar()
    {
        return PlayerDataClass.Flare;
    }



    //// Use this for initialization
    //void Start()
    //{
    //    //if (PlayerDataClass.Flare > 0)
    //    //{
    //    //    img.fillAmount = 1;
    //    //}
    //    //else
    //    //{
    //    //    img.fillAmount = 0;
    //    //}
    //    init(PlayerDataClass.Flare);

    //}

    // Update is called once per frame
    //void Update()
    //{
    //    UpdateUi(PlayerDataClass.Flare);
    //    //flare.text = PlayerDataClass.Flare.ToString();
    //}
}
