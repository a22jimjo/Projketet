using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGhostScript : MonoBehaviour
{

    List<Transform> wayPoints = new List<Transform>();
    UnityEngine.AI.NavMeshAgent agent;
    GameObject player;
    public Animator animator;


    [Tooltip("The minimum distance between the character and it's target to attack")]
    [SerializeField] float attackRange;
    [Tooltip("The area where the enemy can detect the player")]
    [SerializeField] float detectRange;
    [Tooltip("Movement speed of the ghost")]
    [SerializeField] float baseMoveSpeed;
    [Tooltip("The amount of time the ghost waits after an attack")]
    [SerializeField] float waitAfterAttack;

    //Movement
    [SerializeField] float rotationSpeed;

    [SerializeField] bool canAttack = true;
    [SerializeField] bool isAttacking;
    [SerializeField] private float attackTimer;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float windupTime;

    public GameObject testIndicator;

    // Start is called before the first frame update
    void Start()
    {
        //setup
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();

        agent.speed = baseMoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(agent.destination, player.transform.position) <= detectRange && isAttacking == false)
        {
            ChaseState();
            
        }

        //RotateTowards();
    }

    void ChaseState()
    {
        //updates position while outside of the attackrange
        if (Vector3.Distance(agent.destination, player.transform.position) > attackRange)
        {
            agent.destination = player.transform.position;
        }

        if (Vector3.Distance(agent.transform.position, player.transform.position) < attackRange)
        {
            //agent.destination = agent.transform.position;
            
            if (canAttack)
            {
                StartCoroutine(Attack(windupTime));
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
            canAttack = true;
        }
    }

    IEnumerator Attack(float windupTime)
    {
        Debug.Log("enemy attacks player");
        isAttacking = true;
        //stop movement briefly
        agent.speed = 0;

        //reset timers
        canAttack = false;
        attackTimer = 0;
        //run animation
         /*if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {

        }*/
        //gets player position
        agent.destination = player.transform.position;
        Instantiate(testIndicator, player.transform.position, Quaternion.identity);
        //when windup-animation finishes move enemy towards player location with dash speed
        yield return new WaitForSeconds(windupTime);
        agent.speed = dashSpeed;
        //TESTA yield return new WaitUntil(() => frame >= 10);
        //Reset speed to normal
        yield return new WaitForSeconds(dashTime);
        agent.speed = baseMoveSpeed;
        isAttacking = false;


    }

    void RotateTowards()
    {
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

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.forward);
    }
}
