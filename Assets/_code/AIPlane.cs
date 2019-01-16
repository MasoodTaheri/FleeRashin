using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIPlane : MonoBehaviour
{
    protected float turnLeft;
    protected float turnRight;
    [SerializeField]
    protected float forwardSpeed;

    [SerializeField]
    protected float rotateSpeed;
    public Rigidbody rb;

    public Vector3 target;
    public abstract void TargetDetection();


    public virtual void calculateDirection()
    {
        Vector3 relativePos = target - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);

        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotateSpeed);

    }
    public abstract void onCollide(Collision collision);
    public virtual void moveForward()
    {
        rb.velocity = transform.forward * forwardSpeed;
        rb.angularVelocity = Vector3.zero;
    }

    public float b2f(bool b)
    {
        return (b) ? 1 : 0;
    }

    public virtual void Update()
    {
        TargetDetection();
        calculateDirection();
        moveForward();
    }

    public virtual void Start()
    {
        TargetDetection();
    }

}
