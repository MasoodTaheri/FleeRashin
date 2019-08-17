using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterMovement : MonoBehaviour
{
    [SerializeField]
    [Range(0, 100f)] float acceleration;
    [SerializeField]
    [Range(0, 100f)] float maxSpeed;
    [SerializeField]
    [Range(0, 180f)] float rotateSpeed;
    [SerializeField]
    [Range(0, 100f)] float breakAcceleration;
    [SerializeField]
    [Range(0, 10f)] float reachLimit;
    [SerializeField]
    [Range(0, 2f)] float idleMovementRange;
    [SerializeField]
    [Range(0, 2f)] float idleMovementSpeed;

    public GameObject destinationGo;
    Vector3 idlePos;
    Vector3 finalpos;
    float v1;
    float offset = 0;
    float currentSpeed = 0;
    float idleMovementProgress = 0;
    Rigidbody rb;
    [HideInInspector]
    public bool dontMove;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        idlePos = transform.position;
        finalpos = new Vector3(Mathf.Sin(Time.time * idleMovementSpeed) * idleMovementRange, idlePos.y, Mathf.Sin(Time.time * idleMovementSpeed * 2) * idleMovementRange);
    }

    void Update()
    {
        if (destinationGo != null && !dontMove)
        {
            idleMovementProgress = 0;
            RotateTowardTo(destinationGo);
            MoveTowardDestination();
        }
        else
        {

            if (currentSpeed > 0)
            {
                currentSpeed -= breakAcceleration * Time.deltaTime;
                if (currentSpeed <= 0)
                {
                    currentSpeed = 0;
                    offset = Time.time * idleMovementSpeed;
                    idlePos = transform.position;
                }
                rb.velocity = transform.forward * currentSpeed;
            }
            else
            {
                idleMovementProgress += (idleMovementProgress > idleMovementSpeed) ? 0 : Time.deltaTime;
                v1 = Time.time * idleMovementSpeed - offset;
                finalpos.x = idlePos.x + Mathf.Sin(v1) * idleMovementRange * idleMovementProgress;
                finalpos.z = idlePos.z + Mathf.Sin(v1 * 2) * idleMovementRange * idleMovementProgress;
                transform.position = finalpos;
            }
        }
    }

    void MoveTowardDestination()
    {     
        if (Vector3.Distance(transform.position, destinationGo.transform.position) < reachLimit)
        {
            if (currentSpeed > 0)
                currentSpeed -= breakAcceleration * Time.deltaTime;
            else
                currentSpeed = 0;
        }
        else
        {
            if (currentSpeed < maxSpeed)
                currentSpeed += acceleration * Time.deltaTime;
            else
                currentSpeed = maxSpeed;
        }

        rb.velocity = transform.forward * currentSpeed;
        if (currentSpeed == 0)
        {
            destinationGo = null;

            offset = Time.time * idleMovementSpeed;
            idlePos = transform.position;
        }
    }

    public void RotateTowardTo(GameObject target)
    {
        Vector3 directoin = (target.transform.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(directoin);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * rotateSpeed);
    }
}
