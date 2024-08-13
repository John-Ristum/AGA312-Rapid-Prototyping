using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementPT5 : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
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
    float xRaw;
    float yRaw;
    Vector3 targetRotation;
    public float rotationSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Get the input from the player
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        xRaw = Input.GetAxisRaw("Horizontal");
        yRaw = Input.GetAxisRaw("Vertical");

        //Checks if we are touching the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Gravity Stuff
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //Move the player
        Vector3 move = transform.right * x + transform.forward * y;
        controller.Move(move * speed * Time.deltaTime);

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
            if (isGrounded)
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
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
