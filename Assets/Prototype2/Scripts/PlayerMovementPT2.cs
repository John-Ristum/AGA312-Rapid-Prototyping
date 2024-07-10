using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementPT2 : Singleton<PlayerMovementPT2>
{
    public static event Action PlayerDead = null;

    public enum PlayerState { Walking, Climbing}
    public PlayerState state;

    public CharacterController controller;
    public float speed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public Transform startPoint;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;

    private GameObject tree;
    private bool nearTree;

    public bool canShoot = false;

    [HideInInspector]
    public bool isFacingRight = true;  // For determining which way the player is currently facing.

    float x;
    float y;

    //AudioSource audioSource;
    //public float stepRate = 0.5f;
    //float stepCooldown;

    private void Start()
    {
        state = PlayerState.Walking;
    }

    private void Update()
    {
        //Get the input from the player
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        //Checks if we are touching the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Gravity Stuff
        if (state == PlayerState.Walking)
        {
            if (isGrounded && velocity.y < 0)
                velocity.y = -2f;

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        //Move the player
        Vector3 move = transform.right * x;
        if (state == PlayerState.Walking)
            controller.Move(move * speed * Time.deltaTime);

        //Climbing
        Vector3 climb = transform.up * y;
        if (state == PlayerState.Climbing)
            controller.Move(climb * speed * Time.deltaTime);

        // If the input is moving the player right and the player is facing left...
        if (move.x > 0 && !isFacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (move.x < 0 && isFacingRight)
        {
            // ... flip the player.
            Flip();
        }

        //Does the jump stuff
        if (Input.GetButtonDown("Jump"))
        {
            if(isGrounded)
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if(state == PlayerState.Climbing)
            {
                state = PlayerState.Walking;
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }


        if (state == PlayerState.Walking && nearTree && Input.GetKeyDown(KeyCode.W))
        {
            state = PlayerState.Climbing;
            transform.position = new Vector3(tree.transform.position.x, transform.position.y, transform.position.z);
        }

        OneWayPlatform();

        //if (Input.GetKeyDown(KeyCode.R))
        //    canShoot = true;
    }

    void ResetPlayer()
    {
        transform.position = startPoint.transform.position;
        canShoot = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            tree = other.gameObject;
            nearTree = true;
        }

        if (other.CompareTag("EnemyProjectile"))
        {
            Destroy(other.gameObject);
            ResetPlayer();
            //PlayerDead();
            Debug.Log("ded");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            tree = null;
            nearTree = false;
            state = PlayerState.Walking;
        }
    }

    void OneWayPlatform()
    {
        //Create the ray
        Ray ray = new Ray(transform.position, transform.up * -1);
        //Create a refererance to hold the info on what we hit
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1f)) //Mathf.Infinity makes ray go forever
        {
            if (hit.collider.gameObject.CompareTag("OneWayPlat"))
                //Debug.Log("hit");
                hit.collider.isTrigger = false;
        }
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
}
