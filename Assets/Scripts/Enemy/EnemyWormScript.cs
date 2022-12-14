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
    [SerializeField] bool canAttack = true;
    [SerializeField] bool isAttacking;
    [SerializeField] private float attackTimer;
    [SerializeField] private float attackCooldown;

    //Attacking
    [SerializeField] private float delayBeforeAttack;
    [SerializeField] private float AttackDuration;
    [SerializeField] private float delayAfterAttack;

    public Gameobject badring;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        dashLocation = GameObject.FindGameObjectWithTag("GhostDashLocation");
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
    }

    void Idle()
    {

    }

    IEnumerator Move(float waitTime, float waitTime2, float waitTime3)
    {
        //Play coming down animation

        //Remove badring
        badring.setactive(false);
        //Set new destination
        agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
        //calculate time it takes between points??
        //Spawn VFX that follows the enemy, showing the underground location as it travels

        //Reached destination? Play coming up animation, spawn in badring
        //remove follow vfx, spawn in coming up VFX.

    }

    IEnumerator Attack(float waitTime, float waitTime2, float waitTime3)
    {
        Debug.Log("enemy attacks player");
        isAttacking = true;


        //reset timers
        canAttack = false;
        attackTimer = 0;
    }
}
