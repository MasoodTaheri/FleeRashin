using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class OnlineRoomSelection : BasePanel
{
    public void StartRoom1()
    {
        PhotonManager.Instance.OnJoinedRoomCallback = () =>
        {
            Debug.Log("OnJoinedRoomCallback StartRoom1");
            GameObject.FindObjectOfType<MainManager>().GetComponent<MainManager>().StartGameOnline();

        };
        PhotonManager.Instance.JoinRandomRoom();
        

    }




}
