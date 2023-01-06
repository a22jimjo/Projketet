using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    private Animator animator;

    private ThirdPersonController _ThirdPersonController;
    [SerializeField] public Healthbar _healthbar;

    public GameObject deathScreen;
    public GameObject tryAgainButton;


    // Start is called before the first frame update
    void Start()
    {
        //Setup
        health = maxHealth;
        _ThirdPersonController = GetComponent<ThirdPersonController>();
        _healthbar.UpdateHealthBar(maxHealth, health);
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        //deathScreen = GameObject.FindGameObjectWithTag("DeathScreen");
        //tryAgainButton = GameObject.FindGameObjectWithTag("TryAgainButton");
        //tryAgainButton.SetActive(false);
        //deathScreen.SetActive(false);
    }
    public bool TakeDamage(float damageAmount)
    {
        if (_ThirdPersonController.TakeDamage())
        {
            health -= damageAmount;
            StartCoroutine(SlowDownABit());
            _healthbar.UpdateHealthBar(maxHealth, health);

            if (health <= 0)
            {
                StartCoroutine(PlayerDeath());
            }

            return true;
        }

        return false;
    }

    IEnumerator SlowDownABit()
    {
        Time.timeScale = 0.7f;
        yield return new WaitForSeconds(.2f);
        Time.timeScale = 1;
    }

    public IEnumerator PlayerDeath()
    {
        //Play death animation
        animator.SetBool("Death", true);
        _ThirdPersonController._fixedPosition = true;
        _ThirdPersonController.invincible = true;
        Debug.Log("Player has died");
        audioSource.PlayOneShot(DeathClips[Random.Range(0, DeathClips.Length)], 1);
        Time.timeScale = 0.25f;
        yield return new WaitForSeconds(2f);
        Time.timeScale = 1;

        //enable deathscreen
        deathScreen.SetActive(true);
        _ThirdPersonController.invincible = false;
        //disable anim
        animator.SetBool("Death", false);
        //loads startsceen
        SceneManager.LoadSceneAsync("Start scen RestartScene");
        //wait 0.5s
        yield return new WaitForSeconds(.5f);

        //enable button to play again (fade in?)
        tryAgainButton.SetActive(true);

    }
    public void RespawnPlayer()
    {

        //remove deathscreen
        


        //set player position
        gameObject.SetActive(false);
        GameObject spawnPoint;
        spawnPoint = GameObject.FindGameObjectWithTag("PlayerStartPosition");
        gameObject.transform.position = spawnPoint.transform.position;
        gameObject.SetActive(true);
        _ThirdPersonController.MoveSpeed = _ThirdPersonController.MoveSpeed = 4;

        //reset health values
        health = maxHealth;
        _healthbar.UpdateHealthBar(maxHealth, health);

        tryAgainButton.SetActive(false);
        deathScreen.SetActive(false);
    }
}
