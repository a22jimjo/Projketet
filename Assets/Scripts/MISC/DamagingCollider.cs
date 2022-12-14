using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamagingCollider : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] public float destroyDelay;
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

    public void CallDamage(Collider other, bool knockback, float dmg)
    {
        if(other.gameObject.TryGetComponent<EnemyStats>(out EnemyStats enemyComponent ) && canHitEnemy)
        {
            
            enemyComponent.TakeDamage(dmg);
            if (freezeOnContact) Freeze();
            if (destoryOnContact) Destroy(gameObject, destroyDelay);
            Debug.Log(gameObject.name + " Has dealt " + dmg + " damage");
            if (other.gameObject.TryGetComponent<Knockback>(out Knockback knocked) && knockback) knocked.PushRigidBdy(rb);
        }
        
        if (canHitPlayer)
        {
            if (other.gameObject.TryGetComponent<PlayerStats>(out PlayerStats playerComponent))
            {
                playerComponent.TakeDamage(dmg);
                if (freezeOnContact) Freeze();
                if (destoryOnContact) Destroy(gameObject, destroyDelay);
                Debug.Log(gameObject.name + " Has taken " + dmg + " damage");
            }
        }
    } 

    public void Freeze()
    {
        rb.constraints = RigidbodyConstraints.FreezePosition;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
}
