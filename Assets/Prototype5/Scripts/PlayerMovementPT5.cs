using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementPT5 : MonoBehaviour
{
    public enum PlayerState { Normal, Dashing }
    public PlayerState state;

    public CharacterController controller;
    float speed;
    public float walkSpeed = 6f;
    public float dashSpeed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    bool jumping;
    public Transform startPoint;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;

    [HideInInspector]
    public bool isFacingRight = true;  // For determining which way the player is currently facing.

    float x;
    float y;
    float yOffset;
    //float xRaw;
    //float yRaw;
    Vector3 targetRotation;
    public float rotationSpeed = 5;
    bool falling;
    bool canJump;

    // Start is called before the first frame update
    void Start()
    {
        state = PlayerState.Normal;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Player Speed
        //switch (state)
        //{
        //    case PlayerState.Normal:
        //        speed = walkSpeed;
        //        break;
        //    case PlayerState.Dashing:
        //        speed = dashSpeed;
        //        break;
        //}

        //Get the input from the player
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        //xRaw = Input.GetAxisRaw("Horizontal");
        //yRaw = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
            state = PlayerState.Dashing;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            state = PlayerState.Normal;

        //Checks if we are touching the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Gravity Stuff
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            canJump = true;
            falling = false;
        }

        switch (state)
        {
            case PlayerState.Normal:
                velocity.y += gravity * Time.deltaTime;
                controller.Move(velocity * Time.deltaTime);
                break;
            case PlayerState.Dashing:
                if (x == 0 || falling)
                {
                    velocity.y += gravity * Time.deltaTime;
                    controller.Move(velocity * Time.deltaTime);
                }
                break;
        }

        

        //if (x == 0 && y == 0 || falling)
        //{
        //    velocity.y += gravity * Time.deltaTime;
        //    controller.Move(velocity * Time.deltaTime);
        //}

        if (velocity.y < -2.1)
            falling = true;
        //Debug.Log("falling");

        //Move the player
        Vector3 move = transform.right * x + transform.forward * y;
        if (x == 0)
            yOffset = 1;
        else
            yOffset = 0.75f;

        switch (state)
        {
            case PlayerState.Normal:
                controller.Move(move.normalized * walkSpeed * Time.deltaTime);
                break;
            case PlayerState.Dashing:
                controller.Move((transform.right * x) * dashSpeed * Time.deltaTime);
                controller.Move((transform.forward * y) * (walkSpeed * yOffset) * Time.deltaTime);
                break;
        }
        

        

        //Vector3 move = new Vector3(x, 0, y);
        //Vector3 moveRaw = new Vector3(xRaw, 0, yRaw);
        //if (move.sqrMagnitude > 1f)
        //    move.Normalize();
        //if (moveRaw.sqrMagnitude > 1f)
        //    moveRaw.Normalize();

        //if (moveRaw != Vector3.zero)
        //    targetRotation = Quaternion.LookRotation(move).eulerAngles;

        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRotation.x, Mathf.Round(targetRotation.y / 45) * 45, targetRotation.z), Time.deltaTime * rotationSpeed);
        //controller.Move(moveRaw * speed * Time.deltaTime);

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
            Jump();
        }
        if (Input.GetButtonUp("Jump") && velocity.y > -0.2)
        {
            velocity.y /= 2;
        }
    }

    void Jump()
    {
        if (canJump)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            canJump = false;
        }
        falling = true;
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        isFacingRight = !isFacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        falling = true;
    }
}
