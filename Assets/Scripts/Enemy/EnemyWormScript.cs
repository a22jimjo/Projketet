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

    //Movement
    [Tooltip("The minimum distance between the character and it's target to attack")]
    [SerializeField] float attackRange;
    [Tooltip("The area where the enemy can detect the player")]
    [SerializeField] float detectRange;
    [Tooltip("Movement speed of the ghost")]
    [SerializeField] float baseMoveSpeed;
    [Tooltip("The speed that the enemy rotates towards it's target")]
    [SerializeField] private float rotationSpeed;


    //Checks if you can attack
    [SerializeField] private bool canAttack = true;
    [SerializeField] private float attackTimer;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float waitBetweenAttacks;
    [SerializeField] private float anticipationTime;

    //Moving
    [SerializeField] private bool shouldMove;
    [SerializeField] private float BurrowDownWait;
    [SerializeField] private float BurrowWaitTime;
    [SerializeField] private float BurrowUpWait;

    //[SerializeField] GameObject badring;
    [SerializeField] GameObject wormBody;
    [SerializeField] GameObject enemyWorm;
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject firePoint;

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
        
        //check if attack is possible, canAttack = true, !Moving, player nearby?
        if (shouldMove)
        {
            StartCoroutine(Move(BurrowDownWait, BurrowWaitTime, BurrowUpWait));

        }

        //check if attack is possible, canAttack = true, !Moving, player nearby?
        if (canAttack && !ismoving)
        {
            StartCoroutine(Attack(waitBetweenAttacks, anticipationTime));

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

    IEnumerator Move(float BurrowDownWait, float BurrowWaitTime, float BurrowUpWait)
    {
        shouldMove = false;
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

        //have we reached our destination?
        if (Vector3.Distance(agent.transform.position, agent.destination) < 2f)
        {
            StartCoroutine(Move(BurrowDownWait, BurrowWaitTime, BurrowUpWait));
        }
        else
        {
            yield return new WaitForSeconds(BurrowWaitTime);
            Debug.Log("Worm has reached it's destination but after waittime");

            //remove follow vfx, spawn in coming up VFX.

            animator.ResetTrigger("BurrowDown");
            animator.SetTrigger("BurrowUp");
            yield return new WaitForSeconds(BurrowUpWait);
            enemyWorm.GetComponent<Collider>().enabled = true;
            wormBody.SetActive(true);
            ismoving = false;
        }

        
    }

    IEnumerator Attack(float waitBetweenAttacks, float anticipationTime)
    {
        Debug.Log("enemy attacks player");

        isAttacking = true;

        //reset timers
        canAttack = false;
        attackTimer = 0;

        animator.SetTrigger("AttackMultiple");
        yield return new WaitForSeconds(anticipationTime);
        Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation);
        yield return new WaitForSeconds(waitBetweenAttacks);
        Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation);
        yield return new WaitForSeconds(waitBetweenAttacks);
        Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation);
        yield return new WaitForSeconds(waitBetweenAttacks);
        Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation);
        yield return new WaitForSeconds(waitBetweenAttacks);

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
