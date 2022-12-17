using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    private Animator animator;

    [Header("Stats")]
    [Tooltip("Characters health points")]
    public float health;
    public float maxHealth = 3;
    public float damage = 1;

    [SerializeField] private ParticleSystem deathEffect;

    [SerializeField] private AudioClip[] HurtClips;
    [SerializeField] private AudioClip[] DeathClips;
    [SerializeField] private float DelayUntilDestroy;
    [SerializeField] private Healthbar _healthbar;

    private GameObject Enemy;

    // Start is called before the first frame update
    void Start()
    {
        //Setup
        health = maxHealth;
        animator = GetComponent<Animator>();
        _healthbar.UpdateHealthBar(maxHealth, health);
        Enemy = GameObject.FindGameObjectWithTag("Enemy");
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
            Debug.Log("Should be playing hurt animation :)");
            //animator.ResetTrigger("Attack");
            animator.SetTrigger("TakeDamage");
            AudioSource.PlayClipAtPoint(HurtClips[Random.Range(0, HurtClips.Length)], transform.TransformPoint(transform.position));
        }
    }

    IEnumerator Death(float waitTime)
    {
        Debug.Log("Should be playing death animation :)");
        animator.ResetTrigger("Attack");
        animator.SetBool("isDead", true);
        AudioSource.PlayClipAtPoint(DeathClips[Random.Range(0, DeathClips.Length)], transform.TransformPoint(transform.position));
        //Play death VFX
        Instantiate(deathEffect, Enemy.transform);
        yield return new WaitForSeconds(DelayUntilDestroy);

        Instantiate(deathEffect, Enemy.transform);

        Destroy(gameObject);
    }


}
