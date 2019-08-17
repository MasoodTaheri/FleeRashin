using System.Collections.Generic;
using UnityEngine;

public class AirplaneBomb : MonoBehaviour
{
    [SerializeField]
    int bombDamage;
    [SerializeField]
    [Range(0,1)]float explosionRadius;
    [SerializeField]
    Vector2 scaleRange;
    [SerializeField]
    float fallDuration;
    [SerializeField]
    AnimationCurve curve;

    List<GameObject> targetsAround = new List<GameObject>();

    Vector3 localScale;
    float passedTime = 0;
    private void Start()
    {
        targetsAround.Clear();
        localScale = new Vector3(scaleRange.y, scaleRange.y, scaleRange.y);
    }

    private void Update()
    {
        if (passedTime >= fallDuration)
            Explode();

        passedTime += Time.deltaTime;
        passedTime = (passedTime > fallDuration) ? fallDuration : passedTime;
        localScale.x = localScale.y = localScale.z = scaleRange.x + (scaleRange.y - scaleRange.x) * curve.Evaluate(passedTime / fallDuration);
        transform.localScale = localScale; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!targetsAround.Contains(other.gameObject))
        {
            targetsAround.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (targetsAround.Contains(other.gameObject))
        {
            targetsAround.Remove(other.gameObject);
        }
    }

    void Explode()
    {
        DestroyableObject desObj;
        foreach (GameObject item in targetsAround)
        {
            desObj = item.GetComponent<DestroyableObject>();
            if (desObj != null)
            {
                desObj.ApplyDamage(bombDamage);
            }
        }

        ExplosionEffectController.instance.ShowExplosionEffect(transform.position);
        GameObject.Destroy(gameObject);
    }
}
