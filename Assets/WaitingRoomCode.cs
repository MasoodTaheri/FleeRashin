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
        public GameObject obj;
        public bool Isready;

        public PlayerClass(GameObject _obj, GameObject root, int row)
        {
            obj = _obj;
            PlayerName = obj.transform.GetChild(0).GetComponent<Text>();
            IsMaster = obj.transform.GetChild(1).GetComponent<Text>();
            Buttonready = obj.transform.GetChild(2).GetComponent<Button>();
            StatePlayer = obj.transform.GetChild(3).GetComponent<Text>();
            obj.transform.SetParent(root.transform, false);

            //obj.transform.position = root.transform.position + new Vector3(0, row * 100, 0);
            obj.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1 * (row + 1) * 100);
        }

        public void SetFields(int playerid)
        {
            PlayerName.text = PhotonNetwork.PlayerList[playerid].NickName;
            name = PhotonNetwork.PlayerList[playerid].NickName;
            IsMaster.text = (PhotonNetwork.PlayerList[playerid].IsMasterClient) ? "[Master]" : "[Client]";


            object Pready;
            if (PhotonNetwork.PlayerList[playerid].CustomProperties.TryGetValue(luncher.PLAYER_READY, out Pready))
            {
                Isready = (bool)Pready;
                if (PhotonNetwork.NickName == PhotonNetwork.PlayerList[playerid].NickName)
                {
                    Buttonready.gameObject.SetActive(!(bool)Pready);
                    StatePlayer.gameObject.SetActive((bool)Pready);
                    StatePlayer.text = (!(bool)Pready) ? "Waiting ..." : "Ready";
                }
                else
                {
                    Buttonready.gameObject.SetActive(false);
                    Buttonready.gameObject.SetActive(true);
                    //if (!(bool)Pready) players[i].StatePlayer.text = "Waiting ...";
                    //else players[i].StatePlayer.text = "Ready";
                    StatePlayer.text = (!(bool)Pready) ? "Waiting ..." : "Ready";
                }
            }
            else
            {
                Isready = false;
                StatePlayer.text = "Waiting ...";
                if (PhotonNetwork.NickName == PhotonNetwork.PlayerList[playerid].NickName)
                {
                    Buttonready.gameObject.SetActive(true);
                    StatePlayer.gameObject.SetActive(false);
                }
                else
                {
                    Buttonready.gameObject.SetActive(false);
                    StatePlayer.gameObject.SetActive(true);
                }
            }

        }

    }

    public List<PlayerClass> players = new List<PlayerClass>();
    public Button StartGame;
    public luncher luncher;
    public GameObject WaitRoomPlayeDataPrefab;
    public GameObject ContentRoot;
    public RectTransform contentRectTransform;

    // Use this for initialization
    void Start()
    {
        //InvokeRepeating("refresh", 1, 1.5f);
        refresh();
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
        //for (int i = 0; i < 2; i++)
        //{
        //    players[i].PlayerName.text = "Waiting....";
        //    players[i].IsMaster.text = "Waiting....";
        //    players[i].Buttonready.gameObject.SetActive(false);
        //    players[i].StatePlayer.gameObject.SetActive(false);

        //}
        for (int i = 0; i < players.Count; i++)
        {
            Destroy(players[i].obj);
            //Destroy(players[i]);
        }
        players.Clear();
        StartGame.gameObject.SetActive(false);
    }
    public void refresh0()
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

    public void refresh()
    {
        ClearData();
        players = new List<PlayerClass>();
        bool readystate = true;
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            GameObject go = Instantiate(WaitRoomPlayeDataPrefab);
            PlayerClass temp = new PlayerClass(go, ContentRoot, i);
            temp.Buttonready.onClick.AddListener(delegate
            {
                Debug.Log("button pressed");
                int id = i;
                Button_ready(id);
            });
            temp.SetFields(i);
            players.Add(temp);
            readystate = readystate && temp.Isready;
        }
        StartGame.gameObject.SetActive((PhotonNetwork.PlayerList.Length > 1) && readystate && (PhotonNetwork.IsMasterClient));
    }

    public void Button_ready(int id)
    {
        Debug.Log("2");
        Hashtable props = new Hashtable() { { luncher.PLAYER_READY, true } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        players[id].Buttonready.gameObject.SetActive(false);
        players[id].StatePlayer.gameObject.SetActive(true);
    }
}
