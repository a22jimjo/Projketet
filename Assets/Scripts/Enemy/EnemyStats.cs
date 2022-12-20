using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;

    [Header("Stats")]
    [Tooltip("Characters health points")]
    public float health;
    public float maxHealth = 3;
    public float damage = 1;


    [SerializeField] private AudioClip[] HurtClips;
    [SerializeField] private AudioClip[] DeathClips;
    [SerializeField] private float DelayUntilDestroy;
    [SerializeField] private Healthbar _healthbar;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //Setup
        health = maxHealth;
        animator = GetComponent<Animator>();
        _healthbar.UpdateHealthBar(maxHealth, health);
        audioSource = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
    }
    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        _healthbar.UpdateHealthBar(maxHealth, health);

        if (health <= 0)
        {
            StartCoroutine(Death(DelayUntilDestroy));
        }
        else
        {
            //animator.ResetTrigger("Attack");
            animator.SetTrigger("TakeDamage");
            audioSource.PlayOneShot(HurtClips[Random.Range(0, HurtClips.Length)]);
        }
    }

    IEnumerator Death(float waitTime)
    {
        //animator.ResetTrigger("Attack");
        agent.speed = 0;
        this.GetComponent<Collider>().enabled = false;
        animator.SetBool("isDead", true);
        audioSource.PlayOneShot(HurtClips[Random.Range(0, HurtClips.Length)]);
        audioSource.PlayOneShot(DeathClips[Random.Range(0, DeathClips.Length)], 0.8f);

        //Play death VFX
        yield return new WaitForSeconds(DelayUntilDestroy);
        Destroy(gameObject);
    }


}
