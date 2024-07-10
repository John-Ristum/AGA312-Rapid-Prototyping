using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPT2 : GameBehaviour
{
    public enum PatrolType { Patrol, Stunned, Detect, Attack }
    public PatrolType myPatrol;

    public GameObject projectilePrefab;
    public Transform[] points;
    int current;
    public float speed = 2f;
    public float timer;
    public float stunTime = 3f;
    public float detectTime = 1f;

    public bool isFacingRight;  // For determining which way the player is currently facing.

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (myPatrol)
        {
            case PatrolType.Patrol:
                Patrol();
                break;
            case PatrolType.Stunned:
                Stunned();
                break;
            case PatrolType.Detect:
                break;
            case PatrolType.Attack:
                break;
        } 
    }

    void Patrol()
    {
        CheckForPlayer();

        anim.SetTrigger("Walking");

        if (transform.position != points[current].position)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[current].position, speed * Time.deltaTime);
        }
        else
        {
            current = (current + 1) % points.Length;
            Flip();
        }
    }

    void Stunned()
    {
        anim.SetTrigger("Stunned");

        timer += Time.deltaTime;
        if (timer >= stunTime)
        {
            myPatrol = PatrolType.Patrol;
            timer = 0;
        }
    }

    IEnumerator DetectPlayer()
    {
        anim.SetTrigger("Detect");

        yield return new WaitForSeconds(detectTime);

        CheckForPlayer();
    }

    void CheckForPlayer()
    {
        float direction;
        if (isFacingRight)
            direction = 1;
        else
            direction = -1;

        //Create the ray
        Ray ray = new Ray(transform.position, transform.right * direction);
        //Create a refererance to hold the info on what we hit
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10f))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                switch (myPatrol)
                {
                    case PatrolType.Patrol:
                        Debug.Log("Aleart");
                        myPatrol = PatrolType.Detect;
                        StartCoroutine(DetectPlayer());
                        break;
                    case PatrolType.Detect:
                        anim.SetTrigger("Attack");
                        Instantiate(projectilePrefab, transform.position, transform.rotation);
                        myPatrol = PatrolType.Patrol;
                        break;
                }
            }
            else if (myPatrol == PatrolType.Detect)
                myPatrol = PatrolType.Patrol;
        }
        else if (myPatrol == PatrolType.Detect)
            myPatrol = PatrolType.Patrol;
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        isFacingRight = !isFacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerProjectile"))
        {
            //if(other.gameObject.GetComponent<ProjectilePT2>().isGrounded != true)
            myPatrol = PatrolType.Stunned;
            Destroy(other.gameObject);
        }
    }

    void ResetEnemy()
    {
        myPatrol = PatrolType.Patrol;
    }

    private void OnEnable()
    {
        DeathZonePT1.PlayerDead += ResetEnemy;
    }

    private void OnDisable()
    {
        DeathZonePT1.PlayerDead -= ResetEnemy;
    }
}
