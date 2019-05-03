using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class networkRigidbody2 : MonoBehaviourPun, IPunObservable
{
    public Vector3 pos;
    public Vector3 velocity;
    public Quaternion rot;
    public Vector3 angularVelocity;
    public PhotonView pv;
    public Rigidbody rigidbody;
    public float LerpSpeed = 4;
    public float LerpFactor = 1;
    //public float updateTime = 0;
    public float ErrorInPos = 0;
    public float ErrorInRot = 0;
    private DefaultPlayerPlane playercode;
    public bool uselerp;

    // Use this for initialization
    void Start()
    {
        pv = GetComponent<PhotonView>();
        playercode = GetComponent<DefaultPlayerPlane>();
    }

    // Update is called once per frame
    void Update()
    {

        //if (!pv.IsMine)
        //{
        //    LerpFactor = Time.deltaTime * LerpSpeed;
        //    transform.position = Vector3.Lerp(transform.position, pos, LerpFactor);
        //    transform.rotation = Quaternion.Lerp(transform.rotation, rot, LerpFactor);
        //    rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, velocity, LerpFactor);
        //    rigidbody.angularVelocity = Vector3.Lerp(rigidbody.angularVelocity, angularVelocity, LerpFactor);
        //}

    }

    public void FixedUpdate()
    {
        if (!pv.IsMine)
        {
            LerpFactor = Time.fixedDeltaTime * LerpSpeed;

            //Vector3 projectedPosition = pos + velocity * (Time.time - updateTime);
            //transform.position = Vector3.Lerp(transform.position, projectedPosition, LerpFactor);

            ////transform.position = Vector3.Lerp(transform.position, pos, LerpFactor);
            //transform.rotation = Quaternion.Lerp(transform.rotation, rot, LerpFactor);
            //rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, velocity, LerpFactor);
            //rigidbody.angularVelocity = Vector3.Lerp(rigidbody.angularVelocity, angularVelocity, LerpFactor);

            //_rb.position = Vector3.MoveTowards(_rb.position, _networkPosition, Time.fixedDeltaTime);
            //_rb.rotation = Quaternion.RotateTowards(_rb.rotation, _networkRotation, Time.fixedDeltaTime * 100.0f);
            if (!uselerp)
            {
                rigidbody.position = Vector3.MoveTowards(rigidbody.position, pos, Time.fixedDeltaTime);
                rigidbody.rotation = Quaternion.RotateTowards(rigidbody.rotation, rot, Time.fixedDeltaTime * 10.0f);
            }
            else
            {
                rigidbody.position = Vector3.Lerp(rigidbody.position, pos, LerpFactor);
                rigidbody.rotation = Quaternion.Lerp(rigidbody.rotation, rot, LerpFactor * 2);

                rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, velocity, LerpFactor);
                rigidbody.angularVelocity = Vector3.Lerp(rigidbody.angularVelocity, angularVelocity, LerpFactor);

            }


            ErrorInPos = Vector3.Distance(transform.position, pos);
            ErrorInRot = Quaternion.Angle(transform.rotation, rot);

        }

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            pos = rigidbody.position;
            velocity = rigidbody.velocity;
            rot = rigidbody.rotation;
            angularVelocity = rigidbody.angularVelocity;

            stream.SendNext(pos);
            stream.SendNext(velocity);
            stream.SendNext(rot);
            stream.SendNext(angularVelocity);
            if (playercode != null)
                stream.SendNext(playercode.Health);



        }
        else
        {
            pos = (Vector3)stream.ReceiveNext();
            rigidbody.velocity = (Vector3)stream.ReceiveNext();
            rot = (Quaternion)stream.ReceiveNext();
            rigidbody.angularVelocity = (Vector3)stream.ReceiveNext();
            //updateTime = Time.time;

            //float lag = Mathf.Abs((float)(PhotonNetwork.time - info.timestamp));
            //_networkPosition += (_rb.velocity * lag);

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));
            pos += rigidbody.velocity * lag;
            if (playercode != null)
                playercode.Health = (int)stream.ReceiveNext();
        }

    }
}
