using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VihecleMovement : MonoBehaviour
{
    [SerializeField]
    [Range(0, 100f)] float acceleration;
    [SerializeField]
    [Range(0, 100f)] float maxSpeed;
    [SerializeField]
    [Range(0, 100f)] float customMaxSpeed;
    [SerializeField]
    [Range(0, 180f)] float turnSpeed;
    [SerializeField]
    [Range(0, 100f)] float turnLimitSpeed;
    [SerializeField]
    [Range(0, 100f)] float breakAcceleration;
    [SerializeField]
    Waypoint currentWaypoint;
    [SerializeField]
    [Range(0, 10f)] float reachLimit;
    [SerializeField]
    [Range(0, 5f)] float destinationRange;
    [SerializeField]
    Transform[] rayOrigins = new Transform[0];
    public bool stop;
    float currentSpeed;
    Rigidbody rb;
    GameObject wayBlockingObject;
    public bool customMode;
    public Vector3 destination;
    float nextChangeDestinationTime;
    float lastChangeDestinationTime = 0;

    const float fixChangeDestinationInterval = 5f;
    const float ChangeDestinationRandomRangeInterval = 3f;

    void Start()
    {
        //FindObjectOfType<CameraMovement>().AddTarget(gameObject);
        currentSpeed = 0;

        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }   
    }

    void Update()
    {
        for (int i = 0; i < rayOrigins.Length; i++)
        {
            if (Physics.Raycast(rayOrigins[i].position, transform.forward, 1f))
            {
                Break();
                return;
            }
            
        }

        if (customMode)
            CostumMovement();
        else
        {
            if (stop)
                Break();
            else
                MoveTowardTarget();
        }
    }

    void CostumMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Accelerate();
        }

        if (Input.GetKey(KeyCode.D))
        {
            Rotate(true);
            Accelerate();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Rotate(false);
            Accelerate();
        }

        if (Input.GetKey(KeyCode.S))
        {
            Break();
        }
    }

    void MoveTowardTarget()
    {
        if (currentWaypoint == null || !currentWaypoint.move)
        {
            Break();
            return;
        }

        if (currentWaypoint != null && Time.time - lastChangeDestinationTime > nextChangeDestinationTime)
        {
            NextDestination();
        }

        Vector3 directoin = (destination - transform.position);
        Vector3 vehicleForward = new Vector3(Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.y), 0, Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.y));

        if (Vector3.Angle(vehicleForward, directoin) < 3f) ;
        else if (Vector3.Cross(vehicleForward, directoin).y < 0)
            Rotate(false);
        else
            Rotate(true);

        Accelerate();

        if (Vector3.Distance(transform.position, destination) <= reachLimit)
        {
            currentWaypoint = currentWaypoint.GetNextWaypoint();
            if (currentWaypoint != null)
            {
                NextDestination();
                customMaxSpeed = currentWaypoint.customMaxSpeed;
            }
        }

    }

    void Accelerate()
    {
        if (rb.velocity.magnitude < Mathf.Min(maxSpeed, customMaxSpeed))
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else
            Break();

        rb.velocity = new Vector3(Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.y), 0, Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.y)).normalized * currentSpeed;
    }

    void Break()
    {
        if (currentSpeed > 0)
        {
            currentSpeed -= breakAcceleration * Time.deltaTime;
        }
        else
            currentSpeed = 0;

        rb.velocity = new Vector3(Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.y), 0, Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.y)).normalized * currentSpeed;
    }

    void Rotate(bool isTurningRight)
    {
        float turnSpeedFactor = 1f;
        if (currentSpeed < turnLimitSpeed)
        {
            turnSpeedFactor = currentSpeed / turnLimitSpeed;
        }
        transform.Rotate(0, ((isTurningRight) ? turnSpeed : - turnSpeed) * turnSpeedFactor * Time.deltaTime, 0);
    }

    public Vector3 Get2XStoppingPos()
    {
        Vector3 pos = transform.position;
        float t = rb.velocity.magnitude / breakAcceleration;
        float s = 0.5f * -breakAcceleration * t * t + rb.velocity.magnitude * t;
        pos += rb.velocity.normalized * s*2;

        return pos;
    }

    void NextDestination()
    {
        float range = Random.Range(0, destinationRange);
        float angle = Random.Range(0, 2 * Mathf.PI);
        destination = currentWaypoint.transform.position + new Vector3(Mathf.Cos(angle) * range, 0, Mathf.Sin(angle) * range);
        nextChangeDestinationTime = fixChangeDestinationInterval + Random.Range(0, ChangeDestinationRandomRangeInterval);
        lastChangeDestinationTime = Time.time;
    }

}
