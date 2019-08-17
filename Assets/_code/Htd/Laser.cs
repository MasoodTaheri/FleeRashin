using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    int damage;
    int count = 2;
    
    public void End()
    {
        //if(animator != null)
            animator.SetTrigger("End");
    }

    public void Destroy()
    {
        count--;
        if(count == 0)
            GameObject.Destroy(gameObject);
    }

    public void SetDamage(int _damage) { damage = _damage; }
    public int GetDamage() { return damage; }
}
