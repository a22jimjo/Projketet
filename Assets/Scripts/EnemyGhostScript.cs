using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGhostScript : MonoBehaviour
{

    List<Transform> wayPoints = new List<Transform>();
    UnityEngine.AI.NavMeshAgent agent;
    GameObject player;
    [Tooltip("The minimum distance between the character and it's target to attack")]
    [SerializeField] float attackRange;
    [Tooltip("The area where the enemy can detect the player")]
    [SerializeField] float detectRange;
    [Tooltip("Movement speed of the ghost")]
    [SerializeField] float baseMoveSpeed;

    [SerializeField] bool canAttack;
    private float attackTimer;
    private float attackCooldown;

    // Start is called before the first frame update
    void Start()
    {
        //setup
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        baseMoveSpeed = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(agent.destination, player.transform.position) <= detectRange)
        {
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

        if (Vector3.Distance(agent.transform.position, player.transform.position) < attackRange)
        {
            agent.destination = agent.transform.position;
            Debug.Log("enemy attacks player");
            if (canAttack)
            {
                //cattack
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

    IEnumerator Attack(float waitTime)
    {
        canAttack = false;
        attackTimer = 0;
        //run animation

        //Wait until reseting attack
        yield return new WaitForSeconds(waitTime);
        canAttack = true;

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.forward);
    }
}
