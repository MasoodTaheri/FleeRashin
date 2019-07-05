using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using System;
using System.IO;




/// <summary>Class to get current timestamp with enough precision</summary>
static class CurrentMillis
{
    private static readonly DateTime Jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    /// <summary>Get extra long current timestamp</summary>
    public static long Millis { get { return (long)((DateTime.UtcNow - Jan1St1970).TotalMilliseconds); } }
}

[System.Serializable]
public class deltatimeClass
{
    public int pvViewId;
    public long time;
    //public float delta;
    public string str_time;
    public static deltatimeClass init(int name)
    {
        deltatimeClass temp = new deltatimeClass();
        temp.pvViewId = name;
        temp.time = CurrentMillis.Millis;
        //temp.delta = CurrentMillis.Millis - StartGame.instance.InitMil;
        //temp.time / 1000.0f;
        temp.str_time = string.Format("{0}:{1}", DateTime.Now.Second, DateTime.Now.Millisecond);
        StartGame.instance.FileContent += string.Format("{0},{1},{2},{3}\n",
        name, CurrentMillis.Millis.ToString(), DateTime.Now.Second, DateTime.Now.Millisecond);
        return temp;
    }
}


public class StartGame : MonoBehaviour, IOnEventCallback
{
    public Button StartGameBut;
    public List<deltatimeClass> deltatime;
    public static StartGame instance;
    public long startdelay = 0;
    public int InitDateTimesec = 0;
    public long InitDateTimeMil = 0;
    public string FileContent;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("CheckForBeingMaster", 1, 3);
        instance = this;
        Invoke("writePlease", 30);

    }

    // Update is called once per frame
    void Update()
    {

    }
    void CheckForBeingMaster()
    {
        StartGameBut.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }
    private readonly byte StartLab_event = 3;
    public void StartLab()
    {
        Debug.Log("StartLab");
        //playermanager.PlanePlayer.pv.RPC("StartGame_Rpc", RpcTarget.All);
        object[] content = new object[] { CurrentMillis.Millis.ToString() , DateTime.Now.Second,
        DateTime.Now.Millisecond};
        PhotonNetwork.RaiseEvent(StartLab_event, content,
            new RaiseEventOptions { Receivers = ReceiverGroup.All }
        , new SendOptions { Reliability = true });
    }



    public void OnEvent(EventData photonEvent)
    {
        //Debug.Log("OnEvent");
        if (photonEvent.Code == StartLab_event)
        {
            FileContent = CurrentMillis.Millis.ToString() + "  time of getting event\n";//

            object[] data = (object[])photonEvent.CustomData;
            string senderMillisecond = (string)data[0];
            startdelay = CurrentMillis.Millis - long.Parse(senderMillisecond);


            InitDateTimesec = (int)data[1];
            InitDateTimeMil = (int)data[2];
            FileContent += senderMillisecond + "  time of masterClient start event\n";//


            deltatime = new List<deltatimeClass>();
            Debug.Log("OnEvent StartLab_event is called " + photonEvent.Code.ToString());
            //startGameEvent();
            //PhotonNetwork.JoinRandomRoom();
            DefaultPlayerPlane dpp;
            foreach (GameObject plane in GameObject.FindGameObjectsWithTag("Playerbody"))
            {
                //plane.GetComponent<DefaultPlayerPlane>().forwardSpeed = 3;
                dpp = plane.GetComponent<DefaultPlayerPlane>();
                if (dpp.pv.IsMine)
                {
                    dpp.forwardSpeed = 3;
                }

            }
        }
    }

    public void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }


    public void writePlease()
    {
        File.WriteAllText(Application.persistentDataPath + "\\data.csv", FileContent);
    }

}
