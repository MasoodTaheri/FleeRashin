using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System;
using UnityEngine.UI;


public abstract class UiCallFunction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public string direction;

    public abstract void OnPointerDown(PointerEventData eventData);
    //{
    //    Debug.Log("OnPointerDown base");
    //    throw new NotImplementedException();
    //}

    public abstract void OnPointerUp(PointerEventData eventData);
    //{
    //    Debug.Log("OnPointerUp base");
    //    throw new NotImplementedException();
    //}
}

public abstract class PowerupButtons : UiCallFunction
{
    public Image img;
    public float refillspeed;
    //public string direction;
    public Text CountUi;
    public override void OnPointerDown(PointerEventData eventData)
    {
        //if (PlayerDataClass.Flare == 0)
        //{
        //    img.fillAmount = 0;
        //}
        if (getvar() == 0)
        {
            img.fillAmount = 0;
        }
    }

    public abstract int getvar();
    public abstract void DoAction();


    public override void OnPointerUp(PointerEventData eventData)
    {
        //if (direction == "Flare")
        {
            if ((getvar() > 0) && (img.fillAmount == 1))
            {
                //StartCoroutine(playermanager.PlanePlayer.flareDropIE());
                DoAction();
                img.fillAmount = 0;
            }
        }
    }

    void Start()
    {
        if (getvar() > 0)
        {
            img.fillAmount = 1;
        }
        else
        {
            img.fillAmount = 0;
        }
    }

    protected void Update()
    {
        if (getvar() > 0)
        {
            if (img.fillAmount < 1)
            {
                img.fillAmount += refillspeed * Time.deltaTime;
                if (img.fillAmount > 1)
                {
                    img.fillAmount = 1;
                }
            }
        }
        else
        {
            img.fillAmount = 0;
        }

        CountUi.text = getvar().ToString();
    }
}

public class arrowskeys : UiCallFunction
{


    public override void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
        if (playermanager.PlanePlayer != null)
        {
            if (direction == "RightArrow")
                playermanager.PlanePlayer.setsterrtoRight(1.0f);

            if (direction == "LeftArrow")
                playermanager.PlanePlayer.setsterrtoleft(1.0f);

            if (direction == "Shoot")
                playermanager.PlanePlayer.StartShoot();
        }
        //movement.PlanePlayer.OnPointerDown(direction);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("OnPointerClick");
        if (playermanager.PlanePlayer != null)
        {
            if (direction == "RightArrow")
                playermanager.PlanePlayer.setsterrtoRight(0.0f);

            if (direction == "LeftArrow")
                playermanager.PlanePlayer.setsterrtoleft(0.0f);

            if (direction == "Flare")
                StartCoroutine(playermanager.PlanePlayer.flareDropIE());

            if (direction == "Shoot")
                playermanager.PlanePlayer.EndShoot();

            if (direction == "DropBomb")
                playermanager.PlanePlayer.DropBomb();


            //if (direction == "ShootRocket")
            //    playermanager.PlanePlayer.ShootRocket();





        }
        //movement.PlanePlayer.OnPointerUp(direction);
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
