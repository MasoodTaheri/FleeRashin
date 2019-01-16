using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class flareDropController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //public arrowskeys arrowkys;
    public Image img;
    public float refillspeed;
    //public float amount;
    //public int maxamount = 8;
    public string direction;
    public Text flare;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (PlayerDataClass.Flare == 0)
        {
            img.fillAmount = 0;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (direction == "Flare")
        {
            if ((PlayerDataClass.Flare > 0) && (img.fillAmount == 1))
            {
                StartCoroutine(playermanager.PlanePlayer.flareDropIE());
                img.fillAmount = 0;
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        if (PlayerDataClass.Flare > 0)
        {
            img.fillAmount = 1;
        }
        else
        {
            img.fillAmount = 0;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerDataClass.Flare > 0)
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
        flare.text = PlayerDataClass.Flare.ToString();
    }
}
