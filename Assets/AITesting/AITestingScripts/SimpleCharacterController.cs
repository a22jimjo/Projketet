using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCharacterController : MonoBehaviour
{
    [Header("Character Settings")]
    public float BaseMoveSpeed = -2;
    public float currentMoveSpeed;
    public float speedMultiplier = 1.25f;
    private CharacterController controller;


    [Header("Projectile Settings")]
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
        controller = GetComponent<CharacterController>();
        currentMoveSpeed = BaseMoveSpeed;

    }

    // Update is called once per frame
    void Update()
    {  
        Movement();
        Shoot();
    }


    void Movement()
    {

        //Movement
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move.Normalize();
        controller.Move(move * Time.deltaTime * (currentMoveSpeed * speedMultiplier));
        
        
        //Rotations
        // Cast a ray from screen point
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Save the info
        RaycastHit hit;
        // You successfully hit
        if (Physics.Raycast(ray, out hit))
        {
            // Rotates
            transform.LookAt(hit.point);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            currentMoveSpeed *= speedMultiplier;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentMoveSpeed = BaseMoveSpeed;
        }
    }

    void Shoot()
    {
        
        if (projectileTimer < projectileCooldown)
        {
            projectileTimer += Time.deltaTime;
        }
        else
        {
            projectileCooldown = projectileTimer;
            cancast = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && cancast)
        {
            StartCoroutine (ShootProjectile(castTime));
        }
    }

    IEnumerator ShootProjectile(float waitTime)
    {
        //reset cancast & timer
        cancast = false;
        projectileTimer = 0;

        //stops movement
        currentMoveSpeed = 0f;

        //run casting animation
        yield return new WaitForSeconds(waitTime);

        //instantiate projectile
        Debug.Log("Shoot!");
        GameObject _projectile = Instantiate(projectile, firePoint.position, transform.rotation);
        _projectile.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);

        //reset movementspeed
        currentMoveSpeed = BaseMoveSpeed * speedMultiplier;

    }
}
