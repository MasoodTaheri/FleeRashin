using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffectController : MonoBehaviour
{
    public static ExplosionEffectController instance;
    [SerializeField]
    GameObject[] explosionEffects = new GameObject[0];

    private void Awake()
    {
        instance = this;
    }

    public void ShowExplosionEffect(Vector3 position, Transform parent = null, float lifeDuration = -1)
    {
        GameObject go = Instantiate(explosionEffects[Random.Range(0, explosionEffects.Length)], position, Quaternion.identity);

        if (lifeDuration > 0)
            GameObject.Destroy(go, lifeDuration);
        else
            GameObject.Destroy(go, 5);

        if (parent != null)
            go.transform.SetParent(parent, true);
    }
}
