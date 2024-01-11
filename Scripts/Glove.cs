using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glove : MonoBehaviour
{

    [SerializeField] float damage = 10;
    
    void OnTriggerEnter(Collider collision)
    {
        IDamageable damageable = collision.transform.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
    }
}