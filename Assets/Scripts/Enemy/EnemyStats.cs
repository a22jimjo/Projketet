using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{

    [Header("Stats")]
    [Tooltip("Characters health points")]
    public float health;
    public float maxHealth = 3;
    public float damage = 1;

    private EnemyGhostScript enemyGhostScript;

    [SerializeField] private AudioClip[] HurtClips;
    [SerializeField] private AudioClip[] DeathClips;

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
    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        enemyGhostScript.animator.SetTrigger("TakeDamage");
        AudioSource.PlayClipAtPoint(HurtClips[Random.Range(0, HurtClips.Length)], transform.TransformPoint(transform.position));


        if (health <= 0)
        {
            //Play death animation
            enemyGhostScript.animator.SetTrigger("Death");
            AudioSource.PlayClipAtPoint(DeathClips[Random.Range(0, DeathClips.Length)], transform.TransformPoint(transform.position));
            //enemyGhostScript.animator.SetBool("DeadGhost", true);
        }
        else
        {
            //Play get hit animation, apply knockback
        }
    }


}
