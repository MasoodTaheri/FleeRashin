using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public interface IShoot
{
    Player GetOwner();
    int GetDamage();
    void Explude();
}

public class BulletCode : MonoBehaviour,IShoot
{
    [SerializeField]
    private float Speed;
    [SerializeField]
    private float LifeLength;
    //[SerializeField]
    public Player owner;
    public int damage;
    public GameObject HitParticle;


    // Use this for initialization
    void Start()
    {
        Destroy(this.gameObject, LifeLength);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Speed * Time.deltaTime;
    }
    private void OnCollisionEnter(Collision other)
    {
        Debug.LogFormat("OnCollisionEnter BulletCode {0} hited by {1}", other.gameObject.name, this.gameObject.name);
        ////Destroy(this.gameObject);
        ////Destroy(this.gameObject.transform.parent.gameObject);
        //Destroy(this.gameObject);
        ////if (other.gameObject.tag == "Playerbody")
        ////{
        ////    playermanager.PlanePlayer.destroy();
        ////    RocketManager.instance.expludeAt(1, transform.position);
        ////}
        ////if ((Shooter == "Player") && (other.gameObject.tag == "Enemy"))
        ////    uiController.Instanse.IncPlaneHit();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogFormat("OnTriggerEnter BulletCode {0} hited by {1}", other.gameObject.name, this.gameObject.name);

        Destroy(this.gameObject);
    }

    public void Initalize(Player _owner, float _speed, float _LifeLength
        , Vector3 pos, Quaternion rot, float lag, int _damage)
    {
        owner = _owner;
        Speed = _speed;
        LifeLength = _LifeLength;
        transform.position = pos;
        transform.rotation = rot;
        damage = _damage;

    }

    public Player GetOwner()
    {
        return owner;
    }

    public int GetDamage()
    {
        return damage;
    }

    public void Explude()
    {
        GameObject BulletEffect = Instantiate(HitParticle, transform.position, Quaternion.identity) as GameObject;
        Destroy(BulletEffect, 3);
    }
}
