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
    //public int flareCountforBreakTarget = -1;
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
        //canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        //RocketUIPrefab = Resources.Load("RocketUIPos") as GameObject;
        //RocketUI = GameObject.Instantiate(RocketUIPrefab) as GameObject;
        //RocketUI.transform.SetParent(canvas.transform, false);
        //uiIndic = RocketUI.GetComponent<Image>();
        uiIndic = FindObjectOfType<UIManager>().InstantiateIndicator(RocketUIPrefab);
        
        ps = transform.GetChild(1).GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody>();
        deltaPosTarget = new Vector3(UnityEngine.Random.Range(-0.33f, 0.33f), 0, UnityEngine.Random.Range(-0.22f, 0.22f));
        rotateSpeed += UnityEngine.Random.Range(-0.5f, 0.5f);
        life += UnityEngine.Random.Range(0, 5f);
        if (!PhotonNetwork.IsMasterClient)
            return;
       

        findplayer();
       

        transform.position = spawnpos();

    }



    public void Collision(Collision collision, GameObject me)
    {
        Debug.Log("Collision Rocket is hit by " + collision.gameObject.name);
        if (!PhotonNetwork.IsMasterClient)
            return;

        Explude();
        target = null;
    }


    public void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Rocket is hit by " + collision.gameObject.name);

        if (!PhotonNetwork.IsMasterClient)
            return;
        RocketManager.instance.RocketIsExpluded();
        Explude();
        target = null;
    }


    Vector3 spawnpos()
    {
        if (target == null)
            findplayer();

        return target.transform.position + new Vector3(Randomsgn(5, 10), 0, Randomsgn(5, 10));

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
            findplayer();
            uiIndic.enabled = false;
            return;
        }




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
                Explude();
            }
        }
    }





    void findplayer()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Playerbody");

        if (targets.Length == 0)
        {
            Debug.LogError("targets count=" + targets.Length);

            Explude();
            return;
        }
        target = targets[UnityEngine.Random.Range(0, targets.Length)];
        

    }
    

    protected void rotate()
    {
        throw new NotImplementedException();
    }


    void OnDestroy()
    {
        GameObject.Destroy(GameObject.Instantiate(HitParticle, transform.position, Quaternion.identity) as GameObject, 5);
        if (uiIndic != null)
            if (uiIndic.gameObject != null)
                GameObject.Destroy(uiIndic.gameObject, 0.25f);

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
        if (!PhotonNetwork.IsMasterClient) return;

        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.Destroy(this.gameObject);
    }
}
