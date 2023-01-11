using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class EnemyGolem : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject player;
    Animator animator;
    Rigidbody rb;
    AudioSource audioSource;
    EnemyStats stats;
    [SerializeField] VisualEffect attackVFXPrefab;
    private VisualEffect attackEffectToPlay;
    [SerializeField] GameObject attackPoint;
    [SerializeField] Collider attackCollider;

    
    //Movement
    [Tooltip("The minimum distance between the character and it's target to attack")]
    [SerializeField] float attackRange;
    [Tooltip("The area where the enemy can detect the player")]
    [SerializeField] float detectRange;
    [Tooltip("Movement speed of the ghost")]
    [SerializeField] float baseMoveSpeed;
    
    //Attacking
    [SerializeField] bool canAttack = true;
    [SerializeField] bool isAttacking;
    [SerializeField] private float attackTimer;
    [SerializeField] private float attackCooldown;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private float delayAfterAttack;
    [SerializeField] private float attackDuration;
    
    [Tooltip("Sound starting when the windup animation ends")]
    [SerializeField] private AudioClip[] AttackClips;
    [SerializeField] private AudioClip[] SlamDunkClips;
    [SerializeField] private AudioClip[] WalkingClips;
    [Tooltip("Sound starting when the dash ends")]
    [SerializeField] private AudioClip[] WaitAfterAttackClips;
    
    [Header("Volumes")]
    [Range(0,1)] public float attackVolume;
    [Range(0,1)] public float slamDunkVolume;
    [Range(0,1)] public float walkingVolume;
    [Range(0,1)] public float attackWaitAfterAttackVolume;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        attackCollider = attackPoint.GetComponent<BoxCollider>();
        stats = GetComponent<EnemyStats>();

        agent.speed = baseMoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
        RotateTowards();
        
        if (Vector3.Distance(agent.destination, player.transform.position) <= detectRange && isAttacking == false)
        {
            animator.SetBool("isMoving", true);
            ChaseState();
        }else animator.SetBool("isMoving", false);
    }
    
    
    void ChaseState()
    {

        //updates position while outside of the attackrange
        if (Vector3.Distance(agent.destination, player.transform.position) > attackRange)
        {
            agent.destination = player.transform.position;
            audioSource.PlayOneShot(WalkingClips[Random.Range(0,WalkingClips.Length)], walkingVolume);
        }
        //if enemy is = or closer to the player than the attack range, can attack and has line of sight, it attacks.
        if (Vector3.Distance(agent.transform.position, player.transform.position) < attackRange && canAttack)
        {
            //randomizing the Delayafterattack var
            float f = Random.Range(delayAfterAttack * 0.8f, delayAfterAttack * 1.4f);

            //should rotate before executing attack
            StartCoroutine(Attack(attackDuration, f));
        }

        //Timer
        if (attackTimer < attackCooldown)
        {
            attackTimer += Time.deltaTime;
        }
        else
        {
            attackCooldown = attackTimer;
            canAttack = true;
        }
    }
    
        IEnumerator Attack(float waitTime, float waitTime3)
    {
        isAttacking = true;
        //stop movement briefly
        agent.speed = 0;
        //reset timers
        canAttack = false;
        attackTimer = 0;
        //run animation
        //Sound as windup animation start
        animator.SetTrigger("Attack");
        audioSource.PlayOneShot(AttackClips[Random.Range(0,AttackClips.Length)], attackVolume);
        //Sound as windup animation end
        yield return new WaitForSeconds(attackDuration);
        if (stats.health <= 0) yield break;
        attackPoint.SetActive(true);
        attackEffectToPlay = Instantiate(attackVFXPrefab, attackPoint.transform.position,attackPoint.transform.rotation);
        attackEffectToPlay.Play();
        audioSource.PlayOneShot(SlamDunkClips[Random.Range(0,SlamDunkClips.Length)], slamDunkVolume);
        yield return new WaitForSeconds(0.1f);
        attackPoint.SetActive(false);
        //Sound as dash has ended

        yield return new WaitForSeconds(delayAfterAttack);
        RotateTowards();
        agent.speed = baseMoveSpeed;
        isAttacking = false;
        animator.ResetTrigger("TakeDamage");
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.forward);

    }


    void RotateTowards()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
    
}
