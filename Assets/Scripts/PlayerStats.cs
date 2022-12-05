using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("Characters health points")]
    public int health;
    public int maxHealth = 3;

    private ThirdPersonController _ThirdPersonController;

    // Start is called before the first frame update
    void Start()
    {
        //Setup
        health = maxHealth;
        _ThirdPersonController = GetComponent<ThirdPersonController>();
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
                _ThirdPersonController.TakeDamage();
                TakeDamage(enemyComponent.damage);
            }
        }
    }
}
