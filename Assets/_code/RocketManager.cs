using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class UIPOSClass
{
    static Camera cam;
    static Vector3 ret;
    static Vector2 imppos;
    static Vector3 targetDir;
    static float angle;

    public static void UIposArrow(Vector3 target, Image indicator)
    {
        if (indicator == null) return;
        if (cam == null) cam = Camera.main;

        ret = cam.WorldToScreenPoint(target);

        imppos = new Vector2(ret.x, ret.y);

        if (ret.x > Screen.width) imppos.x = Screen.width - 20;
        else if (ret.x < 0) imppos.x = 20;

        if (ret.y > Screen.height) imppos.y = Screen.height - 20;
        else if (ret.y < 0) imppos.y = 20;

        indicator.transform.position = imppos;

        if ((ret.x > 0) && (ret.x < Screen.width) && (ret.y > 0) && (ret.y < Screen.height))
            indicator.enabled = false;
        else
            indicator.enabled = true;

        if (ret.y > Screen.height) indicator.transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            if (ret.y < 0) indicator.transform.rotation = Quaternion.Euler(0, 0, 180);
        else
            if (ret.x > Screen.width) indicator.transform.rotation = Quaternion.Euler(0, 0, -90);
        else if (ret.x < 0) indicator.transform.rotation = Quaternion.Euler(0, 0, 90);

        //Debug.Log(Vector3.Distance(target, cam.transform.position));
        float dist = Vector3.Distance(target, cam.transform.position);
        Color cl = indicator.color;
        if (dist < 50) cl.a = 0.5f;
        if (dist < 20) cl.a = 1;
        if (dist > 50) cl.a = 0.25f;
        indicator.color = cl;

    }
}

public class RocketManager : MonoBehaviourPunCallbacks
{
    public bool allowRockets;
    public static RocketManager instance;
    GameObject player;
    public GameObject RocketPrefab;
    //public Rocket[] rocketlist = new Rocket[5];
    public GameObject RocketRoot;
    //public List<ExplusionClass> explusionList;
    public int RocketCount;
    //public float rotateSpeed;
    //public float forwardSpeed;
    //public float lifetime;
    public int RocketCountInScene;


    // Use this for initialization
    void Start()
    {
        instance = this;

    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        //if (RocketCountInScene == -1)
        //RocketCountInScene = GameObject.FindGameObjectsWithTag("Rocket").Length;

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Playerbody");
            return;
        }

        if (!allowRockets) return;
        //for (int i = 0; i < RocketCount; i++)
        //{
        //    if (rocketlist[i] != null)
        //    {
        //        if (rocketlist[i].readytodestroy)
        //            rocketlist[i] = null;
        //        //else
        //        //    rocketlist[i].Update();
        //    }
        //}


        CheckAndGenerateRockets();

    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        //base.OnMasterClientSwitched(newMasterClient);
        Debug.Log("OnMasterClientSwitched on rocketmanager");
        InitManager();
    }

    public void InitManager()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        GameObject[] curockets = GameObject.FindGameObjectsWithTag("Rocket");
        RocketCountInScene = curockets.Length;
    }

    public void RocketIsExpluded()
    {
        RocketCountInScene--;
    }

    //public void expludeAt(int explusionId, Vector3 pos)
    //{
    //    Destroy(Instantiate(explusionList[explusionId].prefab,
    //pos, Quaternion.identity) as GameObject, 5);
    //}

    private void CheckAndGenerateRockets()
    {
        if (RocketCountInScene < RocketCount)
        {
            PhotonNetwork.InstantiateSceneObject(RocketPrefab.name,
                    new Vector3(0, -6, 0), Quaternion.identity).GetComponent<Rocket>();
            RocketCountInScene++;
        }

        /*
        for (int i = 0; i < RocketCount; i++)
        {
            if (rocketlist[i] == null)
            {
                //Debug.Log("Rocket count=" + );
                //rocketlist[i] = Instantiate(RocketPrefab, spawnpos(), Quaternion.identity) as GameObject;
                //float rotateSpeed = 1;//
                //float forwardSpeed = 3.65f;//
                //float lifetime = 15;//
                //rocketlist[i] = new Rocket(forwardSpeed, rotateSpeed, lifetime, null, RocketPrefab, RocketRoot);
                //rocketlist[i] = Instantiate(RocketPrefab).GetComponent<Rocket>();
                rocketlist[i] = PhotonNetwork.InstantiateSceneObject(RocketPrefab.name,
                    new Vector3(0, -6, 0), Quaternion.identity).GetComponent<Rocket>();
                //rocketlist[i].gameObject.transform.SetParent(RocketRoot.transform);

                //rocketlist[i].forwardSpeed = forwardSpeed;
                //rocketlist[i].rotateSpeed = rotateSpeed;
                //rocketlist[i].lifetime = lifetime;
                //rocketlist[i].gameObject.transform.SetParent(RocketRoot.transform);
            }
        }
        */
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        RocketCountInScene = 0;
    }
    public override void OnJoinedRoom()
    {
        RocketCountInScene = 0;
        InitManager();
    }
}
