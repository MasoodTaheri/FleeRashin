using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public int maxHealth;
    public Transform healthBar;
    int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }
    public void ApplyDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            GameObject.Destroy(gameObject);

        healthBar.localScale = new Vector3((float)currentHealth / maxHealth, 1, 1);
    }
}
