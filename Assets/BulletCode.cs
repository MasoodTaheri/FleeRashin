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
    protected float Speed;
    [SerializeField]
    private float LifeLength;
    public Player owner;
    public int damage;
    public GameObject HitParticle;
    
    protected virtual void Start()
    {
        Destroy(this.gameObject, LifeLength);
    }
    
    protected virtual void Update()
    {
        Move();
    }

    protected virtual void Move()
    {
        transform.position += transform.forward * Speed * Time.deltaTime;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        bool explode = false;
        DestroyableObject detObj = other.gameObject.GetComponent<DestroyableObject>();

        if (detObj != null)
        {
            other.gameObject.GetComponent<DestroyableObject>().ApplyDamage(damage);
            explode = true;
        }
        else if (other.gameObject.layer == 12)
        {
            other.gameObject.GetComponent<BossPlaneBigTurretShield>().Reflect();
            explode = true;
        }

        if (explode)
        {
            Explude();
            Destroy(this.gameObject);
        }

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

    public void Initalize(float _speed, float _LifeLength
        , Vector3 pos, Quaternion rot, int _damage)
    {
        Speed = _speed;
        LifeLength = _LifeLength;
        transform.position = pos;
        transform.rotation = rot;
        damage = _damage;
    }

    public void Initalize(float _speed, float _LifeLength
        , Vector3 pos, Vector3 angle, int _damage)
    {
        Speed = _speed;
        LifeLength = _LifeLength;
        transform.position = pos;
        transform.eulerAngles = angle;
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
