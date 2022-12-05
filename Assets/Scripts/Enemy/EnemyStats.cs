using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyStats : MonoBehaviour
{
    public static event Action<EnemyStats> OnEnemyKilled;

    [Header("Stats")]
    [Tooltip("Characters health points")]
    public int health;
    public int maxHealth = 3;
    public int damage = 1;

    private EnemyGhostScript enemyGhostScript;

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
            //enemyGhostScript.animator.SetBool("DeadGhost", true);
            Destroy(gameObject);
            OnEnemyKilled?.Invoke(this);
        }
        else
        {
            //Play get hit animation, apply knockback
        }
    }


}
