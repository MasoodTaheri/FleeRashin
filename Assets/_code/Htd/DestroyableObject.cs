using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestroyableObject : MonoBehaviour
{
    public int maxHealth;
    public Transform healthBar;
    public bool dontDestroyOnDeath;
    public bool showExplosiontEffect;
    public bool leaveSmokeTrail;
    public GameObject[] smokePsPrefabs = new GameObject[0];
    public UnityEvent OnDeathCallBack;
    public Action<float> OnDamageCallBack;
    int currentHealth;
    public int Health { get{ return currentHealth; }}
    
    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void ApplyDamage(int damage)
    {
        if (currentHealth <= 0)
            return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnDeath();
        }

        if (healthBar != null)
            healthBar.localScale = new Vector3((float)currentHealth / maxHealth, 1, 1);

        if (OnDamageCallBack != null)
            OnDamageCallBack.Invoke((float)currentHealth / maxHealth);
    }

    void OnDeath()
    {
        if (currentHealth > 0)
            return;

        if (!dontDestroyOnDeath)
        {
            GameObject.Destroy(gameObject);

            if(showExplosiontEffect)
                ExplosionEffectController.instance.ShowExplosionEffect(transform.position, transform.parent, 10f);
        }

        if (leaveSmokeTrail)
        {
            foreach (GameObject item in smokePsPrefabs)
            {
                Instantiate(item, transform.position, Quaternion.identity, transform.parent);
            }
        }

        OnDeathCallBack.Invoke();
    }
}
