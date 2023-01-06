using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWormBossHeadScript : MonoBehaviour
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
    [Tooltip("The speed that the enemy rotates towards it's target")]
    [SerializeField] private float rotationSpeed;
    [Tooltip("The speed at which the projectile travels")]
    [SerializeField] private float projectileSpeed;


    //Checks if you can attack
    [SerializeField] private bool canAttack = true;
    [SerializeField] private float attackTimer;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float waitBetweenAttacks;
    [SerializeField] private float anticipationTime;
    [SerializeField] private AudioClip[] attackClips;

    //[SerializeField] GameObject badring;
    [SerializeField] GameObject wormBody;
    [SerializeField] GameObject enemyWorm;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform firePoint;
    
    [SerializeField] private bool isAttacking;
    [SerializeField] private bool hasDetectedPlayer = false;

    private bool ismoving;



    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        RotateTowards();

        if (Vector3.Distance(agent.destination, player.transform.position) <= detectRange)
        {
            hasDetectedPlayer = true;
        }

        if (hasDetectedPlayer)
        {

            //if attack timer = attack cd && we are not moving. start attacking
            if (canAttack && !ismoving && !isAttacking)
            {
                StartCoroutine(Attack(waitBetweenAttacks, anticipationTime));

            }

            if (!isAttacking && !ismoving)
            {
                //Timer attack
                if (attackTimer < attackCooldown)
                {
                    attackTimer += Time.deltaTime;
                }
                else
                {
                    attackTimer = attackCooldown;
                    canAttack = true;

                }
            }
        
        }
        
    }

    IEnumerator Attack(float waitBetweenAttacks, float anticipationTime)
    {
        Debug.Log("enemy attacks player");
        isAttacking = true;

        //reset timers
        canAttack = false;
        attackTimer = 0;

        animator.ResetTrigger("TakeDamage");
        animator.SetTrigger("AttackMultiple");

        yield return new WaitForSeconds(anticipationTime);


        for (int i = 0; i < 4; i++)
        {
            GameObject _projectile = Instantiate(projectile, firePoint.position, transform.rotation);
            _projectile.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
            audioSource.PlayOneShot(attackClips[Random.Range(0, attackClips.Length)], 0.5f);
            yield return new WaitForSeconds(waitBetweenAttacks);
        }

        attackTimer = 0;
        canAttack = false;
        isAttacking = false;

    }

    public void CallMove(float a, float b, float c)
    {
        StartCoroutine(Move(a, b, c));
    }

    IEnumerator Move(float BurrowDownWait, float BurrowWaitTime, float BurrowUpWait)
    {
        ismoving = true;
        Debug.Log("Worm should be Moving");

        //Play coming down animation
        animator.SetTrigger("BurrowDown");
        yield return new WaitForSeconds(BurrowDownWait);
        enemyWorm.GetComponent<Collider>().enabled = false;
        wormBody.SetActive(false);

        //Spawn VFX that follows the enemy, showing the underground location as it travels

        yield return new WaitForSeconds(BurrowWaitTime);
        Debug.Log("Worm has reached it's destination but after waittime");

        //remove follow vfx, spawn in coming up VFX.

        animator.ResetTrigger("BurrowDown");
        wormBody.SetActive(true);
        animator.SetTrigger("BurrowUp");
        yield return new WaitForSeconds(BurrowUpWait);
        enemyWorm.GetComponent<Collider>().enabled = true;
        
        ismoving = false;
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
