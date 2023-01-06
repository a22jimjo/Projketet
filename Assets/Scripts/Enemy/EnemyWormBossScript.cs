using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWormBossScript : MonoBehaviour
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
    [SerializeField] GameObject head1;
    [SerializeField] private GameObject head2;

    private EnemyWormBossHeadScript head1Script;
    private EnemyWormBossHeadScript head2Script;

    [SerializeField] private bool isAttacking;
    [SerializeField] private bool hasDetectedPlayer = false;



    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        head1Script = head1.GetComponent<EnemyWormBossHeadScript>();
        head2Script = head2.GetComponent<EnemyWormBossHeadScript>();

        //sets up patrolstate waypoints
        GameObject go = GameObject.FindGameObjectWithTag("Waypoints");
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
            if (canAttack && !isAttacking)
            {
                StartCoroutine(Attack(waitBetweenAttacks, anticipationTime));

            }

            if (!isAttacking)
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
