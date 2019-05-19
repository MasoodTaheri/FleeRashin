using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Photon.Pun;
using Photon.Realtime;

public class Rocket : MonoBehaviour, IShoot
{

    public GameObject target;
    Vector3 deltaPosTarget;
    Canvas canvas;
    public GameObject RocketUIPrefab;
    GameObject RocketUI;
    Image uiIndic;
    public GameObject RocketImage;
    bool isDestroyed = false;
    public ParticleSystem ps;
    public bool readytodestroy = false;

    float StrateForwardIncreaseAmount = 0.025f;
    public float incSpeedValue = 0;
    float maxIncValue = 1.0f;
    //advance feature
    public int flareCountforBreakTarget = -1;
    public flareController flareControllerCode;
    private float life;
    const float neardeathTime = 3;


    public Rigidbody rb;
    public float forwardSpeed;
    public float rotateSpeed;
    public float lifetime;
    //public Sprite Sprite;
    //public GameObject obj;
    public GameObject HitParticle;
    public int damage;

    //public void Init(GameObject _Root)
    void Start()
    {
        //rotateSpeed = 1;//
        //forwardSpeed = 3.65f;//
        //lifetime = 15;//

        //obj = _obj;
        //obj = GameObject.Instantiate(prefab, spawnpos(), Quaternion.identity) as GameObject;
        //obj.transform.SetParent(_Root.transform);


        //target = GameObject.FindGameObjectWithTag("Playerbody");
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        deltaPosTarget = new Vector3(UnityEngine.Random.Range(-0.33f, 0.33f), 0, UnityEngine.Random.Range(-0.22f, 0.22f));
        //target = GameObject.Find("Playerplane");
        //GameObject.Destroy(obj, lifetime + UnityEngine.Random.Range(0, 5));
        rb = GetComponent<Rigidbody>();

        RocketUIPrefab = Resources.Load("RocketUIPos") as GameObject;
        RocketUI = GameObject.Instantiate(RocketUIPrefab) as GameObject;
        RocketUI.transform.SetParent(canvas.transform, false);
        uiIndic = RocketUI.GetComponent<Image>();


        //forwardSpeed += Random.Range(0, 1.5f);
        rotateSpeed += UnityEngine.Random.Range(-0.5f, 0.5f);

        //ColliderCallback cc = this.gameObject.AddComponent<ColliderCallback>();
        //cc.enter += Collision;
        //cc.destroy += OnDestroy;

        findplayer();
        ps = transform.GetChild(1).GetComponent<ParticleSystem>();
        //cc.InvokeRepeating("findplayer", 0, 1);
        //explusionList = new List<ExplusionClass>();
        life += UnityEngine.Random.Range(0, 5f);
        transform.position = spawnpos();

    }

    public void Collision(Collision collision, GameObject me)
    {
        destroiedbyCollision = true;
        //GameObject.Destroy(obj, 0.25f);
        GameObject.Destroy(GameObject.Instantiate(HitParticle, transform.position, Quaternion.identity) as GameObject, 5);
        GameObject.Destroy(this.gameObject);
        //RocketImage.SetActive(false);
        target = null;
        /*  foreach (ExplusionClass e in RocketManager.instance.explusionList)
          {
              if (e.tag == collision.gameObject.tag)
                  GameObject.Destroy(GameObject.Instantiate(e.prefab, transform.position, Quaternion.identity) as GameObject, 5);
          }

          if (collision.gameObject.tag == "Playerbody")
          {
              playermanager.PlanePlayer.destroy();
          }
          //Destroy(collision.gameObject);*/

        //if (collision.gameObject.tag == "Rocket")
        //    uiController.Instanse.IncRockethit();

        //if (collision.gameObject.tag == "bullet")
        //    uiController.Instanse.IncRockethit();
    }


    public void OnTriggerEnter(Collider collision)
    {
        destroiedbyCollision = true;
        //GameObject.Destroy(GameObject.Instantiate(HitParticle, transform.position, Quaternion.identity) as GameObject, 5);
        //GameObject.Destroy(this.gameObject);
        target = null;
    }


    Vector3 spawnpos()
    {
        return playermanager.PlanePlayer.transform.position + new Vector3(Randomsgn(5, 10), 0, Randomsgn(5, 10));
    }

    float Randomsgn(float min, float max)
    {
        float sign = Mathf.Sign(UnityEngine.Random.Range(-100.0f, 100.0f));
        float number = UnityEngine.Random.Range(min, max);

        return sign * number;
    }

    public void Update()
    {

        UIPOSClass.UIposArrow(transform.position, uiIndic);

        if (!PhotonNetwork.IsMasterClient) return;
        if (isDestroyed) return;
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Playerbody");
            uiIndic.enabled = false;
            return;
        }

        findplayer();


        Vector3 relativePos = target.transform.position + deltaPosTarget - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);

        if (Mathf.Abs(Vector3.Angle(transform.rotation.eulerAngles, rotation.eulerAngles)) < 0.1f)
            incSpeedValue += (incSpeedValue < maxIncValue) ? StrateForwardIncreaseAmount : 0;
        else
            incSpeedValue = 0;



        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotateSpeed);

        rb.velocity = transform.forward * (forwardSpeed + incSpeedValue);


        life += Time.deltaTime;
        if (life + neardeathTime > lifetime)
        {
            if (ps != null)
                ps.enableEmission = false;
            if (life > lifetime)
            {
                GameObject.Destroy(GameObject.Instantiate(HitParticle, transform.position, Quaternion.identity) as GameObject, 5);
                //GameObject.Destroy(this.gameObject);
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }





    void findplayer()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Playerbody");
        if (targets.Length < flareCountforBreakTarget) return;
        if (targets.Length == 0) return;
        if (targets.Length == 1) target = targets[0];
        else
        {
            if (flareControllerCode != null) return;// if this is not null no need to change target(flare)

            int id = -1;
            float dist = Mathf.Infinity;
            float maxdistance = 2.5f;
            for (int i = 1; i < targets.Length; i++)//start from 1 because its better fo find flare insted of planes
            {
                float localdist = Vector3.Distance(transform.position, targets[i].transform.position);

                if (localdist < dist)
                {
                    dist = localdist;
                    id = i;
                }
            }
            if (id != -1)
            {
                float localdist = Vector3.Distance(transform.position, targets[id].transform.position);
                if (localdist < maxdistance)
                {
                    //Debug.Log("localdist=" + localdist);
                    target = null;
                    target = targets[id];
                    flareControllerCode = target.GetComponent<flareController>();
                }
                //else
                //Debug.Log("localdist=" + localdist+"    not selected");


            }

        }

    }

    bool destroiedbyCollision = false;

    protected void rotate()
    {
        throw new NotImplementedException();
    }


    void OnDestroy()
    {
        if (uiIndic != null)
            if (uiIndic.gameObject != null)
                GameObject.Destroy(uiIndic.gameObject, 0.25f);
        //RocketImage.SetActive(false);
        isDestroyed = true;
        GameObject.Destroy(rb);
        if (ps != null)
            ps.enableEmission = false;
        readytodestroy = true;
    }

    public Player GetOwner()
    {
        return null;
    }

    public int GetDamage()
    {
        return damage;
    }

    public void Explude()
    {
        GameObject BulletEffect = Instantiate(HitParticle, transform.position, Quaternion.identity) as GameObject;
        Destroy(BulletEffect, 3);
        GameObject.Destroy(this.gameObject);
    }
}
