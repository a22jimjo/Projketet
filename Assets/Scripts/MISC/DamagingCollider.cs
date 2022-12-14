using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamagingCollider : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] public float destroyDelay;
    [SerializeField] public int damage;
    [SerializeField] public bool canHitEnemy;
    [SerializeField] public bool canHitPlayer;
    [SerializeField] public bool destoryOnContact;
    [SerializeField] public bool freezeOnContact;
    [SerializeField] public bool ScaleOverDuration;
    [SerializeField] public float timeToScale;
    [SerializeField] private Vector3 sizeToScaleTo;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        //if(ScaleOverDuration) gameObject.tra
    }

    public void CallDamage(Collider other, bool knockback)
    {
        if(other.gameObject.TryGetComponent<EnemyStats>(out EnemyStats enemyComponent ) && canHitEnemy)
        {
            
            enemyComponent.TakeDamage(damage);
            if (freezeOnContact) Freeze();
            if (destoryOnContact) Destroy(gameObject, destroyDelay);
            Debug.Log(gameObject.name + " Has taken " + damage + " damage");
            if (other.gameObject.TryGetComponent<Knockback>(out Knockback knocked) && knockback) knocked.PushRigidBdy(rb);
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
