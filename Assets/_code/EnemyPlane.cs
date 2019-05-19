using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class EnemyPlane : AIPlane
{
    public GameObject _target;
    public GameObject Bullet;
    //public ShootClass gun;
    public List<GameObject> gunPosObject;
    public bool coinCollector;
    public bool fightWithPlayer;
    //public List<GameObject> bulleGenerated;
    public SpriteRenderer planeImage;
    public List<Sprite> planeImages;

    public float bulletLifeTime;
    public float bulletSpeed;
    public AudioSource aus;
    public AudioClip auc;
    public float distancetoshoot;
    public float distance;

    public override void onCollide(Collision collision)
    {
        throw new NotImplementedException();
    }

    public override void TargetDetection()
    {
        //if (_target == null) return;
        //target = _target.transform.position;
        //if (playermanager.PlanePlayer == null) return;
        //if (playermanager.PlanePlayer.obj == null) return;

        //target = playermanager.PlanePlayer.obj.transform.position;



        //float ang = Vector3.Dot(playermanager.PlanePlayer.obj.transform.forward
        //    , this.gameObject.transform.forward);
        //Debug.LogFormat("ang{0}", ang);


        if (coinCollector)
        {
            if (_target == null)
            {
                //if (pickupmanagers.instance != null)
                //{
                //    if (pickupmanagers.instance.Collectableobjs.Length > 0)
                //    {
                //        _target = pickupmanagers.instance.Collectableobjs
                //            [UnityEngine.Random.Range(0, pickupmanagers.instance.Collectableobjs.Length)];
                //        if (_target != null)
                //            target = _target.transform.position;
                //    }

                //}
                throw new System.NotImplementedException();
            }
        }

        if (fightWithPlayer)
        {
            if (playermanager.PlanePlayer == null) return;
            if (playermanager.PlanePlayer == null) return;

            target = playermanager.PlanePlayer.transform.position;

        }

    }

    //// Use this for initialization
    void Start()
    {
        base.Start();
        //gun = new MachineGun() { bulletPrefab = Bullet, lifeLength = bulletLifeTime, speed = bulletSpeed };
        //gun.Spawner = new List<GameObject>(gunPosObject);
        planeImage.sprite = planeImages[UnityEngine.Random.Range(0, planeImages.Count)];
        //bulleGenerated = gun.bulltes;
    }
    float t0 = 0;
    float startShootdelay = 1f;
    float ang = 0;
    //// Update is called once per frame
    void Update()
    {
        base.Update();
        if (playermanager.PlanePlayer != null)
            if (playermanager.PlanePlayer != null)
            {
                distance = Vector3.Distance(playermanager.PlanePlayer.transform.position, transform.position);
                if (distance > distancetoshoot)
                    return;
            }

        {
            if (playermanager.PlanePlayer != null)
                if (playermanager.PlanePlayer != null)
                    ang = Vector3.Dot(playermanager.PlanePlayer.transform.forward
           , this.gameObject.transform.forward);

            //if (ang > 0.5f)
            //{
            //    t0 += Time.deltaTime;
            //    if (t0 > startShootdelay)
            //    {
            //        gun.Shoot(-1);
            //        aus.PlayOneShot(auc);

            //        //if (t0 > Shootdelay + ShootLength)
            //        t0 = 0;
            //    }
            //}

        }


        //gun.Update();

    }

    protected void OnCollisionEnter(Collision other)
    {
        //Debug.Log("EnemyPlane hit " + other.gameObject.tag);

        if (other.gameObject.tag == "Star")
        {
            GameObject.Destroy(other.gameObject);
        }
        else
        {
            if (other.gameObject.tag == "Playerbody")
            {
                playermanager.PlanePlayer.destroy();
                GameObject.Destroy(this.gameObject);
            }
            else
                GameObject.Destroy(this.gameObject);
        }

    }

    private void OnDestroy()
    {
        //RocketManager.instance.expludeAt(1, transform.position);
        throw new System.NotImplementedException();
        
        //if (Vector3.Distance(playermanager.PlanePlayer.obj.transform.position
        //    , transform.position) < 10)
        //uiController.Instanse.IncPlaneHit();
    }

}
