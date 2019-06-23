using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;


[System.Serializable]
public class PlayerBindingObjectClass
{
    public GameObject obj;
    public string Nickname;
    public string PlayerID;
    public Player owner;
    public Color cl;

    private static luncher lunch;


    public static void update()
    {
        if (lunch == null)
            lunch = GameObject.Find("Manager").GetComponent<luncher>();
        GameObject[] allPlane = GameObject.FindGameObjectsWithTag("Playerbody");
        if (allPlane.Length != 0)
        {
            lunch.PlayersInRoom2.Clear();
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                PlayerBindingObjectClass pbo = new PlayerBindingObjectClass();
                pbo.Nickname = PhotonNetwork.PlayerList[i].NickName;
                pbo.PlayerID = PhotonNetwork.PlayerList[i].UserId;


                object playerproperty_Color;
                if (PhotonNetwork.PlayerList[i].CustomProperties.TryGetValue("Color", out playerproperty_Color))
                {
                    pbo.cl = playermanager.Instance.planeColorClass.GetColorByNmae((string)playerproperty_Color);
                }


                for (int j = 0; j < allPlane.Length; j++)
                {
                    if (allPlane[j].GetComponent<PhotonView>().Owner == PhotonNetwork.PlayerList[i])
                    {
                        pbo.obj = allPlane[j];
                        pbo.owner = PhotonNetwork.PlayerList[i];
                        break;
                    }
                }
                lunch.PlayersInRoom2.Add(pbo);
            }
        }
    }

    public static GameObject GetObject(Player player)
    {
        if (lunch == null)
            lunch = GameObject.Find("Manager").GetComponent<luncher>();
        foreach (PlayerBindingObjectClass pbo in lunch.PlayersInRoom2)
        {
            if (pbo.owner == player)
                return pbo.obj;
        }

        Debug.LogError("Error getting player obj");
        return null;
    }

    public static void Setcolor(Player player, string _cl)
    {
        Color cl = playermanager.Instance.planeColorClass.GetColorByNmae(_cl);

        Debug.Log(player.NickName + " color=" + _cl);
        if (lunch == null)
            lunch = GameObject.Find("Manager").GetComponent<luncher>();
        foreach (PlayerBindingObjectClass pbo in lunch.PlayersInRoom2)
        {
            if (pbo.owner == player)
                pbo.cl = cl;
        }

        //Debug.LogError("Error getting player obj");
        //return null;
    }


}

public class luncher : MonoBehaviourPunCallbacks
{
    public string Myname;
    //public string Lobbyname;
    public string roomname;
    public List<string> playersInRoom;
    public List<PlayerBindingObjectClass> PlayersInRoom2;
    //public GameObject playerPrefab;
    public int ping;
    public Image pingMeter;
    public Text pingMeter_text;
    public int SerializationRate;
    public GameObject StartButton;
    public Image loading;
    public bool IsMasterClient;
    //public GameObject player;
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.SerializationRate = SerializationRate;
    }
    // Use this for initialization
    void Start()
    {
        //PhotonNetwork.OfflineMode = true;

        //Myname = "masood" + Random.Range(0, 100).ToString();
        //PhotonNetwork.NickName = Myname;
        Myname = "Player" + Random.Range(0, 100).ToString();
        loading.gameObject.SetActive(false);
        StartButton.SetActive(true);
        //Connect();

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
        StartButton.SetActive(false);
        loading.gameObject.SetActive(true);
        loading.gameObject.transform.GetChild(0).GetComponent<Text>().text = "Connecting to server ...";


        PhotonNetwork.LocalPlayer.NickName = Myname;
        PhotonNetwork.NickName = Myname;

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
        StartButton.SetActive(true);
        loading.gameObject.SetActive(false);

        loading.gameObject.transform.GetChild(0).GetComponent<Text>().text = "Searching for Oponnent ...";


    }

    void getping()
    {

        ping = PhotonNetwork.GetPing();
        pingMeter_text.text = ping.ToString();
        if (PhotonNetwork.OfflineMode)
        {
            pingMeter_text.text = "0";
            ping = 10;
        }
        if (ping > 200) pingMeter.color = Color.red;
        if (ping < 150) pingMeter.color = Color.gray;
        if (ping < 100) pingMeter.color = Color.green;
        if (ping == 0) pingMeter.color = Color.black;

        //SerializationRate = PhotonNetwork.SerializationRate;



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
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 20 });

    }

    public override void OnCreatedRoom()
    {
        PhotonNetwork.NickName = Myname;
        Debug.Log("OnCreatedRoom");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom CurrentRoom.Name:" + PhotonNetwork.CurrentRoom.Name);
        GetAllPlayersInRoom();
        //PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity, 0);
        //if (PhotonNetwork.IsMasterClient)
        //    setRoomCustomPropertise();

        playermanager.Instance.startGameEvent();
        IsMasterClient = PhotonNetwork.IsMasterClient;
        //GetAllPlayersInRoom();
    }




    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom " + newPlayer.NickName);
        GetAllPlayersInRoom();
        IsMasterClient = PhotonNetwork.IsMasterClient;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("OnPlayerLeftRoom " + otherPlayer.NickName);
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



    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        //base.OnMasterClientSwitched(newMasterClient);
        Debug.Log("OnMasterClientSwitched");
        Debug.Log("PhotonNetwork.IsMasterClient=" + PhotonNetwork.IsMasterClient);
        IsMasterClient = PhotonNetwork.IsMasterClient;
    }

    //public void Fill_PlayerBindingObjectClass()
    //{
    //    GameObject[] allPlane = GameObject.FindGameObjectsWithTag("Playerbody");
    //    if (allPlane.Length != 0)
    //    {
    //        PlayersInRoom2.Clear();
    //        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
    //        {
    //            PlayerBindingObjectClass pbo = new PlayerBindingObjectClass();
    //            pbo.Nickname = PhotonNetwork.PlayerList[i].NickName;
    //            pbo.PlayerID = PhotonNetwork.PlayerList[i].UserId;

    //            for (int j = 0; j < allPlane.Length; j++)
    //            {
    //                if (allPlane[j].GetComponent<PhotonView>().Owner == PhotonNetwork.PlayerList[i])
    //                {
    //                    pbo.obj = allPlane[j];
    //                    pbo.owner = PhotonNetwork.PlayerList[i];
    //                    break;
    //                }
    //            }
    //            PlayersInRoom2.Add(pbo);
    //        }
    //    }
    //}
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


        object playerproperty;
        if (changedProps.TryGetValue("Color", out playerproperty))
        {
            Debug.Log("Player propertise color update " + (string)playerproperty);
            GameObject go = PlayerBindingObjectClass.GetObject(targetPlayer);
            if (go != null)
            {
                Color cl = playermanager.Instance.planeColorClass.GetColorByNmae((string)playerproperty);
                go.GetComponent<DefaultPlayerPlane>().SetMyColor(cl);
                //PlayerBindingObjectClass.Setcolor(targetPlayer, (string)playerproperty);
            }
            else
                Debug.LogError("cound not find plNE TO SET COLOR");



        }
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        //base.OnRoomPropertiesUpdate(propertiesThatChanged);
        Debug.Log("OnRoomPropertiesUpdate");

    }

    //private void setRoomCustomPropertise()
    //{
    //    Hashtable hash = new Hashtable();
    //    for (int i = 0; i < 10; i++)
    //    //int i = 0;
    //    {
    //        string str = "AI" + i.ToString() + "Color";
    //        //Debug.Log("set sample color = " + playermanager.Instance.planeColorClass.GetRandomColor().ToString()
    //        //    + "for " + str);
    //        hash.Add(str, playermanager.Instance.planeColorClass.GetColorname(
    //            playermanager.Instance.planeColorClass.GetRandomColor()));
    //    }


    //    PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
    //}
}
