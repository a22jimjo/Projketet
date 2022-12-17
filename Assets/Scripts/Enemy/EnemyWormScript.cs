using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWormScript : MonoBehaviour
{
    List<Transform> wayPoints = new List<Transform>();
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

    //Moving
    [SerializeField] private bool canMove;
    [SerializeField] private float moveTimer;
    [SerializeField] private float moveCooldown;
    [SerializeField] private float BurrowDownWait;
    [SerializeField] private float BurrowWaitTime;
    [SerializeField] private float BurrowUpWait;

    //[SerializeField] GameObject badring;
    [SerializeField] GameObject wormBody;
    [SerializeField] GameObject enemyWorm;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform firePoint;

    //current state
    [SerializeField] private bool ismoving;
    [SerializeField] private bool isAttacking;



    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        agent.speed = baseMoveSpeed;

        //sets up patrolstate waypoints
        GameObject go = GameObject.FindGameObjectWithTag("Waypoints");
        foreach (Transform t in go.transform)
        {
            wayPoints.Add(t);
        }
    }

    // Update is called once per frame
    void Update()
    {
        RotateTowards();
        
        //if move timer = move cd && we are not attacking. start moving
        if (canMove && !isAttacking && !ismoving)
        {
            StartCoroutine(Move(BurrowDownWait, BurrowWaitTime, BurrowUpWait));

        }

        //if attack timer = attack cd && we are not moving. start attacking
        if (canAttack && !ismoving && !isAttacking)
        {
            StartCoroutine(Attack(waitBetweenAttacks, anticipationTime));

        }

        if(!isAttacking && !ismoving)
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
            //Timer movement
            if (moveTimer < moveCooldown)
            {
                moveTimer += Time.deltaTime;
            }
            else
            {
                moveTimer = moveCooldown;
                canMove = true;

            }
        }
        
    }

    IEnumerator Move(float BurrowDownWait, float BurrowWaitTime, float BurrowUpWait)
    {
        moveTimer = 0;
        canMove = false;
        ismoving = true;
        Debug.Log("Worm should be Moving");

        //Play coming down animation
        animator.SetTrigger("BurrowDown");
        yield return new WaitForSeconds(BurrowDownWait);
        enemyWorm.GetComponent<Collider>().enabled = false;
        wormBody.SetActive(false);

        //Set new destination
        agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
        Debug.Log("should have set destination");

        //Spawn VFX that follows the enemy, showing the underground location as it travels
        
        yield return new WaitForSeconds(BurrowWaitTime);
        Debug.Log("Worm has reached it's destination but after waittime");

        //remove follow vfx, spawn in coming up VFX.

        animator.ResetTrigger("BurrowDown");
        wormBody.SetActive(true);
        animator.SetTrigger("BurrowUp");
        yield return new WaitForSeconds(BurrowUpWait);
        enemyWorm.GetComponent<Collider>().enabled = true;

        moveTimer = 0;
        canMove = false;
        ismoving = false;

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
            yield return new WaitForSeconds(waitBetweenAttacks);
        }

        attackTimer = 0;
        canAttack = false;
        isAttacking = false;

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
