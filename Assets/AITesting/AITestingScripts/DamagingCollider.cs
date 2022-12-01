using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamagingCollider : MonoBehaviour
{
    Rigidbody rb;
    public float destroyDelay;
    public int damage;
    public bool canHitEnemy;
    public bool canHitPlayer;
    public bool destoryOnContact;
    public bool freezeOnContact;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<EnemyStats>(out EnemyStats enemyComponent ) && canHitEnemy)
        {
            
            enemyComponent.TakeDamage(damage);
            if (freezeOnContact) Freeze();
            if (destoryOnContact) Destroy(gameObject, destroyDelay);
            Debug.Log(gameObject.name + " Has taken " + damage + " damage");
        }

        if (other.gameObject.TryGetComponent<PlayerStats>(out PlayerStats playerComponent) && canHitPlayer)
        {
            playerComponent.TakeDamage(damage);
            if (freezeOnContact) Freeze();
            if (destoryOnContact) Destroy(gameObject, destroyDelay);
            Debug.Log(gameObject.name + " Has taken " + damage + " damage");
        }

    }

    public void Freeze()
    {
        rb.constraints = RigidbodyConstraints.FreezePosition;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
}
