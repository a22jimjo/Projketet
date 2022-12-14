using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGhostScript : MonoBehaviour
{
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

    //Attacking
    [SerializeField] bool canAttack = true;
    [SerializeField] bool isAttacking;
    [SerializeField] private float attackTimer;
    [SerializeField] private float attackCooldown;

    //Dash
    [SerializeField] private float dashSpeed;
    [SerializeField] private float delayBeforeAttack;
    [SerializeField] private float dashDuration;
    [SerializeField] private float delayAfterAttack;
    [SerializeField] private float rotationSpeed;

    private GameObject dashLocation;
    public GameObject attackIndicator;

    // Start is called before the first frame update
    void Start()
    {
        //setup
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        dashLocation = GameObject.FindGameObjectWithTag("GhostDashLocation");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        agent.speed = baseMoveSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(agent.destination, player.transform.position) <= detectRange && isAttacking == false)
        {
            RotateTowards();
            ChaseState();
        }
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
            StartCoroutine(Attack(delayBeforeAttack, dashDuration, delayAfterAttack));
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

    IEnumerator Attack(float waitTime, float waitTime2, float waitTime3)
    {
        //isInLineOfSight = false;
        Debug.Log("enemy attacks player");
        isAttacking = true;
        //stop movement briefly
        agent.speed = 0;

        //reset timers
        canAttack = false;
        attackTimer = 0;

        //Checks if destination is valid
        NavMeshHit hit;
        if (NavMesh.SamplePosition(dashLocation.transform.position, out hit, 1.0f, NavMesh.AllAreas))
        {
            if (hit.hit)
            {
                // The destination is valid and the agent can navigate to it
                agent.destination = dashLocation.transform.position;
                Instantiate(attackIndicator, agent.destination, Quaternion.identity);

            }
            else
            {
                // The destination is not valid and the agent cannot navigate to it
                Debug.Log("The destination is not valid and the agent cannot navigate to it, attacks player position instead");
                agent.destination = player.transform.position;
                Instantiate(attackIndicator, agent.destination, Quaternion.identity);
            }
        }

        //run animation
        animator.SetBool("AttackGhost", true);
        yield return new WaitForSeconds(delayBeforeAttack);
        agent.speed = dashSpeed;

        yield return new WaitForSeconds(dashDuration);
        yield return new WaitForSeconds(delayAfterAttack);
        RotateTowards();
        agent.speed = baseMoveSpeed;
        isAttacking = false;
        animator.SetBool("AttackGhost", false);
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

    void StopAttack()
    {
        agent.speed = baseMoveSpeed;
        isAttacking = false;
        Debug.Log("Stopping coroutine");
        StopCoroutine(Attack(delayBeforeAttack, dashDuration, delayAfterAttack));
    }
}
