using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWormScript : MonoBehaviour
{
    List<Transform> wayPoints = new List<Transform>();
    NavMeshAgent agent;
    GameObject player;
    GameObject wormBody;
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
    [SerializeField] bool canAttack = true;
    [SerializeField] bool isAttacking;
    [SerializeField] private float attackTimer;
    [SerializeField] private float attackCooldown;

    //Attacking
    [SerializeField] private float BurrowDownWait;
    [SerializeField] private float AttackDuration;
    [SerializeField] private float delayAfterAttack;

    public GameObject badring;

    

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
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

        //check if attack is possible, canAttack = true, !Moving, player nearby?
        if (canAttack)
        {
            StartCoroutine(Move(BurrowDownWait));

        }


    }

    IEnumerator Move(float BurrowDownWait)
    {
        Debug.Log("Worm should be Moving");
        //Play coming down animation
        animator.SetTrigger("BurrowDown");
        yield return new WaitForSeconds(BurrowDownWait);

        //transform.position = transform.position + new Vector3 (0f,-3f, 0f);
        //Remove badring
        badring.SetActive(false);
        //Set new destination
        agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
        //Spawn VFX that follows the enemy, showing the underground location as it travels

        //have we reached our destination?
        if (agent.transform.position == agent.destination)
        {
            //play coming up animation, spawn in badring
            //remove follow vfx, spawn in coming up VFX.
            badring.SetActive(true);
        }
        canAttack = false;
    }

    IEnumerator Attack(float waitTime)
    {
        Debug.Log("enemy attacks player");
        isAttacking = true;
        yield return new WaitForSeconds(waitTime);

        //reset timers
        canAttack = false;
        attackTimer = 0;
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
}
