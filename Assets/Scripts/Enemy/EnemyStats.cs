using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public float defense = 1;


    [SerializeField] private AudioClip[] HurtClips;
    [SerializeField] private AudioClip[] DeathClips;
    [SerializeField] private float DelayUntilDestroy;
    [SerializeField] private Healthbar _healthbar;

    private AudioSource audioSource;
    
    private bool[] damageTriggers = new bool[3];

    // Start is called before the first frame update
    void Start()
    {
        //Setup
        health = maxHealth;
        animator = GetComponent<Animator>();
        _healthbar.UpdateHealthBar(maxHealth, health);
        audioSource = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        for (int i = damageTriggers.Length - 1; i >= 0; i--)
        {
            damageTriggers[i] = true;
        }
    }
    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount - defense;
        _healthbar.UpdateHealthBar(maxHealth, health);

        if (health <= 0)
        {
            StartCoroutine(Death(DelayUntilDestroy));
        }
        else
        {
            Debug.Log("Tar skada");
            if (health < maxHealth * 0.1f && damageTriggers[2]) damageTriggers[2] = false;
            else if (health < maxHealth * 0.25f && damageTriggers[1]) damageTriggers[1] = false;
            else if (health < maxHealth * 0.5f && damageTriggers[0]) damageTriggers[0] = false;
            else return;
            //animator.ResetTrigger("Attack");
            animator.SetTrigger("TakeDamage");
            audioSource.PlayOneShot(HurtClips[Random.Range(0, HurtClips.Length)]);
        }
    }

    IEnumerator Death(float waitTime)
    {
        //animator.ResetTrigger("Attack");
        GetComponent<Collider>().enabled = false;
        agent.speed = 0;
        animator.SetBool("isDead", true);
        audioSource.PlayOneShot(HurtClips[Random.Range(0, HurtClips.Length)]);
        audioSource.PlayOneShot(DeathClips[Random.Range(0, DeathClips.Length)], 0.8f);

        //Play death VFX
        yield return new WaitForSeconds(DelayUntilDestroy);
        Destroy(gameObject);
    }


}
