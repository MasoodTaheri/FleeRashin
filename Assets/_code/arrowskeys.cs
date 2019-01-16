using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class arrowskeys : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public string direction;

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("OnPointerClick");
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

    public void OnPointerUp(PointerEventData eventData)
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
