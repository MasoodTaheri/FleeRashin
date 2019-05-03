using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class luncher : MonoBehaviourPunCallbacks
{
    public string Myname;
    //public string Lobbyname;
    public string roomname;
    public List<string> playersInRoom;
    //public GameObject playerPrefab;
    public int ping;
    public Image pingMeter;
    public Text pingMeter_text;
    public int SerializationRate;
    public GameObject StartButton;
    public Image loading;
    //public GameObject player;
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.SerializationRate = SerializationRate;
    }
    // Use this for initialization
    void Start()
    {


        //Myname = "masood" + Random.Range(0, 100).ToString();
        //PhotonNetwork.NickName = Myname;
        StartButton.SetActive(false);
        loading.gameObject.SetActive(true);
        loading.gameObject.transform.GetChild(0).GetComponent<Text>().text = "Connecting to server ...";

        Myname = "Player" + Random.Range(0, 100).ToString();
        PhotonNetwork.LocalPlayer.NickName = Myname;
        PhotonNetwork.NickName = Myname;

        Connect();

    }


    private int loadingFillDirection = 1;
    // Update is called once per frame
    void Update()
    {
        loading.fillAmount += Time.deltaTime * loadingFillDirection;
        if ((loading.fillAmount >= 1) || (loading.fillAmount <= 0))
        {
            loadingFillDirection *= -1;
            loading.fillClockwise = !loading.fillClockwise;
        }

        //if (Input.GetKeyUp(KeyCode.Q))
        //{
        //    Hashtable props = new Hashtable() { { PLAYER_READY, true } };
        //    PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        //}
        //if (Input.GetKeyUp(KeyCode.W))
        //{
        //    Hashtable props = new Hashtable() { { PLAYER_READY, false } };
        //    PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        //}
    }

    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Joining Room...");
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {

            Debug.Log("Connecting...");

            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.GameVersion = "1";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.NickName = Myname;
        Debug.Log("Connected ");
        //Hashtable props = new Hashtable() { { luncher.PLAYER_READY, true } };
        //PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        PhotonNetwork.JoinRandomRoom();
        InvokeRepeating("getping", 1, 1);
        //Debug.Log("NickName=" + PhotonNetwork.NickName);
        //Debug.Log("LocalPlayer.NickName=" + PhotonNetwork.LocalPlayer.NickName);
        //Myname = PhotonNetwork.NickName;// "masood" + Random.Range(0, 100).ToString();


    }

    void getping()
    {
        ping = PhotonNetwork.GetPing();
        pingMeter_text.text = ping.ToString();
        if (ping > 200) pingMeter.color = Color.red;
        if (ping < 150) pingMeter.color = Color.gray; ;
        if (ping < 100) pingMeter.color = Color.green;
        if (ping == 0) pingMeter.color = Color.black;

        SerializationRate = PhotonNetwork.SerializationRate;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        Debug.Log("OnDisconnected      " + cause.ToString());
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });

    }

    public override void OnCreatedRoom()
    {
        PhotonNetwork.NickName = Myname;
        Debug.Log("OnCreatedRoom");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom CurrentRoom.Name" + PhotonNetwork.CurrentRoom.Name);
        GetAllPlayersInRoom();
        //PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity, 0);
        StartButton.SetActive(true);
        loading.gameObject.SetActive(false);

        loading.gameObject.transform.GetChild(0).GetComponent<Text>().text = "Searching for Oponnent ...";
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom " + newPlayer.NickName);
        GetAllPlayersInRoom();
    }

    public void GetAllPlayersInRoom()
    {
        //roomname = PhotonNetwork.CurrentRoom.Name;
        ////Lobbyname = PhotonNetwork.CurrentLobby.Name;
        //Dictionary<int, Player> allplayers = PhotonNetwork.CurrentRoom.Players;
        //Debug.Log("(allplayers.Count=" + allplayers.Count);


        ////string othersname = "";
        //playersInRoom.Clear();
        //foreach (var pnmae in allplayers)
        //{
        //    //othersname += pnmae.Value.NickName + "   ";
        //    //playersInRoom.Add(pnmae.Value.NickName);
        //    playersInRoom.Add(pnmae.Value.NickName);

        //}
        ////Debug.Log("othersname=" + othersname);

        //Debug.Log("PlayerList.Length=" + PhotonNetwork.PlayerList.Length);
        playersInRoom.Clear();
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            //Debug.LogFormat("player[{0}]  NickName ={1}    UserId={2}   ActorNumber={3}", i, PhotonNetwork.PlayerList[i].NickName
            //    , PhotonNetwork.PlayerList[i].UserId, PhotonNetwork.PlayerList[i].ActorNumber);
            playersInRoom.Add(PhotonNetwork.PlayerList[i].NickName);
        }

    //    foreach (Player p in PhotonNetwork.PlayerList)
    //    {
    //        //Debug.Log("player[" + p.ActorNumber + "].NickName=" + p.NickName);
    //        //Debug.Log("player[" + p.ActorNumber + "].UserId=" + p.UserId);
    //        //Debug.Log("player[" + p.ActorNumber + "].ActorNumber=" + p.ActorNumber);
    //        Debug.LogFormat("player[{0}]  NickName ={1}    UserId={2}   ActorNumber={3}", p.ActorNumber, p.NickName
    //, p.UserId, p.ActorNumber);

    //        //playersInRoom.Add(p.NickName);
    //    }






    }
    public const string PLAYER_READY = "IsPlayerReady";
    //public const string PLAYER_NotREADY = "PlayerNotReady";

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        Debug.Log("OnPlayerPropertiesUpdate " + targetPlayer.NickName);
        object isPlayerReady;
        if (changedProps.TryGetValue(PLAYER_READY, out isPlayerReady))
        {
            Debug.Log(PLAYER_READY + "  " + ((bool)isPlayerReady).ToString());
        }

    }




}
