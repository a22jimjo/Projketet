using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGhostScript : MonoBehaviour
{

    List<Transform> wayPoints = new List<Transform>();
    UnityEngine.AI.NavMeshAgent agent;
    GameObject player;
    public Animator animator;
    Rigidbody rb;


    [Tooltip("The minimum distance between the character and it's target to attack")]
    [SerializeField] float attackRange;
    [Tooltip("The area where the enemy can detect the player")]
    [SerializeField] float detectRange;
    [Tooltip("Movement speed of the ghost")]
    [SerializeField] float baseMoveSpeed;

    //Movement
    [SerializeField] bool canAttack = true;
    [SerializeField] bool isAttacking;
    [SerializeField] private float attackTimer;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float delayBeforeAttack;
    [SerializeField] private float dashDuration;
    [SerializeField] private float delayAfterAttack;
    [SerializeField] private float dashForce;
    [SerializeField] private float rotationSpeed;

    Ray ray;
    [SerializeField]float maxDistance = 50;
    public LayerMask layersToHit;
    [SerializeField] bool isInLineOfSight;
    [SerializeField] float SphereCastRadius;


    //public GameObject testIndicator;

    // Start is called before the first frame update
    void Start()
    {
        //setup
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        agent.speed = baseMoveSpeed;

        ray = new Ray(transform.position, transform.forward);

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(agent.destination, player.transform.position) <= detectRange && isAttacking == false)
        {
            RotateTowards();
            ChaseState();
            CheckForColliders();
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
        if (Vector3.Distance(agent.transform.position, player.transform.position) < attackRange && canAttack && isInLineOfSight)
        {
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
        isInLineOfSight = false;
        Debug.Log("enemy attacks player");
        isAttacking = true;
        //stop movement briefly
        agent.speed = 0;

        //reset timers
        canAttack = false;
        attackTimer = 0;

        //gets player position

        //agent.destination = player.transform.position;
        rb.AddForce(transform.forward * dashForce);


        //Instantiate(testIndicator, player.transform.position, Quaternion.identity);

        //run animation
        animator.SetBool("AttackGhost", true);
        yield return new WaitForSeconds(delayBeforeAttack);
        //damagingCollider.enabled = true;
        agent.speed = dashSpeed; 

        yield return new WaitForSeconds(dashDuration);
        yield return new WaitForSeconds(delayAfterAttack);

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

    void CheckForColliders()
    {


        if(Physics.SphereCast(ray, SphereCastRadius, out RaycastHit hit, maxDistance, layersToHit))
        {
            Debug.Log(hit.collider.gameObject.name + " was hit!");
            isInLineOfSight = true;
        }
    }

    void RotateTowards()
    {
        Debug.Log("rotating towards player");

        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
} 
