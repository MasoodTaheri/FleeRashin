using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaserRocket : RocketSniffer
{
    [SerializeField]
    float offsetRange;
    [SerializeField]
    float offsetChangeSpeed;
    Vector3 offset = Vector3.zero;
    float startoffset;

    private void Start()
    {
        startoffset = Random.Range(0, Mathf.PI);
    }

    protected override void Rotate()
    {
        if (target != null)
        {
            offset.x = Mathf.Cos(Time.time * offsetChangeSpeed + startoffset) * offsetRange;
            offset.z = Mathf.Sin(Time.time * offsetChangeSpeed + startoffset) * offsetRange;

            Quaternion desireRotation = Quaternion.LookRotation(new Vector3(target.transform.position.x + offset.x, transform.position.y, target.transform.position.z + offset.z) - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desireRotation, Time.deltaTime * rotateSpeed);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.gameObject.layer == 14)
        {
            ExplosionEffectController.instance.ShowExplosionEffect(transform.position);
            GameObject.Destroy(gameObject);
            GameObject.Destroy(other.gameObject);
        }
    }
}
