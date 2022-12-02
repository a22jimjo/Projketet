using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.AI;
using UnityEngine.UI;

public class SimpleEnemy : MonoBehaviour
{

    List<Transform> wayPoints = new List<Transform> ();
    NavMeshAgent agent;
    GameObject player;


    [Header("Patrol Settings")]
    [Tooltip("Activates the characters patrolstate")]
    public bool patrolState;
    [Tooltip("Amount of time the character waits at each waypoint")]
    public float waitTime;

    private float timer;


    [Header("Chase Settings")]
    [Tooltip("Activates the characters chasestate")]
    public bool chaseState;
    [Tooltip("The minimum distance between the character and it's target to attack")]
    public float attackRange;
    [Tooltip("The area where the enemy can detect the player")]
    public float detectRange;
    public float escapeRange;
    private float baseMoveSpeed;



    [Header("Attack Settings")]
    public GameObject projectile;
    public Transform firePoint;
    public float projectileSpeed;
    public float projectileCooldown;
    public float projectileTimer;
    public float castTime;
    public bool cancast;

    // Start is called before the first frame update
    void Start()
    {
        //setup
        agent = GetComponent<NavMeshAgent> ();
        player = GameObject.FindGameObjectWithTag("Player");
        baseMoveSpeed = agent.speed;


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

        //FaceTarget();

        if ((Vector3.Distance(agent.destination, player.transform.position) <= detectRange) && Vector3.Distance(agent.destination, player.transform.position) <= escapeRange || chaseState)
        {
            ChaseState();
        }
        else
        {
            PatrolState();
        }
    }

    void PatrolState()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            timer += Time.deltaTime;
            if (timer > waitTime)
            {
                agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
            }
        }
        else timer = 0;
    }

    void ChaseState()
    {
        //updates position while outside of the attackrange
        if(Vector3.Distance(agent.destination, player.transform.position) > attackRange)
        {
            agent.destination = player.transform.position;
        }

        if(Vector3.Distance(agent.transform.position, player.transform.position) < attackRange)
        {
            agent.destination = agent.transform.position;
            Debug.Log("enemy attacks player");
            if (cancast)
            {
                //cast ability
                StartCoroutine(Attack(castTime));
            }
        }

        //Timer
        if (projectileTimer < projectileCooldown)
        {
            projectileTimer += Time.deltaTime;
        }
        else
        {
            projectileCooldown = projectileTimer;
            cancast = true;
        }
    }

    IEnumerator Attack(float waitTime)
    {
        Debug.Log("corutine running");
        //reset cancast & timer
        cancast = false;
        projectileTimer = 0;

        //stops movement
        agent.speed = 0;
        //run casting animation
        Debug.Log("TELEGRAPHING");
        yield return new WaitForSeconds(waitTime);

        //instantiate projectile
        Debug.Log("Shoot!");
        GameObject _projectile = Instantiate(projectile, firePoint.position, transform.rotation);
        _projectile.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);

        //reset movementspeed
        agent.speed = baseMoveSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, escapeRange);

    }

    void FaceTarget()
    {
        var turnTowardNavSteeringTarget = agent.steeringTarget;

        Vector3 direction = (turnTowardNavSteeringTarget - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }
}
