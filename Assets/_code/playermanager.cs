using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;


[System.Serializable]
public class PlaneColorClass
{
    public List<Color> playercolors;
    public List<string> playercolorsName;


    void fillcolors()
    {
        playercolors.Add(Color.white); playercolorsName.Add("white");
        playercolors.Add(Color.red); playercolorsName.Add("red");
        playercolors.Add(Color.green); playercolorsName.Add("green");
        playercolors.Add(Color.blue); playercolorsName.Add("blue");
        //playercolors.Add(Color.black); playercolorsName.Add("black");
        playercolors.Add(Color.yellow); playercolorsName.Add("yellow");
        playercolors.Add(Color.magenta); playercolorsName.Add("magenta");
        playercolors.Add(Color.grey); playercolorsName.Add("grey");
        playercolors.Add(Color.cyan); playercolorsName.Add("cyan");
    }

    public PlaneColorClass()
    {
        playercolors = new List<Color>();
        playercolorsName = new List<string>();
        fillcolors();
    }

    public Color GetRandomColor()
    {
        return playercolors[Random.Range(0, playercolors.Count)];
    }

    public string GetRandomColorName()
    {
        Color cl = GetRandomColor();
        return GetColorname(cl);
    }
    public string GetColorname(Color cl)
    {
        for (int i = 0; i < playercolors.Count; i++)
            if (playercolors[i] == cl)
                return playercolorsName[i];

        Debug.LogError("Cannot find colo with value" + cl.ToString());
        return "";
    }

    public Color GetColorByNmae(string colorName)
    {
        for (int i = 0; i < playercolorsName.Count; i++)
            if (playercolorsName[i] == colorName)
                return playercolors[i];


        //foreach (Color cl in playercolors)
        //    if (cl.ToString() == colorName)
        //        return cl;

        Debug.LogError("Cannot find clor with name=" + colorName);
        return Color.black;
    }
}

public class playermanager : MonoBehaviour, IOnEventCallback
{
    public static DefaultPlayerPlane PlanePlayer;
    public GameObject playerPrefab;
    //public GameObject Enemy;
    //public GameObject player;
    //public GameObject MainMenu;
    //public GameObject ingameMenu;
    //public GameObject ResaultMenu;
    public bool outofgame = true;
    public AudioSource music;
    public List<AudioClip> InGameMusic;
    public float forwardSpeed;
    public float rotateSpeed;
    public GameObject puncoin;
    public static playermanager Instance;
    public PlaneColorClass planeColorClass;
    public int AICount;
    public bool isGameStarted;



    private readonly byte EndOfRound_event = 0;
    private readonly byte StartGame_event = 1;
    public static readonly byte pickupItemGeneration = 2;

    public bool Generate_near;

    // Use this for initialization
    void Start()
    {
        Instance = this;
        planeColorClass = new PlaneColorClass();
        SceneManager.sceneLoaded += CustomOnLevelWasLoaded;
        startGameEvent();
    }

    bool dontShowGameOverPanel = false;
    void CustomOnLevelWasLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        dontShowGameOverPanel = true;
        StartCoroutine(CanShowGameOverPanel());
    }
    IEnumerator CanShowGameOverPanel()
    {
        yield return new WaitForSeconds(3f);
        dontShowGameOverPanel = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlanePlayer == null)
        {
            //Debug.Log("11111111");
            if (isGameStarted && !outofgame)
            {
                //Debug.Log("EndOfRound_event calling");
                //PhotonNetwork.RaiseEvent(EndOfRound_event, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }
                //, new SendOptions { Reliability = true });
                GameFinished();
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

            //if (Input.GetKeyUp(KeyCode.Q))
            //{
            //    // PhotonNetwork.Instantiate(puncoin.name, Vector3.zero, Quaternion.identity);
            //    GetComponent<Pickupmanager2>().GenerateItem();
            //}
            //if (Input.GetKeyUp(KeyCode.W))
            //{
            //    // PhotonNetwork.Instantiate(puncoin.name, Vector3.zero, Quaternion.identity);
            //    GetComponent<Pickupmanager2>().RemoveAll();
            //}


        }
        else
        {
            if (PlanePlayer.readytodestroy)
                PlanePlayer = null;
            else
                ;// PlanePlayer.Update();
        }

        if (Input.GetKeyUp(KeyCode.Q))
            EndGame();

    }

    public void EndGame()
    {
        PlanePlayer.destroy();
    }

    public void startGame()
    {
        //Debug.Log("startGame RaiseEvent calling button");
        //PhotonNetwork.RaiseEvent(StartGame_event, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }
        //, new SendOptions { Reliability = true });
        //PhotonNetwork.CurrentRoom.IsOpen = false;
        //PhotonNetwork.CurrentRoom.IsVisible = false;
        //startGameEvent();
        //PhotonNetwork.JoinRandomRoom();
        isGameStarted = true;
        //GetComponent<PhotonManager>().Connect();
    }

    //public void startGameOffline()
    //{
    //    //PhotonNetwork.Disconnect();
    //    isGameStarted = true;
    //    PhotonNetwork.OfflineMode = true;
    //    //PhotonNetwork.JoinRandomRoom();
    //}

    public void startGameEvent()
    {
        //Debug.Log("startGameEvent function ");

        if (PlanePlayer != null)
        {
            Debug.Log("try to destroy myself");
            PlanePlayer.destroy();
        }
        GetComponent<Pickupmanager2>().GenerateItem();


        /*uiController.Instanse.mainmenu.SetActive(false);
        uiController.Instanse.Ingame.SetActive(true);
        uiController.Instanse.Powerup.SetActive(true);
        uiController.Instanse.waitingroom.SetActive(false);*/

        GetComponent<RocketManager>().allowRockets = true;

        Vector3 spawnpoint = FindSpawnPoint();

        PlanePlayer = PhotonNetwork.Instantiate(playerPrefab.name, spawnpoint, Quaternion.identity).GetComponent<DefaultPlayerPlane>();
        PlanePlayer.forwardSpeed = forwardSpeed;
        PlanePlayer.rotateSpeed = rotateSpeed;
        string cl = planeColorClass.GetRandomColorName();
        PlanePlayer.GetComponent<PhotonView>().RPC("SetColor", RpcTarget.AllBufferedViaServer, cl);
        Camera.main.GetComponent<CameraMovement>().AddTarget(PlanePlayer.gameObject);
        //Debug.Log("PlanePlayer color set to" + cl);


        /*music.clip = InGameMusic[Random.Range(0, InGameMusic.Count)];
        music.Play();
        if (PlayerDataClass.Flare <= 2)
            PlayerDataClass.Flare = 3;

        outofgame = false;
        uiController.Instanse.resetUIelements();*/

        //if (PhotonNetwork.IsMasterClient)
        //    for (int i = 0; i < AICount; i++)
        //    {
        //        GeneratePlane(Enemy, FindSpawnPoint(), i);
        //    }

        AIManager.StartGeneratingAI();
    }

    private Vector3 FindSpawnPoint()
    {
        //if (FindObjectOfType<LevelManager>() != null)

        //if (LevelManager.Instance == null)
        //    Debug.LogError("No \"LevelManager\" was found.");

        //return LevelManager.Instance.startingPoint;


        Vector3 spawnpoint;// = new Vector3(50f, -6f, -8f);
        if (Generate_near)
            spawnpoint = new Vector3(Random.Range(-3.0f, 3.0f), -6, Random.Range(-3.0f, 3.0f));
        else
            spawnpoint = new Vector3(Random.Range(-30.0f, 30.0f), -6, Random.Range(-30.0f, 30.0f));

        return spawnpoint;
    }

    //public void GeneratePlane(GameObject planename, Vector3 spawnpoint, int id)
    //{

    //    EnemyPlane enemy1 = PhotonNetwork.InstantiateSceneObject(planename.name, spawnpoint, Quaternion.identity).GetComponent<EnemyPlane>();
    //    //enemy1.forwardSpeed = forwardSpeed;// * 0.9f;
    //    //enemy1.rotateSpeed = rotateSpeed;// * 0.5f; ;
    //    //enemy1.AIid = id;
    //    enemy1.GetComponent<PhotonView>().RPC("SetDtat", RpcTarget.AllBufferedViaServer, forwardSpeed.ToString()
    //        , rotateSpeed.ToString());


    //    string cl = planeColorClass.GetRandomColorName();
    //    enemy1.GetComponent<PhotonView>().RPC("SetColor", RpcTarget.AllBufferedViaServer, cl);
    //    //Debug.Log("enemy color set to" + cl);
    //}

    IEnumerator result_show()
    {
        GetComponent<RocketManager>().allowRockets = false;
        yield return new WaitForSeconds(2.5f);
        /*ingameMenu.SetActive(false);
        ResaultMenu.SetActive(true);*/
        if (!dontShowGameOverPanel)
        {
            FindObjectOfType<UIManager>().ChangePanel(BasePanel.PanelType.GameOver);
            outofgame = false;
        }
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
            GameFinished();
        }
        //if (photonEvent.Code == StartGame_event)
        //{
        //    Debug.Log("OnEvent StartGame_event is called " + photonEvent.Code.ToString());
        //    //startGameEvent();
        //    PhotonNetwork.JoinRandomRoom();
        //}

        if (photonEvent.Code == pickupItemGeneration)
        {
            object[] data = (object[])photonEvent.CustomData;

            //Vector3 targetPosition = (Vector3)data[0];

        }
    }

    private void GameFinished()
    {
        //music.Stop();
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

        //PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        //Debug.Log("LoosecountForAd=" + PlayerPrefs.GetInt("LoosecountForAd", 0));
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
