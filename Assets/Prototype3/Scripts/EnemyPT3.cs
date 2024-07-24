using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPT3 : MonoBehaviour
{
    public enum EnemyState { Patrol, Stunned}
    public EnemyState state;
    public PlayerMovementPT3 player;
    public GameManagerPT3 gameManager;

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

    [Header("Ghost")]
    public bool isGhost;
    public Material defaultMaterial;
    public Material ghostMaterial;
    Renderer meshRenderer;
    Collider meshCollider;

    bool gameStarted;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        movedOnZ = Random.Range(0, 2);
        meshRenderer = GetComponent<Renderer>();
        meshCollider = GetComponent<Collider>();

        gameStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStarted)
            return;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);

        if (!isGrounded)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            rb.useGravity = true;
            rb.isKinematic = false;
        }
        else
            rb.useGravity = false;

        if (state == EnemyState.Patrol && !cantPatrol)
            Patrol();

        if (destPoint != null)
            transform.LookAt(new Vector3(destPoint.x, transform.position.y, destPoint.z));

        //if (state == EnemyState.Stunned && (rb.velocity.x + rb.velocity.y) <= 0 && isGrounded)
        //{
        //    agent.enabled = true;
        //    state = EnemyState.Patrol;
        //}
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

    public void SetGhost()
    {
        isGhost = true;
        meshRenderer.material = ghostMaterial;
        meshCollider.isTrigger = true;
    }

    public void ResetGhost()
    {
        isGhost = false;
        meshRenderer.material = defaultMaterial;
        meshCollider.isTrigger = false;
    }

    IEnumerator SetPatrol()
    {
        agent.enabled = true;
        state = EnemyState.Patrol;

        if (isGhost)
            ResetGhost();

        yield return new WaitForSeconds(0.5f);

        rb.isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall") && !isGhost)
        {
            StartCoroutine(SetPatrol());
        }

        if (other.CompareTag("DeathZone"))
        {
            gameManager.ShakeCamera();
            gameManager.IncreaseScore(1);

            transform.rotation = startPoint.transform.rotation;
            transform.position = startPoint.transform.position;

            StartCoroutine(SetPatrol());
        }
    }

    void GameStartedTrue()
    {
        gameStarted = true;
    }

    private void OnEnable()
    {
        GameManagerPT3.startGame += GameStartedTrue;
    }

    private void OnDisable()
    {
        GameManagerPT3.startGame -= GameStartedTrue;
    }
}
