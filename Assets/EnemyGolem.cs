using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGolem : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject player;
    Animator animator;
    Rigidbody rb;
    AudioSource audioSource;
    
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
    [SerializeField] private float delayBeforeAttack;
    
    [Tooltip("Sound starting when the windup animation start")]
    [SerializeField] private AudioClip[] AttackWindupClips;
    [Tooltip("Sound starting when the windup animation ends")]
    [SerializeField] private AudioClip[] AttackClips;
    [Tooltip("Sound starting when the dash ends")]
    [SerializeField] private AudioClip[] WaitAfterAttackClips;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        agent.speed = baseMoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Vector3.Distance(agent.destination, player.transform.position) <= detectRange && isAttacking == false)
        {
            animator.SetBool("isMoving", true);
            RotateTowards();
            ChaseState();
        }else animator.SetBool("isMoving", false);
    }
    
    
    void ChaseState()
    {

        //updates position while outside of the attackrange
        if (Vector3.Distance(agent.destination, player.transform.position) > attackRange)
        {
            agent.destination = player.transform.position;
        }
        //if enemy is = or closer to the player than the attack range, can attack and has line of sight, it attacks.
        if (Vector3.Distance(agent.transform.position, player.transform.position) < attackRange && canAttack)
        {
            //randomizing the Delayafterattack var
            delayAfterAttack = Random.Range(delayAfterAttack * 0.8f, delayAfterAttack * 1.4f);

            //should rotate before executing attack
            StartCoroutine(Attack(delayBeforeAttack, delayAfterAttack));
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
        agent.avoidancePriority = 49;
        Debug.Log("enemy attacks player");
        isAttacking = true;
        //stop movement briefly
        agent.speed = 0;

        //reset timers
        canAttack = false;
        attackTimer = 0;
        
        //run animation
        //Sound as windup animation start
        audioSource.PlayOneShot(AttackWindupClips[Random.Range(0, AttackWindupClips.Length)]);
        //animator.SetBool("AttackGhost", true);
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(delayBeforeAttack);
        //Sound as windup animation end
        audioSource.PlayOneShot(AttackClips[Random.Range(0,AttackClips.Length)]);
        agent.acceleration = 0;
        agent.speed = 0;

        yield return new WaitForSeconds(attackDuration);
        //Sound as dash has ended
        audioSource.PlayOneShot(WaitAfterAttackClips[Random.Range(0, WaitAfterAttackClips.Length)]);

        yield return new WaitForSeconds(delayAfterAttack);
        RotateTowards();
        agent.acceleration = 8;
        agent.speed = baseMoveSpeed;
        isAttacking = false;
        agent.avoidancePriority = 50;
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
        Debug.Log("rotating towards player");

        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
    
}
