using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class gun : MonoBehaviour 
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
    public DefaultPlayerPlane plane;
    public int damage;
    float firstClickTime;
    float secondClickTime;

    GameObject target;
    [HideInInspector]
    public bool forceShoot;

    void Start()
    {
    }
    
    void Update()
    {
        if (!plane.IsControllable())
            return;
        
        if (!PhotonNetwork.IsConnected)
            return;
        
        if (shoot || (!(plane is EnemyPlane) && Input.GetKey(KeyCode.LeftControl)) || forceShoot)
        {
            t0 += Time.deltaTime;
            if (t0 > Shootdelay)
            {
                FireBullet();
            }
        }
    }

    private void FireBullet()
    {
        pv.RPC("Fire", RpcTarget.All);
        t0 = 0;
        aus.Play();
    }

    [PunRPC]
    public void Fire(PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);
        
        if (target == null)
        {
            for (int i = 0; i < gunPosObject.Count; i++)
                gunPosObject[i].transform.rotation = transform.rotation;
        }
        else
        {
            Vector3 lookPos;
            for (int i = 0; i < gunPosObject.Count; i++)
            {
                lookPos = CorrectPosition(target.transform.position) + 2 * (CorrectPosition(target.transform.position) - transform.position).normalized;
                Rigidbody rb = target.GetComponent<Rigidbody>();
                lookPos += (rb != null && !rb.isKinematic && rb.velocity.magnitude > 0) ? rb.velocity * (Vector3.Distance(CorrectPosition(target.transform.position), transform.position) / bulletSpeed) : Vector3.zero;
                gunPosObject[i].transform.LookAt(lookPos);
                //gunPosObject[i].transform.LookAt(CorrectPosition(target.transform.position) + ((target.GetComponent<Rigidbody>() != null && !target.GetComponent<Rigidbody>().isKinematic) ? target.transform.forward : 2 * (CorrectPosition(target.transform.position) - transform.position).normalized));
            }
        }

        GameObject Bullet1 = Instantiate(Bullet) as GameObject;
        Bullet1.GetComponent<BulletCode>().Initalize(pv.Owner, bulletSpeed, bulletLifeTime, gunPosObject[0].transform.position, gunPosObject[0].transform.rotation, Mathf.Abs(lag), damage);
        Bullet1 = Instantiate(Bullet) as GameObject;
        Bullet1.GetComponent<BulletCode>().Initalize(pv.Owner, bulletSpeed, bulletLifeTime, gunPosObject[1].transform.position, gunPosObject[1].transform.rotation, Mathf.Abs(lag), damage);

    }

    public void Shoot(bool enable)
    {
        shoot = enable;
    }

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

    Vector3 CorrectPosition(Vector3 input)
    {
        input = new Vector3(input.x, transform.position.y, input.z);
        return input;
    }

    public void SetTarget(GameObject _target)
    {
        target = _target;
    }
}
