using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("Characters health points")]
    public float health;
    public float maxHealth = 3;
    public float damage;
    public float heavyDamage;
    [SerializeField] private AudioClip[] DeathClips;

    private AudioSource audioSource;

    private ThirdPersonController _ThirdPersonController;
    [SerializeField] public Healthbar _healthbar;

    // Start is called before the first frame update
    void Start()
    {
        //Setup
        health = maxHealth;
        _ThirdPersonController = GetComponent<ThirdPersonController>();
        _healthbar.UpdateHealthBar(maxHealth, health);
        audioSource = GetComponent<AudioSource>();
    }
    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        StartCoroutine(SlowDownABit());
        _healthbar.UpdateHealthBar(maxHealth, health);
        _ThirdPersonController.TakeDamage();
        
        if (health <= 0)
        {
            //Play death animation
            Debug.Log("Player has died");

            StartCoroutine(SlowDown());
            audioSource.PlayOneShot(DeathClips[Random.Range(0, DeathClips.Length)], 1);
            health = maxHealth;
            _healthbar.UpdateHealthBar(maxHealth, health);
            SceneManager.LoadSceneAsync("Start scen RestartScene");
            //Destroy(gameObject);
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
