using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class AIManager : MonoBehaviourPunCallbacks
{
    /*
    public GameObject enemyPrefab;
    public GameObject FighterPrefab;
    public GameObject iconPrefab;
    private GameObject[] EnemyCoincollector;
    private Image[] EnemyIconCoincollector;
    private GameObject[] EnemyFighter;
    private Image[] EnemyIconFighter;
    public Canvas canvas;
    public int CoinCollectorCount;
    public int FighterCount;

    // Use this for initialization
    void Start()
    {
        EnemyCoincollector = new GameObject[CoinCollectorCount];
        EnemyIconCoincollector = new Image[CoinCollectorCount];
        EnemyFighter = new GameObject[FighterCount];
        EnemyIconFighter = new Image[FighterCount];
        //GenerateAI(CoinCollectorCount, FighterCount);
        //        StartCoroutine(regenerateMissedPlane());
    }

    // Update is called once per frame
    void Update()
    {
        if (playermanager.PlanePlayer == null)
        {
            for (int i = 0; i < EnemyCoincollector.Length; i++)
                if (EnemyCoincollector[i] != null)
                    Destroy(EnemyCoincollector[i].gameObject);

            for (int i = 0; i < EnemyFighter.Length; i++)
                if (EnemyFighter[i] != null)
                    Destroy(EnemyFighter[i].gameObject);

            return;
        }

        GenerateAI(CoinCollectorCount, FighterCount);

        for (int i = 0; i < EnemyCoincollector.Length; i++)
            if (EnemyCoincollector[i] != null && EnemyIconCoincollector[i] != null)
                UIPOSClass.UIposArrow(EnemyCoincollector[i].transform.position, EnemyIconCoincollector[i]);


        for (int i = 0; i < EnemyFighter.Length; i++)
            if (EnemyFighter[i] != null && EnemyIconFighter[i] != null)
                UIPOSClass.UIposArrow(EnemyFighter[i].transform.position, EnemyIconFighter[i]);

    }

    private void GenerateAI(int coinCollector, int Fighter)
    {
        for (int i = 0; i < CoinCollectorCount; i++)
            if (EnemyCoincollector[i] == null)
            {
                if (EnemyIconCoincollector[i] != null)
                    Destroy(EnemyIconCoincollector[i].gameObject);
                InstatiateAI(true, false, i);
            }

        for (int i = 0; i < FighterCount; i++)
            if (EnemyFighter[i] == null)
            {
                if (EnemyIconFighter[i] != null)
                    Destroy(EnemyIconFighter[i].gameObject);
                InstatiateAI(false, true, i);
            }



        //for (int i = 0; i < coinCollector; i++)
        //    InstatiateAI(true, false, i);
        //for (int i = 0; i < Fighter; i++)
        //    InstatiateAI(false, true, i);
    }

    private void InstatiateAI(bool coinCollector, bool fighter, int ArrayId)
    {
        //Enemy = Instantiate(enemyPrefab, new Vector3(-7.78f, -6, 0.27f), Quaternion.identity);

        GameObject Enemy = Instantiate(fighter ? FighterPrefab : enemyPrefab, Vector3.zero, Quaternion.identity);
        Enemy.transform.position = new Vector3(Random.Range(-50, 50), -6, Random.Range(-50, 50));
        GameObject tmp = Instantiate(iconPrefab) as GameObject;
        Image icon = tmp.GetComponent<Image>();
        tmp.transform.SetParent(canvas.transform, false);


        Enemy.GetComponent<EnemyPlane>().coinCollector = coinCollector;
        Enemy.GetComponent<EnemyPlane>().fightWithPlayer = fighter;

        if (coinCollector)
        {
            EnemyCoincollector[ArrayId] = Enemy;
            EnemyIconCoincollector[ArrayId] = icon;
        }
        else
        {
            EnemyFighter[ArrayId] = Enemy;
            EnemyIconFighter[ArrayId] = icon;
        }
    }

    //IEnumerator regenerateMissedPlane()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(10);
    //        for (int i = 0; i < CoinCollectorCount; i++)
    //            if (EnemyCoincollector[i] == null)
    //            {
    //                Destroy(EnemyIconCoincollector[i].gameObject);
    //                InstatiateAI(true, false, i);
    //            }
    //        for (int i = 0; i < FighterCount; i++)
    //            if (EnemyFighter[i] == null)
    //            {
    //                Destroy(EnemyIconFighter[i].gameObject);
    //                InstatiateAI(false, true, i);
    //            }
    //    }

    //}
    */
    public GameObject Enemy;
    public float forwardSpeed;
    public float rotateSpeed;
    public PlaneColorClass planeColorClass;
    public bool Generate_near;
    public int AICount;
    public int AIInScene;
    public static AIManager instance;
    public bool AllowGenerateAI;

    void Start()
    {
        //Instance = this;
        instance = this;
        planeColorClass = new PlaneColorClass();
    }

    public static void StartGeneratingAI()
    {
        instance.AllowGenerateAI = true;
    }

    void Update()
    {
        if (!AllowGenerateAI)
            return;
        if (!PhotonNetwork.IsMasterClient) return;
        //if (RocketCountInScene == -1)
        //RocketCountInScene = GameObject.FindGameObjectsWithTag("Rocket").Length;

        if (playermanager.PlanePlayer == null)
            return;

        if(AIInScene<AICount)
            instance.GeneratePlane(instance.Enemy, instance.FindSpawnPoint());

    }
    public void InitManager()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        GameObject[] cuAIs = GameObject.FindGameObjectsWithTag("Playerbody");
        AIInScene = cuAIs.Length;
    }



    public void GeneratePlane(GameObject planename, Vector3 spawnpoint)
    {

        EnemyPlane enemy1 = PhotonNetwork.InstantiateSceneObject(planename.name, spawnpoint, Quaternion.identity).GetComponent<EnemyPlane>();
        //enemy1.forwardSpeed = forwardSpeed;// * 0.9f;
        //enemy1.rotateSpeed = rotateSpeed;// * 0.5f; ;
        //enemy1.AIid = id;
        enemy1.GetComponent<PhotonView>().RPC("SetDtat", RpcTarget.AllBufferedViaServer, forwardSpeed.ToString()
            , rotateSpeed.ToString());


        string cl = planeColorClass.GetRandomColorName();
        enemy1.GetComponent<PhotonView>().RPC("SetColor", RpcTarget.AllBufferedViaServer, cl);
        //Debug.Log("enemy color set to" + cl);
        AIInScene++;
    }

    private Vector3 FindSpawnPoint()
    {
        Vector3 spawnpoint;
        if (Generate_near)
            spawnpoint = new Vector3(Random.Range(-3.0f, 3.0f), -6, Random.Range(-3.0f, 3.0f));
        else
            spawnpoint = new Vector3(Random.Range(-30.0f, 30.0f), -6, Random.Range(-30.0f, 30.0f));

        return spawnpoint;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        AllowGenerateAI =false;
        AIInScene = 0;
    }
    public override void OnJoinedRoom()
    {
        AllowGenerateAI = false;
        AIInScene = 0;
        InitManager();
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        //base.OnMasterClientSwitched(newMasterClient);
        Debug.Log("OnMasterClientSwitched on rocketmanager");
        AllowGenerateAI = false;
        InitManager();
    }

    public void AIExpluded()
    {
        AIInScene--;
    }
}
