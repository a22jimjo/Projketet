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
    [SerializeField] private Healthbar _healthbar;

    // Start is called before the first frame update
    void Start()
    {
        //Setup
        health = maxHealth;
        _ThirdPersonController = GetComponent<ThirdPersonController>();
        _healthbar.UpdateHealthBar(maxHealth, health);
    }
    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        _healthbar.UpdateHealthBar(maxHealth, health);
        
        if (health <= 0)
        {
            //Play death animation
            Destroy(gameObject);
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
