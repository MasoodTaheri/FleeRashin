using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CustomController : MonoBehaviour
{
    public DefaultPlayerPlane PlanePlayer;
    public GameObject playerPrefab;

    void Start()
    {
        PhotonNetwork.OfflineMode = true;
        PlanePlayer = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(Random.Range(-3.0f, 3.0f), -6, Random.Range(-3.0f, 3.0f)), Quaternion.identity).GetComponent<DefaultPlayerPlane>();
        PlanePlayer.forwardSpeed = 3;
        PlanePlayer.rotateSpeed = 50;
        //string cl = planeColorClass.GetRandomColorName();
        //PlanePlayer.GetComponent<PhotonView>().RPC("SetColor", RpcTarget.AllBufferedViaServer, cl);
    }
    
    void Update()
    {
        
    }
}
