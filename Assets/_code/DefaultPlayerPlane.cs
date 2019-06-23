using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

[System.Serializable]
public class Wing2
{
    [SerializeField]
    public GameObject PS_winghitPrefab;
    [SerializeField]
    public ParticleSystem ps;

    const float MinValueInHit = -0.8f;
    public float Health = 1;
    //public ColliderCallback cc;
    public GameObject root;
}

[System.Serializable]
public class DefaultPlayerPlane : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    public PhotonView pv;
    public float Leftweight;
    public float Rightweight;
    public Vector2 rotateFactor;//winghit

    public float RotateLeftFactor;//touch left
    public float RotateRightFactor;//touch right
    protected GameObject flareObj;
    //[SerializeField]
    protected Wing2 leftWing;
    //[SerializeField]
    protected Wing2 rightWing;
    public bool readytodestroy = false;
    protected gun planeGun;
    private AudioSource sfx;


    private Rigidbody rb;
    public float forwardSpeed;
    public float rotateSpeed;
    public float lifetime;
    //public Sprite Sprite;
    //public GameObject obj;
    public GameObject IndicatorPrefab;
    protected Image Indicator;

    public ColliderDataClass[] colliders;
    public enum PlanePartEnum { body, LeftWing, rightWing }
    public int Health;
    public SpriteRenderer Healthbar;
    public GameObject particlePrefab;
    public GameObject RocketPrefab;
    public GameObject planeExplusion;
    [SerializeField]
    protected Color mycolor;
    //public bool playerControlable;
    //public bool ControlableOnThisDevice;



    void Start()
    {
        //obj = _obj;
        //obj = GameObject.Instantiate(prefab, new Vector3(0, -6, 0), Quaternion.identity) as GameObject;
        rb = GetComponent<Rigidbody>();
        rotateFactor = Vector2.one;
        RotateLeftFactor = 0;
        RotateRightFactor = 0;
        flareObj = Resources.Load("flare") as GameObject;
        //leftWing = new Wing(0, 0, 0, obj.transform.GetChild(2).gameObject, this);
        //rightWing = new Wing(0, 0, 0, obj.transform.GetChild(1).gameObject, this);
        //obj.AddComponent<ColliderCallback>()
        readytodestroy = false;


        //gameObject.AddComponent<ColliderCallback>().enter += Collision;
        sfx = transform.GetChild(0).GetComponent<AudioSource>();


        leftWing = new Wing2();
        leftWing.root = transform.GetChild(2).gameObject;
        leftWing.PS_winghitPrefab = Resources.Load("Particle_winghit") as GameObject;

        rightWing = new Wing2();
        rightWing.root = transform.GetChild(1).gameObject;
        rightWing.PS_winghitPrefab = Resources.Load("Particle_winghit") as GameObject;

        //selectColor();

        GameObject tmp = Instantiate(IndicatorPrefab) as GameObject;
        Indicator = tmp.GetComponent<Image>();

        tmp.transform.SetParent(GameObject.FindObjectOfType<Canvas>().gameObject.transform, false);
        IsControllable();

        if (IsControllable())
        {
            this.gameObject.name += "Mine";// + ((this is EnemyPlane) ? "_enemy" : "");
        }

    }

    public bool IsInturn()
    {
        //Cuangvel = rb.angularVelocity.magnitude;
        //return (Cuangvel > angvelMax);

        return (Rightweight != 0) || (Leftweight != 0);


    }

    public virtual bool IsControllable()
    {
        //Debug.Log("pv.IsMine=" + pv.IsMine, this.gameObject);
        //Debug.Log("IsControllable by DefaultPlayerPlane" + pv.IsMine);
        //ControlableOnThisDevice = false;
        //playerControlable = pv.IsMine;

        return pv.IsMine;
    }


    //private void selectColor()
    //{
    //    if (IsControllable())
    //    {
    //        mycolor = playermanager.Instance.planeColorClass.GetRandomColor();
    //        //Debug.Log("my color is set to " + playermanager.Instance.planeColorClass.GetColorname(mycolor));
    //        //mycolor.ToString());
    //        SetMyColor(mycolor);
    //        SendMyColor();
    //        this.gameObject.name += "Mine" + ((this is EnemyPlane) ? "_enemy" : "");
    //    }
    //    else
    //    {
    //        object playerproperty_Color;
    //        if (pv.Owner == null)
    //        {
    //            Debug.Log("pv.Owner is null", this.gameObject);
    //            if (this is EnemyPlane)
    //            {
    //                Debug.Log("this is enemy", this.gameObject);
    //                if (PhotonNetwork.CurrentRoom.CustomProperties.
    //                    TryGetValue("AI" + (this as EnemyPlane).AIid.ToString() + "Color",
    //                    out playerproperty_Color))
    //                {
    //                    Debug.Log("get roomprop  read " + "AI" + (this as EnemyPlane).AIid.ToString() + "Color"
    //                        + "=" + (string)playerproperty_Color, this.gameObject);
    //                    mycolor = playermanager.Instance.planeColorClass.GetColorByNmae((string)playerproperty_Color);
    //                    SetMyColor(mycolor);
    //                }

    //            }

    //        }
    //        else
    //        if (pv.Owner.CustomProperties.TryGetValue("Color", out playerproperty_Color))
    //        {
    //            Debug.Log("other player color=" + (string)playerproperty_Color, this.gameObject);
    //            mycolor = playermanager.Instance.planeColorClass.GetColorByNmae((string)playerproperty_Color);
    //            SetMyColor(mycolor);
    //        }
    //    }

    //}

    public void moveforward()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * forwardSpeed;
        rb.angularVelocity = Vector3.zero;
    }


    public float b2f(bool b)
    {
        return (b) ? 1 : 0;
    }
    public void StartShoot()
    {
        //Debug.Log("StartShoot " + "IsControllable=" + IsControllable());
        if (!IsControllable())
            return;

        if (planeGun == null)
            planeGun = GetComponent<gun>();
        planeGun.Shoot(true);
    }
    public void EndShoot()
    {
        if (!IsControllable())
            return;
        if (planeGun == null)
            planeGun = GetComponent<gun>();
        planeGun.Shoot(false);
    }

    public void ShootRocket()
    {
        if (!IsControllable())
            return;
        pv.RPC("ShootRocket_Rpc", RpcTarget.All);
    }

    [PunRPC]
    private void ShootRocket_Rpc()
    {
        GameObject Bullet1 = Instantiate(RocketPrefab) as GameObject;
        Bullet1.transform.position = transform.position + transform.forward * 2;
        //- new Vector3(0, -2, 0);
        Bullet1.transform.rotation = transform.rotation;

    }

    //[PunRPC]
    //public void startWingParticle(string WingName)
    //{
    //    Wing2 temp = null;
    //    Debug.Log("startWingParticle " + WingName);
    //    if (WingName == "rightWing")
    //        temp = rightWing;

    //    if (WingName == "LeftWing")
    //        temp = leftWing;

    //    if (temp.ps == null)
    //    {
    //        GameObject go = Instantiate(temp.PS_winghitPrefab, temp.root.transform.position, Quaternion.identity) as GameObject;
    //        temp.ps = go.GetComponent<ParticleSystem>();
    //        go.transform.SetParent(temp.root.transform);
    //    }
    //}

    //[PunRPC]
    //public void startAllWingParticle()
    //{
    //    //Wing2 temp = null;
    //    //Debug.Log("startWingParticle " + WingName);
    //    //if (WingName == "rightWing")
    //    //    temp = rightWing;

    //    //if (WingName == "LeftWing")
    //    //    temp = leftWing;

    //    if (rightWing.ps == null)
    //    {
    //        GameObject go = Instantiate(rightWing.PS_winghitPrefab, rightWing.root.transform.position, Quaternion.identity) as GameObject;
    //        rightWing.ps = go.GetComponent<ParticleSystem>();
    //        go.transform.SetParent(rightWing.root.transform);
    //    }

    //    if (leftWing.ps == null)
    //    {
    //        GameObject go = Instantiate(leftWing.PS_winghitPrefab, leftWing.root.transform.position, Quaternion.identity) as GameObject;
    //        leftWing.ps = go.GetComponent<ParticleSystem>();
    //        go.transform.SetParent(leftWing.root.transform);
    //    }
    //}

    public bool ishit()
    {
        if (leftWing.Health < 1) return true;
        if (rightWing.Health < 1) return true;

        return false;
    }
    protected virtual void rotate()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime * (Rightweight - Leftweight), 0);
    }

    public void setsterrtoleft(float PaddelValue)//touch controller
    {
        RotateLeftFactor = PaddelValue;

    }

    public void setsterrtoRight(float PaddelValue)//touch controller
    {
        RotateRightFactor = PaddelValue;

    }

    private void InputManager()
    {
        if (!(this is DefaultPlayerPlane))
            return;

        //rotateFactor += new Vector2(1 - leftWing.Health, 1 - rightWing.Health);
        if (rotateFactor.x > 0.5 && leftWing.Health < 0) rotateFactor.x += leftWing.Health;
        if (rotateFactor.y > 0.5 && rightWing.Health < 0) rotateFactor.y += rightWing.Health;
        //    rotateFactor += new Vector2(
        //(isLeft && (rotateFactor.x > 0.5f)) ? minvalue : 0,
        //  (isRight && (rotateFactor.y > 0.5f)) ? minvalue : 0);

        Rightweight = (b2f(Input.GetKey(KeyCode.RightArrow)) + RotateRightFactor) * rotateFactor.y;
        Leftweight = (b2f(Input.GetKey(KeyCode.LeftArrow)) + RotateLeftFactor) * rotateFactor.x;
    }

    public void updateHealthbar()
    {
        Healthbar.transform.localScale = new Vector3(Health / 100.0f, 1, 1);
    }

    public virtual void Update()
    {

        UIPOSClass.UIposArrow(transform.position, Indicator);
        updateHealthbar();

        if (!IsControllable() && PhotonNetwork.IsConnected)
            return;


        //Debug.Log("Update");
        InputManager();
        moveforward();
        rotate();
        //leftWing.Update();
        //rightWing.Update();
    }


    public IEnumerator flareDropIE()
    {
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

    //public void flareDropIE2()
    //{
    //    GameObject.Instantiate(flareObj, obj.transform.TransformPoint(0, 0, -0.5f), obj.transform.rotation);
    //}

    //public void Collision(Collision collision, GameObject me)


    public virtual void OnCollisionEnter(Collision collision)
    {
        if (!IsControllable() && PhotonNetwork.IsConnected)
            return;

        Debug.Log("Collision with " + collision.gameObject.name + "  tag=" + collision.gameObject.tag);
        if (collision.gameObject.tag == "Star")
        {
            Debug.Log("Star");
            GameObject.Destroy(collision.gameObject);
            uiController.Instanse.IncStarPickedup();
            sfx.Play();
            return;
        }

        if (collision.gameObject.tag == "Repair")
        {
            //Debug.Log("Repair");
            GameObject.Destroy(collision.gameObject);
            //if (!leftWing.inchealth())
            //{
            //    rightWing.inchealth();
            //    rotateFactor.y = 1;
            //}
            //else
            //{
            //    rotateFactor.x = 1;
            //}
            inchealth();
            return;
        }


        //for (int i = 0; i < colliders.Length; i++)
        //    if (collision.contacts[0].thisCollider == colliders[i].collider)
        //        if (colliders[i].partname == PlanePartEnum.body)
        //        {
        //            //if (collision.gameObject.tag == "Star")
        //            //{
        //            //    Debug.Log("Star");
        //            //    GameObject.Destroy(collision.gameObject);
        //            //    uiController.Instanse.IncStarPickedup();
        //            //    sfx.Play();
        //            //}

        //            //if (collision.gameObject.tag == "Repair")
        //            //{
        //            //    //Debug.Log("Repair");
        //            //    GameObject.Destroy(collision.gameObject);
        //            //    //if (!leftWing.inchealth())
        //            //    //{
        //            //    //    rightWing.inchealth();
        //            //    //    rotateFactor.y = 1;
        //            //    //}
        //            //    //else
        //            //    //{
        //            //    //    rotateFactor.x = 1;
        //            //    //}
        //            //    inchealth();
        //            //}

        //            if (collision.gameObject.tag == "bullet")
        //            {
        //                //destroy();
        //                //RocketManager.instance.expludeAt(1, transform.position);
        //                BulletCode bc = collision.gameObject.GetComponent<BulletCode>();
        //                if (bc.owner == pv.Owner)
        //                {
        //                    Debug.Log("I hit myself");
        //                    return;
        //                }

        //                Debug.Log("calling RPC planeBodyHit " + gameObject.name);
        //                pv.RPC("planeBodyHit", RpcTarget.All, bc.damage);
        //            }
        //        }
        //        else if ((colliders[i].partname == PlanePartEnum.LeftWing) || (colliders[i].partname == PlanePartEnum.rightWing))
        //        {
        //            if (collision.gameObject.tag == "bullet")
        //            {
        //                BulletCode bc = collision.gameObject.GetComponent<BulletCode>();
        //                if (bc.owner == pv.Owner)
        //                {
        //                    Debug.Log("I hit myself");
        //                    return;
        //                }

        //                //Debug.Log("calling RPC planeBodyHit " + gameObject.name);
        //                //pv.RPC("planeBodyHit", RpcTarget.All, bc.damage);
        //                Debug.Log("WingCollision with" + collision.gameObject.tag + "   " + collision.gameObject.name);

        //                Health -= bc.damage;
        //                if ((Health < 50) && (leftWing.ps == null))
        //                    pv.RPC("startAllWingParticle", RpcTarget.All);

        //                if (Health < 0)
        //                    pv.RPC("planeBodyHit", RpcTarget.All, bc.damage);

        //            }

        //            //Wing2 temp = null;

        //            //if (colliders[i].partname == PlanePartEnum.rightWing)
        //            //    temp = rightWing;

        //            //if (colliders[i].partname == PlanePartEnum.LeftWing)
        //            //    temp = leftWing;

        //            ////Debug.Log("winghit");
        //            //if (temp.ps == null)
        //            //{
        //            //    Debug.Log("calling RPC startWingParticle");
        //            //    pv.RPC("startWingParticle", RpcTarget.All, colliders[i].partname.ToString());
        //            //    //GameObject go = GameObject.Instantiate(temp.PS_winghitPrefab, temp.root.transform.position, Quaternion.identity) as GameObject;
        //            //    //temp.ps = go.GetComponent<ParticleSystem>();
        //            //    //Health = MinValueInHit;

        //            //    //body.winghit(isLeft, isRight, minvalue);
        //            //}
        //            //else
        //            //{

        //            //    Debug.Log("winghit Destroy body");
        //            //    //RocketManager.instance.expludeAt(1, transform.position);
        //            //    //destroy();
        //            //    Debug.Log("calling RPC planeBodyHit");
        //            //    pv.RPC("planeBodyHit", RpcTarget.All);
        //            //}
        //        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        //Debug.Log("plane hit by " + collision.gameObject.tag);


        if (collision.gameObject.tag == "bullet")
        {
            //Debug.Log("Trigger hit by bullet");
            BulletCode bc = collision.gameObject.GetComponent<BulletCode>();
            //GameObject BulletEffect = Instantiate(particlePrefab, collision.transform.position, Quaternion.identity) as GameObject;
            GameObject BulletEffect = Instantiate(bc.HitParticle, collision.transform.position, Quaternion.identity) as GameObject;
            BulletEffect.transform.SetParent(this.transform);
            Destroy(BulletEffect, 3);

            //Debug.Log(collision.gameObject.name);

            if (!IsControllable() && PhotonNetwork.IsConnected)
                return;

            //BulletCode bc = collision.gameObject.GetComponent<BulletCode>();
            //if (bc.owner == pv.Owner)
            //{
            //    Debug.Log("I hit myself");
            //    return;
            //}

            //for (int i = 0; i < colliders.Length; i++)
            //    if (collision.contacts[0].thisCollider == colliders[i].collider)
            //        if (colliders[i].partname == PlanePartEnum.body)
            //        {
            //        }


            pv.RPC("RPC_Plane_Bullet_hit", RpcTarget.All, bc.damage);
            //Debug.Log("WingCollision with" + collision.gameObject.tag + "   " + collision.gameObject.name);
        }

        if (collision.gameObject.tag == "Rocket")
        {
            //Debug.Log("Trigger hit by Rocket");

            IShoot shootable = collision.gameObject.GetComponent<IShoot>();
            if (shootable == null)
            {
                //Debug.LogError("shootable is null");
                return;
            }
            shootable.Explude();


            //BulletCode bc = collision.gameObject.GetComponent<BulletCode>();
            //if (bc != null)
            //{
            //    GameObject BulletEffect = Instantiate(bc.HitParticle, collision.transform.position, Quaternion.identity) as GameObject;
            //    Destroy(BulletEffect, 3);
            //}
            //else
            //{
            //    Rocket rc = collision.gameObject.GetComponent<Rocket>();
            //    if (rc != null)
            //    {
            //        GameObject BulletEffect = Instantiate(rc.HitParticle, collision.transform.position, Quaternion.identity) as GameObject;
            //        Destroy(BulletEffect, 3);
            //    }
            //}



            //Debug.Log(collision.gameObject.name);

            if (!IsControllable() && PhotonNetwork.IsConnected)
                return;


            //if (bc.owner == pv.Owner)
            //{
            //    Debug.Log("I hit myself");
            //    return;
            //}

            pv.RPC("RPC_Plane_Bullet_hit", RpcTarget.All, shootable.GetDamage());
            //Debug.Log("WingCollision with" + collision.gameObject.tag + "   " + collision.gameObject.name);
        }
    }

    [PunRPC]
    public void RPC_Plane_Bullet_hit(int damage)
    {
        Health -= damage;
        if ((Health < 50) && (leftWing.ps == null))
        {
            if (rightWing.ps == null)
            {
                GameObject go = Instantiate(rightWing.PS_winghitPrefab, rightWing.root.transform.position, Quaternion.identity) as GameObject;
                rightWing.ps = go.GetComponent<ParticleSystem>();
                go.transform.SetParent(rightWing.root.transform);
            }

            if (leftWing.ps == null)
            {
                GameObject go = Instantiate(leftWing.PS_winghitPrefab, leftWing.root.transform.position, Quaternion.identity) as GameObject;
                leftWing.ps = go.GetComponent<ParticleSystem>();
                go.transform.SetParent(leftWing.root.transform);
            }
        }
        if (Health < 0)
        {
            Destroy(Instantiate(planeExplusion, transform.position, Quaternion.identity) as GameObject, 5);
            destroy();
        }
    }


    //called by GeneratePlane in player manager  with RpcTarget.AllBufferedViaServer
    [PunRPC]
    public void SetColor(string color)
    {
        //Debug.Log("SetColor called with color=" + color);
        mycolor = playermanager.Instance.planeColorClass.GetColorByNmae(color);
        SetMyColor(mycolor);
    }

    [PunRPC]
    public void SetDtat(string _forwardSpeed, string _rotateSpeed)
    {
        //Debug.Log("SetColor called with color=" + color);
        //Debug.Log("_forwardSpeed=" + _forwardSpeed + "   _rotateSpeed=" + _rotateSpeed);
        forwardSpeed = float.Parse(_forwardSpeed);
        rotateSpeed = float.Parse(_rotateSpeed);
    }



    public void inchealth()
    {
        if (leftWing.ps != null)
        {
            leftWing.ps.enableEmission = false;
            GameObject.Destroy(leftWing.ps, 2);
            leftWing.ps = null;
            rotateFactor.x = 1;
        }
        else

        if (rightWing.ps != null)
        {
            rightWing.ps.enableEmission = false;
            GameObject.Destroy(rightWing.ps, 2);
            rightWing.ps = null;
            rotateFactor.y = 1;
        }



    }

    //public void winghit(bool isLeft, bool isRight, float minvalue)
    //{

    //    Debug.Log("plane.rotateFactor =" + rotateFactor);
    //}

    public void destroy()
    {
        if (this is EnemyPlane) AIManager.instance.AIExpluded();
        readytodestroy = true;
        //Debug.Log("bodey destroy" + gameObject.name);
        //if (flareObj!=null)
        //GameObject.Destroy(flareObj);
        GameObject.Destroy(Indicator.gameObject);
        //Debug.Log("WingOnDestroy");
        if (leftWing.ps != null)
        {
            leftWing.ps.enableEmission = false;
            //GameObject.Destroy(leftWing.ps.gameObject, 3);
            GameObject.Destroy(leftWing.ps.gameObject);
        }

        if (rightWing.ps != null)
        {
            rightWing.ps.enableEmission = false;
            //GameObject.Destroy(rightWing.ps.gameObject, 3);
            GameObject.Destroy(rightWing.ps.gameObject);
        }

        if (IsControllable())
            PhotonNetwork.Destroy(this.gameObject);
        else
            GameObject.Destroy(this.gameObject);



    }

    void OnDestroy()
    {
        if (Indicator != null)
        {
            GameObject.Destroy(Indicator.gameObject);
            //Debug.Log("Indicator destroyed @ ondestroy");
        }
    }

    //public void WingOnDestroy()
    //{
    //    Debug.Log("WingOnDestroy");
    //    if (leftWing.ps != null)
    //    {
    //        leftWing.ps.enableEmission = false;
    //        //GameObject.Destroy(leftWing.ps.gameObject, 3);
    //        GameObject.Destroy(leftWing.ps.gameObject);
    //    }

    //    if (rightWing.ps != null)
    //    {
    //        rightWing.ps.enableEmission = false;
    //        //GameObject.Destroy(rightWing.ps.gameObject, 3);
    //        GameObject.Destroy(rightWing.ps.gameObject);
    //    }
    //}

    //public void SetPlayerCustomPropertise()
    //{
    //    int KillScore = (int)PhotonNetwork.LocalPlayer.CustomProperties["Color"];
    //    mycolor = Color.blue;
    //    Hashtable hash = new Hashtable();
    //    hash.Add("Color", mycolor);
    //    PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    //}

    public Color GetMyColor()
    {
        return mycolor;
    }
    public void SetMyColor(Color cl)
    {
        //Debug.Log("Set color to " + playermanager.Instance.planeColorClass.GetColorname(cl));
        mycolor = cl;
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = mycolor;
    }

    //public void SendMyColor()
    //{

    //    //Debug.Log("sending color");
    //    Hashtable hash = new Hashtable();
    //    hash.Add("Color", playermanager.Instance.planeColorClass.GetColorname(mycolor));
    //    PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    //    PlayerBindingObjectClass.update();
    //}
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        //Debug.Log("OnPhotonInstantiate");
        PlayerBindingObjectClass.update();
    }
}


[System.Serializable]
public class ColliderDataClass
{
    public Collider collider;
    public DefaultPlayerPlane.PlanePartEnum partname;
    //public string partname;
}