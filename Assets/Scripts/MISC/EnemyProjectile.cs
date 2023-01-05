using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    Rigidbody rb;
    [SerializeField] private int damage;
    [SerializeField] public float destroyDelay;
    [SerializeField] public bool destoryOnContact;
    [SerializeField] public bool freezeOnContact;
    [SerializeField] private float damageTimer;
    [SerializeField] private float damageCooldown;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        damageTimer = damageCooldown;
    }

    // Update is called once per frame
    void Update()
    {

        if(damageTimer < damageCooldown)
        {
            damageTimer += Time.deltaTime;
        }
        else
        {
            damageTimer = damageCooldown;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && damageTimer == damageCooldown)
        {
            damageTimer = 0;
            other.gameObject.TryGetComponent<PlayerStats>(out PlayerStats stats);
            Debug.Log("player was hit by a projectile");
            if (stats.TakeDamage(damage))
            {

                if (freezeOnContact) Freeze();
                if (destoryOnContact)
                {
                    Destroy(gameObject);
                }
                else
                {
                    Destroy(gameObject, destroyDelay);
                }
            }
        }
    }


    public void Freeze()
    {
        rb.constraints = RigidbodyConstraints.FreezePosition;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
}
