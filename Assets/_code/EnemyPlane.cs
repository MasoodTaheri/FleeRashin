using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/*
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
*/


[System.Serializable]
public class TargetDataClass
{
    [System.Serializable]
    public class targetinfo
    {
        public GameObject obj;
        public float distance;
        public float target_value;
        public float valueToSelect;
        public bool Attackable;
    }

    public targetinfo CurrentTarget;
    public List<targetinfo> targetlist;
    public GameObject plane;

    public TargetDataClass()
    {
        targetlist = new List<targetinfo>();
        //plane = _plane;
    }
    public void Addtarget(GameObject[] obj, float value, bool _attackable)
    {
        for (int i = 0; i < obj.Length; i++)
            Addtarget(obj[i], value, _attackable);
    }

    public void Cleartargets()
    {
        targetlist = new List<targetinfo>();
    }


    public void Addtarget(GameObject obj, float value, bool _attackable)
    {
        if (obj.GetInstanceID() == plane.GetInstanceID())
            return;


        //if there is this instance 
        for (int i = 0; i < targetlist.Count; i++)
        {
            if (obj.GetInstanceID() == targetlist[i].obj.GetInstanceID())
                return;
        }

        targetinfo tinfo = new targetinfo();
        tinfo.obj = obj;
        tinfo.target_value = value;
        tinfo.Attackable = _attackable;
        targetlist.Add(tinfo);
        updateInfo();
    }
    public void updateInfo()
    {
        for (int i = 0; i < targetlist.Count; i++)
        {
            if (targetlist[i].obj == null)
            {
                targetlist.RemoveAt(i);
            }
            else
            {
                targetlist[i].distance = Vector3.Distance(targetlist[i].obj.transform.position, plane.transform.position);
                targetlist[i].valueToSelect = targetlist[i].distance;
            }

        }
    }

    public GameObject SelectRandomTarget()
    {
        updateInfo();
        int rnd = UnityEngine.Random.Range(0, targetlist.Count);
        CurrentTarget = targetlist[rnd];
        return targetlist[rnd].obj;
    }
}

//public enum AIStateEnum{ Idle, Seek, Evade}
public class EnemyPlane : DefaultPlayerPlane
{
    public GameObject _target;
    public TargetDataClass Targets;
    public float ShootDistance;
    //public int AIid;
    //public AIStateEnum State;
    //public bool coinCollector;
    //public bool fightWithPlayer;
    //public AudioSource aus;
    //public AudioClip auc;

    public override bool IsControllable()
    {
        //Debug.Log("IsControllable by EnemyPlane" + PhotonNetwork.IsMasterClient);
        //Debug.Log("IsMasterClient=" + PhotonNetwork.IsMasterClient, this.gameObject);

        //ControlableOnThisDevice = PhotonNetwork.IsMasterClient;
        //playerControlable = false;
        return PhotonNetwork.IsMasterClient;
    }

    //float Distance_to_target_is_near = 10.0f;
    //bool isNearToTarget()
    //{

    //    if 
    //    if (Vector3.Distance(transform.position, _target.transform.position) < Distance_to_target_is_near)
    //        return true;
    //    else
    //        return false;
    //}


    public void TargetDetection()
    {
        //Debug.Log("TargetDetection");
        Targets.Cleartargets();
        Targets.Addtarget(GameObject.FindGameObjectsWithTag("Playerbody"), 2, true);
        Targets.Addtarget(GameObject.FindGameObjectsWithTag("Star"), 1, false);

        _target = Targets.SelectRandomTarget();

        //List<GameObject> targetstemp = new List<GameObject>();
        //targetstemp.AddRange(GameObject.FindGameObjectsWithTag("Playerbody"));
        //for (int i = 0; i < targetstemp.Count; i++)
        //{
        //    Targets.Addtarget(targetstemp[i], 2);
        //}
        //targets.AddRange(GameObject.FindGameObjectsWithTag("Star"));
        //Debug.Log("search for target from " + targets.Count + "  target");


        //select target randomly
        //_target = targets[UnityEngine.Random.Range(0, targets.Count)];




    }

    public bool IsInAttackState()
    {
        Vector3 Agentforward = transform.forward;
        Vector3 agentToTarget = _target.transform.position - transform.position;
        Debug.DrawRay(transform.position, Agentforward, Color.blue);
        Debug.DrawRay(transform.position, agentToTarget, Color.red);

        float dot = Vector3.Dot(Agentforward, agentToTarget);
        if (dot < 0) return false;
        else
        if (Vector3.Angle(Agentforward, agentToTarget) > 40)
            return false;

        return true;
    }


    protected override void rotate()
    {
        //Debug.Log("rotate");
        if (_target == null)
        {
            //Debug.Log("_target is null");

            //TargetDetection();
            return;
        }

        ////transform.Rotate(0, rotateSpeed * Time.deltaTime * (Rightweight - Leftweight), 0);
        //Vector3 relativePos = _target.transform.position - transform.position;
        //Quaternion rotation = Quaternion.LookRotation(relativePos);

        //transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotateSpeed);

        Vector3 Agentforward = transform.forward;
        Vector3 agentToTarget = _target.transform.position - transform.position;
        Debug.DrawRay(transform.position, Agentforward, Color.blue);
        Debug.DrawRay(transform.position, agentToTarget, Color.red);
        if (Vector3.Angle(Agentforward, agentToTarget) < 5)
            return;


        float dot = Vector3.Dot(Agentforward, agentToTarget);



        Vector3 cross = Vector3.Cross(Agentforward, agentToTarget);
        Debug.DrawRay(transform.position, cross, Color.yellow);

        float crosslength = Vector3.Dot(cross, transform.up);
        // crosslength<0 turn left
        // crosslength >0 turn right


        //dot>0 target in front
        //dot<0 tatget is in  back


        Rightweight = (Mathf.Sign(crosslength) + RotateRightFactor) * rotateFactor.y;
        Leftweight = (-1 * Mathf.Sign(crosslength) + RotateLeftFactor) * rotateFactor.x;

        //transform.Rotate(0, rotateSpeed * Time.deltaTime * (Mathf.Sign(crosslength)), 0);
        transform.Rotate(0, rotateSpeed * Time.deltaTime * (Rightweight - Leftweight), 0);

    }


    private void Attack(bool IsinAttackState)
    {
        if (planeGun == null)
            planeGun = GetComponent<gun>();

        if (!Targets.CurrentTarget.Attackable)
        {
            planeGun.Shoot(false);
            return;
        }


        if (IsinAttackState)
        {
            //planeGun.Shoot(true);
            //
            if (_target != null)
                planeGun.Shoot((Vector3.Distance(transform.position,
                    _target.transform.position) < ShootDistance));
        }
        else
        {
            planeGun.Shoot(false);
        }
    }

    float t0 = 0;
    float TimeToChangetarget = 10;

    public override void Update()
    {
        base.Update();
        if (!IsControllable() && PhotonNetwork.IsConnected)
            return;
        //Debug.Log("updateAI");
        Targets.updateInfo();
        if (_target == null || Input.GetKeyUp(KeyCode.Q))
        {
            //Debug.Log("_target is null");
            TargetDetection();
            return;
        }
        //ChangeState();

        Attack(IsInAttackState());

        t0 += Time.deltaTime;
        if (t0 > TimeToChangetarget)
        {
            TimeToChangetarget = 10 + UnityEngine.Random.Range(-4.0f, 4.0f);
            t0 = 0;
            //if (!isNearToTarget())
            TargetDetection();
        }
    }



    //float ChangeStateTime = 0;
    //public void ChangeState()
    //{
    //    ChangeStateTime += Time.deltaTime;// + UnityEngine.Random.Range(-0.02f, 0.02f);
    //        if (ChangeStateTime>5)
    //    {
    //        ChangeStateTime = 0;
    //        TargetDetection();
    //    }


    //}

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (!IsControllable() && PhotonNetwork.IsConnected)
            return;

        if (collision.gameObject.tag == "Star")
        {
            TargetDetection();
            return;
        }
    }

    public IEnumerator flareDropIE()
    {
        throw new System.NotImplementedException();
        if (PlayerDataClass.Flare > 0)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject.Instantiate(flareObj, transform.TransformPoint(0, 0, -0.5f), transform.rotation);
                for (int j = 0; j < 10; j++)
                    yield return null;
            }
            PlayerDataClass.Flare--;
        }
    }


}