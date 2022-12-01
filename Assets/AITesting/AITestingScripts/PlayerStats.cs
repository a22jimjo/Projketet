using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("Characters health points")]
    public int health;
    public int maxHealth = 3;

    // Start is called before the first frame update
    void Start()
    {
        //Setup
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            //Play death animation

            Destroy(gameObject);
        }
        else
        {
            //Play get hit animation, apply knockback
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("Aj!");
            if (other.gameObject.TryGetComponent<EnemyStats>(out EnemyStats enemyComponent))
            {
                TakeDamage(enemyComponent.damage);
            }
        }
    }
}
