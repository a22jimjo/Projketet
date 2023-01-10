using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class EnemyBossGolem : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject player;
    Animator animator;
    Rigidbody rb;
    AudioSource audioSource;
    [SerializeField] VisualEffect attackVFXPrefab;
    [SerializeField] VisualEffect attackVfxOneHandPrefab;
    private VisualEffect attackEffectToPlay;
    [SerializeField] GameObject attackPoint;
    [SerializeField] GameObject attackPoint2;
    [SerializeField] GameObject attackPointOneHand;
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

    [SerializeField] private float delayAfterOneHandAttack;
    [SerializeField] private float oneHandAttackDuration;
    
    [Tooltip("Sound starting when the windup animation ends")]
    [SerializeField] private AudioClip[] AttackClips;
    [Tooltip("Sound starting when the dash ends")]
    [SerializeField] private AudioClip[] WaitAfterAttackClips;
    
    [Header("Volumes")]
    [Range(0,1)] public float attackVoulume;
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

        agent.speed = baseMoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        RotateTowards(1);
        
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
        }
        //if enemy is = or closer to the player than the attack range, can attack and has line of sight, it attacks.
        if (Vector3.Distance(agent.transform.position, player.transform.position) < attackRange && canAttack)
        {
            //randomizing the Delayafterattack var
            delayAfterAttack = Random.Range(delayAfterAttack * 0.8f, delayAfterAttack * 1.4f);

            //should rotate before executing attack
            StartCoroutine(Attack());
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
    
        IEnumerator Attack()
    {
        Debug.Log("Golem attacks player");
        isAttacking = true;
        //stop movement briefly
        agent.speed = 0;
        //reset timers
        canAttack = false;
        attackTimer = 0;
        //run animation
        //Sound as windup animation start
        animator.SetTrigger("Attack");
        //Sound as windup animation end
        yield return new WaitForSeconds(attackDuration);
        attackPoint.SetActive(true);
        attackPoint2.SetActive(true);
        attackEffectToPlay = Instantiate(attackVFXPrefab, attackPoint.transform.position,attackPoint.transform.rotation);
        attackEffectToPlay.Play();
        attackEffectToPlay = Instantiate(attackVFXPrefab, attackPoint2.transform.position,attackPoint2.transform.rotation);
        attackEffectToPlay.Play();
        audioSource.PlayOneShot(AttackClips[Random.Range(0,AttackClips.Length)], attackVoulume);
        yield return new WaitForSeconds(0.1f);
        attackPoint.SetActive(false);
        attackPoint2.SetActive(false);
        //Sound as dash has ended
        
        yield return new WaitForSeconds(delayAfterAttack);
        animator.SetTrigger("Onehand");
        yield return new WaitForSeconds(oneHandAttackDuration);
        attackPointOneHand.SetActive(true);
        attackEffectToPlay = Instantiate(attackVfxOneHandPrefab, attackPointOneHand.transform.position,attackPointOneHand.transform.rotation);
        attackEffectToPlay.Play();
        audioSource.PlayOneShot(AttackClips[Random.Range(0,AttackClips.Length)], attackVoulume);
        yield return new WaitForSeconds(0.1f);
        attackPointOneHand.SetActive(false);
            
        yield return new WaitForSeconds(delayAfterAttack);
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


    void RotateTowards(float bonusSpeed)
    {
        Debug.Log("rotating towards player");

        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed * bonusSpeed);
    }
    
}
