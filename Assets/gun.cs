using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class gun : MonoBehaviour //MonoBehaviourPun, IPunObservable
{
    public PhotonView pv;
    public GameObject Bullet;
    //public ShootClass gunobj;
    public List<GameObject> gunPosObject;
    public float bulletLifeTime;
    public float bulletSpeed;
    public AudioSource aus;

    private float t0 = 0;
    public float Shootdelay = 0.5f;
    float doubleClickDeltaTime;
    private bool shoot;
    //private int gunId = 0;
    public int damage;

    // Use this for initialization
    void Start()
    {
        //base.Start();
        //pv = GetComponent<PhotonView>();
        //gunobj = new MachineGun() { bulletPrefab = Bullet, lifeLength = 8, speed = 5 };
        //gunobj = new MachineGun() { bulletPrefab = Bullet, lifeLength = bulletLifeTime, speed = bulletSpeed };
        //gunobj.Spawner = new List<GameObject>(gunPosObject);
    }




    // Update is called once per frame
    void Update()
    {

        if (!pv.IsMine && PhotonNetwork.IsConnected)
            return;

        //if (Input.GetKey(KeyCode.Q))
        if (shoot || /*DetectDoubleClick() || */Input.GetKey(KeyCode.LeftControl))
        {
            //Debug.Log("1111");
            t0 += Time.deltaTime;
            if (t0 > Shootdelay)
            {
                firebullet();
            }
        }
        //gunobj.Update();
    }

    private void firebullet()
    {
        pv.RPC("Fire", RpcTarget.All);
        t0 = 0;
        aus.Play();
    }

    [PunRPC]
    public void Fire(PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);

        GameObject Bullet1 = Instantiate(Bullet) as GameObject;
        Bullet1.GetComponent<BulletCode>().Initalize(pv.Owner, bulletSpeed, bulletLifeTime, gunPosObject[0].transform.position, gunPosObject[0].transform.rotation, Mathf.Abs(lag), damage);

        Bullet1 = Instantiate(Bullet) as GameObject;
        Bullet1.GetComponent<BulletCode>().Initalize(pv.Owner, bulletSpeed, bulletLifeTime, gunPosObject[1].transform.position, gunPosObject[1].transform.rotation, Mathf.Abs(lag), damage);
        //gunPosObject[gunId % 2].transform.rotation);

    }
    public void Shoot(bool enable)
    {
        shoot = enable;
        firebullet();
    }


    float firstClickTime;
    float secondClickTime;
    bool DetectDoubleClick()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (firstClickTime == 0)
                firstClickTime = Time.time;
            else
            {
                if (secondClickTime == 0)
                    secondClickTime = Time.time;
                else
                {
                    firstClickTime = secondClickTime = 0;
                    firstClickTime = Time.time;
                }
            }
        }

        doubleClickDeltaTime = secondClickTime - firstClickTime;


        if (Input.GetMouseButton(0) && doubleClickDeltaTime > 0 && doubleClickDeltaTime < 0.3f)
        {
            return true;
        }
        else
        {
            doubleClickDeltaTime = 0;
        }

        return false;
    }

    //public override void Shoot(int SpawnerId, string shooter = "")
    //{
    //    throw new System.NotImplementedException();
    //}


    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    //if (stream.IsWriting)
    //    //{
    //    //    // We own this player: send the others our data
    //    //    stream.SendNext(IsFiring);
    //    //}
    //    //else
    //    //{
    //    //    // Network player, receive data
    //    //    this.IsFiring = (bool)stream.ReceiveNext();
    //    //}
    //}
}
