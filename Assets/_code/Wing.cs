using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Wing : MovableObject
{
    [SerializeField]
    GameObject PS_winghitPrefab;
    [SerializeField]
    ParticleSystem ps;

    [SerializeField]
    DefaultPlayerPlane body;
    const float MinValueInHit = -0.8f;
    public float Health;



    public Wing(float _forwardSpeed, float _rotateSpeed, float _lifetime,
         GameObject _obj, DefaultPlayerPlane _body) :
        base(_forwardSpeed, _rotateSpeed, _lifetime, null, _obj)
    {
        obj = _obj;
        body = _body;
        Health = 1.0f;
        //code = _obj.GetComponent<winghit>();
        //code.WingObject = this;
        PS_winghitPrefab = Resources.Load("Particle_winghit") as GameObject;


        ColliderCallback cc = obj.gameObject.AddComponent<ColliderCallback>();
        cc.enter += Collision;
        cc.destroy += OnDestroy;
    }

    public override void Collision(Collision collision, GameObject me)
    {
        Debug.Log("winghit");
        if (ps == null)
        {

            GameObject go = GameObject.Instantiate(PS_winghitPrefab, obj.transform.position, Quaternion.identity) as GameObject;
            ps = go.GetComponent<ParticleSystem>();
            //Health = MinValueInHit;

            //body.winghit(isLeft, isRight, minvalue);
        }
        else
        {
            Debug.Log("winghit Destroy body");
            RocketManager.instance.expludeAt(1, body.obj.transform.position);
            body.destroy();
        }
    }

    public override void Update()
    {

        if (ps == null) return;

        ps.transform.position = this.obj.transform.position;
        ps.transform.rotation = obj.transform.rotation;
        //Debug.Log("pos=" + obj.transform.position, obj);

    }

    protected override void rotate()
    {
        throw new NotImplementedException();
    }

    public bool inchealth()
    {
        bool ret = (ps != null);
        if (ps != null)
        {
            ps.enableEmission = false;
            GameObject.Destroy(ps, 2);
            ps = null;
        }
        return (ret);
    }

    public void OnDestroy()
    {
        //Debug.Log("OnDestroy");
        if (ps != null)
        {
            ps.enableEmission = false;
            GameObject.Destroy(ps.gameObject, 3);
        }
    }
}
