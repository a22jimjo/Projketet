using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestEnemyGhostController : MonoBehaviour
{
    //References
    Animator animator;
    Rigidbody rb;
    NavMeshAgent agent;
    GameObject player;

    [SerializeField] float detectRange;
    [SerializeField] float attackRange;
    [SerializeField] float baseMoveSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] bool shouldRotate;
    [SerializeField] bool isAttacking;
    [SerializeField] float attackCooldown;

    Ray ray;
    [SerializeField] float maxDistance;
    public LayerMask layersToHit;

    private float attackTimer;
    private bool hasBeenDetected;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");

        agent.speed = baseMoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (shouldRotate)
        {
            RotateTowards();
        }
        // if within detectRange and is not attacking
        if (Vector3.Distance(agent.destination, player.transform.position) <= detectRange && !isAttacking)
        {
            ChaseState();
        }

        //if in attackrange and attackcooldown = timer
        if (Vector3.Distance(agent.destination, player.transform.position) <= attackRange && attackTimer == attackCooldown)
        {
            AttackState();
        }*/


        //------------------------------------------------------------------------------------------


        if(Vector3.Distance(agent.destination, player.transform.position) <= detectRange)
        {
            AlertState();
        }
        

        if (hasBeenDetected)
        {
            if (Vector3.Distance(agent.destination, player.transform.position) <= attackRange && attackTimer == attackCooldown)
            {
                AttackState();
            }
            
            
            if (!isAttacking)
            {
                ChaseState();
            }

            if (!isAttacking)
            {
                RotateTowards();

            }
        }

        //Timer
        if (attackTimer < attackCooldown)
        {
            attackTimer += Time.deltaTime;
        }
        else
        {
            attackCooldown = attackTimer;
        }
    }

    void ChaseState()
    {
        Debug.Log("chasestate");
        shouldRotate = true;
        agent.destination = player.transform.position;
    }

    void AttackState()
    {
        isAttacking = true;
        Debug.Log("attackstate");
        //attack
        agent.destination = agent.transform.position;

        isAttacking = false;
    }

    void AlertState()
    {
        //play dectected/alert animation here
        hasBeenDetected = true;
    }

    void RotateTowards()
    {
        Debug.Log("rotating towards player");

        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
