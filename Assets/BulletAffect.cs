using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAffect : MonoBehaviour
{
    public float speed;
    public float lifelength;
    public string Shooter;
    // Use this for initialization
    void Start()
    {
        GameObject.Destroy(this.gameObject, lifelength);
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent.position += transform.parent.forward * speed * Time.deltaTime;
    }
    void OnCollisionEnter(Collision other)
    {
        Debug.LogFormat("{0} hited by {1}", other.gameObject.name, this.gameObject.name);
        //Destroy(this.gameObject);
        Destroy(this.gameObject.transform.parent.gameObject);
        if (other.gameObject.tag == "Playerbody")
        {
            playermanager.PlanePlayer.destroy();
            RocketManager.instance.expludeAt(1, transform.position);
        }
        if ((Shooter == "Player") && (other.gameObject.tag == "Enemy"))
            uiController.Instanse.IncPlaneHit();
    }

    private void OnDestroy()
    {
        Destroy(this.gameObject.transform.parent.gameObject);
    }
}
