using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PunObjController : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        //Debug.Log("OnPhotonInstantiate " + info.Sender.IsLocal.ToString());
        Debug.Log("OnPhotonInstantiate " + info.photonView.name);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
