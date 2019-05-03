using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class playermanager : MonoBehaviour, IOnEventCallback
{
    public static DefaultPlayerPlane PlanePlayer;
    public GameObject playerPrefab;
    //public GameObject player;
    public GameObject MainMenu;
    public GameObject ingameMenu;
    public GameObject ResaultMenu;
    public bool outofgame = true;
    public AudioSource music;
    public List<AudioClip> InGameMusic;
    public float forwardSpeed;
    public float rotateSpeed;




    private readonly byte EndOfRound_event = 0;
    private readonly byte StartGame_event = 1;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PlanePlayer == null)
        {
            //Debug.Log("11111111");
            if (!outofgame)
            {
                Debug.Log("EndOfRound_event calling");
                PhotonNetwork.RaiseEvent(EndOfRound_event, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }
                , new SendOptions { Reliability = true });
                //music.Stop();
                //StartCoroutine(result_show());
                outofgame = true;
                //if (PlayerPrefs.GetInt("LoosecountForAd", 0) > 6)
                //{
                //    PlayerPrefs.SetInt("LoosecountForAd", 0);
                //    StartCoroutine(result_ad());
                //}
                //else
                //{
                //    PlayerPrefs.SetInt("LoosecountForAd", PlayerPrefs.GetInt("LoosecountForAd", 0) + 1);
                //}

                ////Debug.Log("LoosecountForAd=" + PlayerPrefs.GetInt("LoosecountForAd", 0));
            }

        }
        else
        {
            if (PlanePlayer.readytodestroy)
                PlanePlayer = null;
            else
                PlanePlayer.Update();
        }

    }



    public void startGame()
    {
        Debug.Log("startGame RaiseEvent calling button");
        PhotonNetwork.RaiseEvent(StartGame_event, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }
        , new SendOptions { Reliability = true });
        //PhotonNetwork.CurrentRoom.IsOpen = false;
        //PhotonNetwork.CurrentRoom.IsVisible = false;
    }

    public void startGameEvent()
    {

        if (PlanePlayer != null)
        {
            Debug.Log("try to destroy myself");
            PlanePlayer.destroy();
        }


        Debug.Log("startGameEvent function ");
        uiController.Instanse.mainmenu.SetActive(false);
        uiController.Instanse.Ingame.SetActive(true);
        uiController.Instanse.Powerup.SetActive(true);
        uiController.Instanse.waitingroom.SetActive(false);
        



        //PlanePlayer = new DefaultPlayerPlane(forwardSpeed, rotateSpeed, -1, null, playerPrefab);
        //PlanePlayer = Instantiate(playerPrefab).GetComponent<DefaultPlayerPlane>();
        //PlanePlayer = PhotonNetwork.Instantiate(playerPrefab.name,Vector3.zero,Quaternion.identity).GetComponent<DefaultPlayerPlane>();
        Vector3 spawnpoint = new Vector3(Random.Range(-3.0f, 3.0f), 0, Random.Range(-3.0f, 3.0f));
        PlanePlayer = PhotonNetwork.Instantiate(playerPrefab.name, spawnpoint, Quaternion.identity).GetComponent<DefaultPlayerPlane>();
        PlanePlayer.forwardSpeed = forwardSpeed;
        PlanePlayer.rotateSpeed = rotateSpeed;


        music.clip = InGameMusic[Random.Range(0, InGameMusic.Count)];
        music.Play();
        if (PlayerDataClass.Flare <= 2)
            PlayerDataClass.Flare = 3;

        //player = Instantiate(playerPrefab, new Vector3(0, -6, 0), Quaternion.identity) as GameObject;
        outofgame = false;
        uiController.Instanse.resetUIelements();
    }

    IEnumerator result_show()
    {
        yield return new WaitForSeconds(2.5f);
        ingameMenu.SetActive(false);
        ResaultMenu.SetActive(true);
        //yield return new WaitForSeconds(0.5f);
        //IAPandAD.showad("");

    }

    IEnumerator result_ad()
    {
        yield return new WaitForSeconds(2f);
        IAPandAD.showad("");
    }

    public void OnEvent(EventData photonEvent)
    {

        if (photonEvent.Code == EndOfRound_event)
        {
            Debug.Log("OnEvent EndOfRound_event is called " + photonEvent.Code.ToString());
            //Debug.Log("photonEvent.Code == EndOfRound_even");
            //if (PlanePlayer != null)
            //{
            //    Debug.Log("try to destroy myself");
            //    PlanePlayer.planeBodyHit();
            //}                
            //else
                //Debug.Log("PlanePlayer is null");
            //pv.RPC("planeBodyHit", RpcTarget.All);


            music.Stop();
            StartCoroutine(result_show());
            //outofgame = true;
            if (PlayerPrefs.GetInt("LoosecountForAd", 0) > 6)
            {
                PlayerPrefs.SetInt("LoosecountForAd", 0);
                StartCoroutine(result_ad());
            }
            else
            {
                PlayerPrefs.SetInt("LoosecountForAd", PlayerPrefs.GetInt("LoosecountForAd", 0) + 1);
            }

            //Debug.Log("LoosecountForAd=" + PlayerPrefs.GetInt("LoosecountForAd", 0));
        }
        if(photonEvent.Code== StartGame_event)
        {
            Debug.Log("OnEvent StartGame_event is called " + photonEvent.Code.ToString());
            startGameEvent();
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

}
