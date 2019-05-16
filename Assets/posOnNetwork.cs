using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class posOnNetwork : MonoBehaviourPun, IPunObservable
{

    public Vector3 pos;
    public Vector3 oldpos;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new System.NotImplementedException();
        if (stream.IsWriting)
        {
            if (oldpos != pos)
            {
                stream.SendNext(pos);
                Debug.Log("writepos");
                oldpos = pos;
            }

        }
        else
        {
            Debug.Log("readpos");
            pos = (Vector3)stream.ReceiveNext();
        }

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
