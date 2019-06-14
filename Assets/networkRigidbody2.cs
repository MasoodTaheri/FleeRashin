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
    public float PosLerpSpeed = 4;
    public float PosLerpFactor = 1;
    public float RotLerpSpeed = 4;
    public float RotLerpFactor = 1;
    //public float updateTime = 0;
    public Vector2 ErrorInPos;
    public Vector2 ErrorInRot;
    private DefaultPlayerPlane playercode;
    public bool uselerp;

    //bool initOnce = false;
    // Use this for initialization
    protected void Start()
    {
        pv = GetComponent<PhotonView>();
        playercode = GetComponent<DefaultPlayerPlane>();
    }

    // Update is called once per frame
    protected void Update()
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

    protected void FixedUpdate()
    {
        if (!pv.IsMine)
        {
            PosLerpFactor = Time.fixedDeltaTime * PosLerpSpeed;
            RotLerpFactor = Time.fixedDeltaTime * RotLerpSpeed;

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
                rigidbody.position = Vector3.Lerp(rigidbody.position, pos, PosLerpFactor);
                rigidbody.rotation = Quaternion.Lerp(rigidbody.rotation, rot, RotLerpFactor);

                rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, velocity, PosLerpFactor);
                rigidbody.angularVelocity = Vector3.Lerp(rigidbody.angularVelocity, angularVelocity, PosLerpFactor);

            }

            CalculateError();
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

            //if (!initOnce)
            //{
            //    if (playercode is EnemyPlane)
            //    {
            //        stream.SendNext((playercode as EnemyPlane).AIid);
            //        Debug.Log("send aiid");
            //    }
            //    initOnce = true;
            //}
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

            //if (!initOnce)
            //{
            //    if (playercode is EnemyPlane)
            //    {
            //        (playercode as EnemyPlane).AIid = (int)stream.ReceiveNext();
            //        Debug.Log("receive  aiid");
            //    }
            //    initOnce = true;
            //}

        }
    }

    public void CalculateError()
    {
        ErrorInPos.x = Vector3.Distance(transform.position, pos);
        ErrorInRot.y = Quaternion.Angle(transform.rotation, rot);

    }
}
