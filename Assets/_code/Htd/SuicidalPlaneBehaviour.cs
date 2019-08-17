using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIPlaneMovement))]
public class SuicidalPlaneBehaviour : MonoBehaviour
{
    [SerializeField]
    int damage;
    [SerializeField]
    float spinDistance;
    [SerializeField]
    bool spinClockwise = true;
    GameObject target;

    AIPlaneMovement pm;
    bool spined;
    bool spinning;

    private void Awake()
    {
        pm = GetComponent<AIPlaneMovement>();
    }

    private void Update()
    {
        if (target == null)
        {
            if (LevelManager.Instance == null || LevelManager.Instance.PlayerPlane == null)
                return;
            target = LevelManager.Instance.PlayerPlane.gameObject;
            return;
        }

        if (!spined)
        {
            if (spinning)
            {
                pm.Steer((spinClockwise) ? 1f : -1f);
                return;
            }
            else
            {
                if (Vector3.Distance(target.transform.position, transform.position) < spinDistance)
                {
                    StartCoroutine(StopSpinning());
                    pm.stopRotate = true;
                    spinning = true;
                }
            }
        }

        pm.destinationPos = target.transform.position;
    }

    IEnumerator StopSpinning()
    {
        yield return new WaitForSeconds(190f / pm.RotateSpeed);
        spinning = false;
        spined = true;
        pm.stopRotate = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == LevelManager.Instance.PlayerPlane.tag)
        {
            LevelManager.Instance.PlayerPlane.GetComponent<DestroyableObject>().ApplyDamage(damage);
            ExplosionEffectController.instance.ShowExplosionEffect(transform.position);
            GameObject.Destroy(gameObject);
        }
    }
}
