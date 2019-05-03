using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class WaitingRoomCode : MonoBehaviour
{


    [System.Serializable]
    public class PlayerClass
    {
        public string name;
        public Text PlayerName;
        public Text IsMaster;
        public Button Buttonready;
        public Text StatePlayer;

    }

    public PlayerClass[] players = new PlayerClass[2];
    public Button StartGame;
    public luncher luncher;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("refresh", 1, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnEnable()
    {
        ClearData();
        Hashtable props = new Hashtable() { { luncher.PLAYER_READY, false } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }
    public void ClearData()
    {
        for (int i = 0; i < 2; i++)
        {
            players[i].PlayerName.text = "Waiting....";
            players[i].IsMaster.text = "Waiting....";
            players[i].Buttonready.gameObject.SetActive(false);
            players[i].StatePlayer.gameObject.SetActive(false);

        }
        StartGame.gameObject.SetActive(false);
    }
    public void refresh()
    {
        ClearData();
        bool readystate = true;
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            players[i].PlayerName.text = PhotonNetwork.PlayerList[i].NickName;
            players[i].IsMaster.text = (PhotonNetwork.PlayerList[i].IsMasterClient) ? "[Master]" : "[Client]";

            //players[i].Buttonready.gameObject.SetActive((PhotonNetwork.NickName == PhotonNetwork.PlayerList[i].NickName));
            //players[i].StatePlayer.gameObject.SetActive((PhotonNetwork.NickName != PhotonNetwork.PlayerList[i].NickName));

            object Pready;

            if (PhotonNetwork.PlayerList[i].CustomProperties.TryGetValue(luncher.PLAYER_READY, out Pready))
            {
                readystate = readystate && (bool)Pready;
                if (PhotonNetwork.NickName == PhotonNetwork.PlayerList[i].NickName)
                {
                    if (!(bool)Pready) players[i].Buttonready.gameObject.SetActive(true);
                    else players[i].Buttonready.gameObject.SetActive(false);

                    if (!(bool)Pready) players[i].StatePlayer.gameObject.SetActive(false);
                    else players[i].StatePlayer.gameObject.SetActive(true);
                    if (!(bool)Pready) players[i].StatePlayer.text = "Waiting ...";
                    else players[i].StatePlayer.text = "Ready";

                }
                else
                {
                    players[i].StatePlayer.gameObject.SetActive(false);
                    players[i].StatePlayer.gameObject.SetActive(true);

                    if (!(bool)Pready) players[i].StatePlayer.text = "Waiting ...";
                    else players[i].StatePlayer.text = "Ready";

                }

            }
            else
            {
                readystate = readystate && false;
                players[i].StatePlayer.text = "Waiting ...";
                if (PhotonNetwork.NickName == PhotonNetwork.PlayerList[i].NickName)
                {
                    players[i].Buttonready.gameObject.SetActive(true);
                    players[i].StatePlayer.gameObject.SetActive(false);
                }
                else
                {
                    players[i].Buttonready.gameObject.SetActive(false);
                    players[i].StatePlayer.gameObject.SetActive(true);
                }
            }




        }



        StartGame.gameObject.SetActive((PhotonNetwork.PlayerList.Length > 1) && readystate && (PhotonNetwork.IsMasterClient));
    }

    public void Button_ready(int id)
    {
        Hashtable props = new Hashtable() { { luncher.PLAYER_READY, true } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        players[id].Buttonready.gameObject.SetActive(false);
        players[id].StatePlayer.gameObject.SetActive(true);
    }


}
