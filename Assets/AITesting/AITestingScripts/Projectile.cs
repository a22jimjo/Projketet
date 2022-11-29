using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Projectile : MonoBehaviour
{
    Rigidbody rb;
    public float destroyDelay;
    public int damage;
    public int health;
    public bool canHitEnemy;
    public bool canHitPlayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<EnemyStats>(out EnemyStats enemyComponent ) && canHitEnemy)
        {
            Freeze();
            enemyComponent.TakeDamage(damage);
            Destroy(gameObject, destroyDelay);
        }

        if (other.gameObject.TryGetComponent<PlayerStats>(out PlayerStats playerComponent) && canHitPlayer)
        {
            Freeze();
            playerComponent.TakeDamage(damage);
            Destroy(gameObject, destroyDelay);
        }

    }

    public void Freeze()
    {
        rb.constraints = RigidbodyConstraints.FreezePosition;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
}
