using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : MonoBehaviour
{
    public GameObject Bullet;
    public ShootClass gunobj;
    public List<GameObject> gunPosObject;
    public float bulletLifeTime;
    public float bulletSpeed;
    public AudioSource aus;

    // Use this for initialization
    void Start()
    {
        //gunobj = new MachineGun() { bulletPrefab = Bullet, lifeLength = 8, speed = 5 };
        gunobj = new MachineGun() { bulletPrefab = Bullet, lifeLength = bulletLifeTime, speed = bulletSpeed };
        gunobj.Spawner = new List<GameObject>(gunPosObject);
    }


    float t0 = 0;
    public float Shootdelay = 0.5f;
    float doubleClickDeltaTime;
    private bool shoot;
    int gunId = 0;

    // Update is called once per frame
    void Update()
    {

        //if (Input.GetKey(KeyCode.Q))
        if (shoot || /*DetectDoubleClick() || */Input.GetKey(KeyCode.LeftControl))
        {
            t0 += Time.deltaTime;
            if (t0 > Shootdelay)
            {
                //gunobj.Shoot(-1);
                gunobj.Shoot(++gunId % 2);
                //if (t0 > Shootdelay + ShootLength)
                t0 = 0;
                aus.Play();
            }
        }

        gunobj.Update();

    }
    public void Shoot(bool enable)
    {
        shoot = enable;
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
}
