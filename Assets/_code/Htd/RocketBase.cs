using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class RocketBase : MonoBehaviour
{
    [SerializeField]
    protected float forwardSpeed;
    [SerializeField]
    int damage;
    [SerializeField]
    float liftTime;

    protected string[] targetableTags;
    protected int playerInstanceID;
    protected GameObject target;
    protected Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        GameObject.Destroy(gameObject, liftTime);
    }

    protected virtual void Update()
    {
        Move();
    }

    protected virtual void Move()
    {
        rb.velocity = transform.forward.normalized * forwardSpeed;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (IsInTargetableTags(other.tag))
        {
            DestroyableObject detObj = other.GetComponent<DestroyableObject>();

            if (detObj != null)
            {
                detObj.ApplyDamage(damage);

                ExplosionEffectController.instance.ShowExplosionEffect(transform.position);
                GameObject.Destroy(gameObject);
            }

        }
    }

    bool IsInTargetableTags(string tag)
    {
        for (int i = 0; i < targetableTags.Length; i++)
        {
            if (targetableTags[i] == tag)
                return true;
        }

        return false;
    }

    public void SetRocketData(int playerID, string[] tags, GameObject target = null)
    {
        targetableTags = tags;
        playerInstanceID = playerID;
        this.target = target;
    }
}
