using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class MovableObject
{
    public Rigidbody rb;
    protected float forwardSpeed;
    protected float rotateSpeed;
    protected float lifetime;
    protected Sprite Sprite;
    public GameObject obj;



    public MovableObject(float _forwardSpeed, float _rotateSpeed,
        float _lifetime, Sprite _sprite, GameObject _obj)
    {
        //Debug.Log("MovableObject Constructor");

        forwardSpeed = _forwardSpeed;
        rotateSpeed = _rotateSpeed;
        lifetime = _lifetime;
        //obj = _obj;


    }

    public virtual void moveforward()
    {
        rb.velocity = obj.transform.forward * forwardSpeed;
        rb.angularVelocity = Vector3.zero;
    }
    protected abstract void rotate();
    public abstract void Collision(Collision collision, GameObject me);
    public abstract void Update();

    public float b2f(bool b)
    {
        return (b) ? 1 : 0;
    }
}





