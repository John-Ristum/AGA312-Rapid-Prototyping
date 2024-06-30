using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementPT2 : Singleton<PlayerMovementPT2>
{
    public CharacterController controller;
    public float speed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float health = 100;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;

    [HideInInspector]
    public bool isFacingRight = true;  // For determining which way the player is currently facing.

    //AudioSource audioSource;
    //public float stepRate = 0.5f;
    //float stepCooldown;

    private void Start()
    {
        //audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //Checks if we are touching the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        //Get the input from the player
        float x = Input.GetAxis("Horizontal");

        //Move the player
        Vector3 move = transform.right * x;
        controller.Move(move * speed * Time.deltaTime);

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
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //Footstep Audio Stuff
        //stepCooldown -= Time.deltaTime;
        //if(stepCooldown < 0 && isGrounded && (move.x != 0 || move.z != 0))
        //{
        //    stepCooldown = stepRate;
        //    _AM.PlaySound(_AM.GetFootstepSound(), audioSource);
        //}

    }

    public void Hit(int damage)
    {
        health -= damage;
        print("Player health: " + health);
        if (health <= 0)
        {
            //_GM.gameState = GameState.GameOver;
            //_AM.PlaySound(_AM.GetEnemyDieSound(), audioSource);
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
