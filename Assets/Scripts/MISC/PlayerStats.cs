using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("Characters health points")]
    public float health;
    public float maxHealth = 3;
    public float damage;
    public float heavyDamage;

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
    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        StartCoroutine(SlowDownABit());
        _healthbar.UpdateHealthBar(maxHealth, health);
        
        if (health <= 0)
        {
            //Play death animation
            Debug.Log("Player has died");

            StartCoroutine(SlowDown());

            //Destroy(gameObject);
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

    IEnumerator SlowDown()
    {
        Time.timeScale = 0.4f;
        yield return new WaitForSeconds(1);
        Time.timeScale = 1;
    }

    IEnumerator SlowDownABit()
    {
        Time.timeScale = 0.7f;
        yield return new WaitForSeconds(.2f);
        Time.timeScale = 1;
    }
}
