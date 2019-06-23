using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testcode : MonoBehaviour
{


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.LogFormat("OnCollisionEnter BulletCode {0} hited by {1}", other.gameObject.name, this.gameObject.name);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.LogFormat("OnTriggerEnter BulletCode {0} hited by {1}", other.gameObject.name, this.gameObject.name);
    }
}
