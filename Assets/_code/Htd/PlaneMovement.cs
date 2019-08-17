using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMovement : MonoBehaviour
{
    public enum PlaneSteerDirection { Left,Right}

    [SerializeField]
    [Range(0, 10)] public float forwardSpeed;
    [SerializeField]
    [Range(0, 200)] float rotateSpeed;
    public float RotateSpeed { get { return rotateSpeed; } }
    [SerializeField]
    Color mycolor;
    [SerializeField]
    string leftTriggerName;
    [SerializeField]
    string rightTriggerName;

    Rigidbody rb;
    Collider[] colliders;
    Animator planeAnimation;
    protected float Leftweight;
    protected float Rightweight;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        colliders = GetComponents<Collider>();
        planeAnimation = GetComponent<Animator>();

        SpriteRenderer sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        if(sprite != null)
            sprite.color = mycolor;
    }

    protected virtual void Update()
    {
        MoveForward();
    }

    protected void MoveForward()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * forwardSpeed;
        rb.angularVelocity = Vector3.zero;
    }
    
    public void Steer(float value)
    {
        Leftweight = (value < 0) ? 1 : 0;
        Rightweight = (value > 0) ? 1 : 0;

        if (planeAnimation != null)
        {
            planeAnimation.SetBool(rightTriggerName, Rightweight > 0);
            planeAnimation.SetBool(leftTriggerName, Leftweight > 0);
        }

        transform.Rotate(0, rotateSpeed * Time.deltaTime * (Rightweight - Leftweight), 0);
    }

    public void Steer(PlaneSteerDirection direction)
    {
        float value = (direction == PlaneSteerDirection.Left) ? -1 : 1;
        Steer(value);
    }
}
