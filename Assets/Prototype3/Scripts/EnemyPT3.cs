using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPT3 : MonoBehaviour
{
    public enum EnemyState { Patrol, Stunned}
    public EnemyState state;
    public PlayerMovementPT3 player;

    Rigidbody rb;
    NavMeshAgent agent;

    [SerializeField] LayerMask groundLayer;
    public Transform startPoint;

    //patrol
    Vector3 destPoint;
    bool walkpointSet;
    [SerializeField] float range;
    int movedOnZ;
    public bool cantPatrol;

    //gravity
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    bool isGrounded;

    [Header("Rigidbody Projectiles")]
    public GameObject projectilePrefab;     //The projectile we wish to instantiate
    public float projectileSpeed = 1000;    //The speed that our ptojectile fires at
    public Transform firingPoint;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        movedOnZ = Random.Range(0, 2);
        //Debug.Log(movedOnZ);
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);

        if (!isGrounded)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            rb.useGravity = true;
        }
        else
            rb.useGravity = false;

        if (state == EnemyState.Patrol && !cantPatrol)
            Patrol();

        if (destPoint != null)
            transform.LookAt(new Vector3(destPoint.x, transform.position.y, destPoint.z));
    }

    void Patrol()
    {
        rb.useGravity = false;

        if (!walkpointSet) 
            SearchForDest();
        if (walkpointSet) 
            agent.SetDestination(destPoint);
        if (Vector3.Distance(transform.position, destPoint) < 1) 
            walkpointSet = false;
    }

    void SearchForDest()
    {
        float z = Random.Range(-range, range);
        float x = Random.Range(-range, range);

        destPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        if (Physics.Raycast(destPoint, Vector3.down, groundLayer))
        {
            walkpointSet = true;
            FireRigidbody();
        }
    }

    void FireRigidbody()
    {
        //Create a reference to hold out instantiated object
        GameObject projectileInstance;
        //Insantiate our projectile prefab at the firing points position and rotation
        projectileInstance = Instantiate(projectilePrefab, firingPoint.position, firingPoint.rotation);
        //Get the rigidbody component of the projectile and add force to "fire" it
        projectileInstance.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed);
    }

    public void Stun()
    {
        if (state == EnemyState.Stunned)
            return;

        state = EnemyState.Stunned;
        agent.ResetPath();
        agent.enabled = false;
        walkpointSet = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            agent.enabled = true;
            state = EnemyState.Patrol;
        }

        if (other.CompareTag("DeathZone"))
        {
            player.IncreaseScore(1);

            transform.rotation = startPoint.transform.rotation;
            transform.position = startPoint.transform.position;

            agent.enabled = true;
            state = EnemyState.Patrol;
        }
    }
}
