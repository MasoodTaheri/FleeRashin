using UnityEngine;
using System.Collections;

public class flareController : MonoBehaviour
{
    public Rigidbody rb;
    public float forwardSpeed;
    public float lifetime;
    // Use this for initialization
    void Start()
    {
        //transform.rotation = Quaternion.Euler(0,
        //    Mathf.Sign(Random.Range(-2.0f, 2.0f)) * 
        //    /*Random.Range(20, 50)*/120, 0);
        transform.Rotate(new Vector3(0, Mathf.Sign(Random.Range(-2.0f, 2.0f)) * 120, 0));

        Destroy(this.gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.forward * forwardSpeed;
        rb.angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Rocket")
        {
            other.GetComponent<AntiaircraftChaserRocket>().SetFlare(gameObject);
        }

        if (other.tag == "PlaneRocket")
        {
            if(other.GetComponent<BossPlaneRocket>() != null)
            other.GetComponent<BossPlaneRocket>().flare = gameObject;
        }
    }
}
