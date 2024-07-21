using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovementPT3 : MonoBehaviour
{
    CharacterController controller;
    [Header("Player Attributes")]
    public float playerSpeed = 5;
    public Transform startPoint;
    public int lives = 3;
    public int score;

    [Header("Gravity")]
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    LayerMask blankMask;
    private Vector3 velocity;
    private bool isGrounded;
    public float fallTimer;
    bool canMove;

    [Header("Rigidbody Projectiles")]
    public GameObject projectilePrefab;     //The projectile we wish to instantiate
    public float projectileSpeed = 1000;    //The speed that our ptojectile fires at
    public Transform firingPoint;

    [Header("UI")]
    public TMP_Text livesText;
    public TMP_Text scoreText;
    public TMP_Text finalScoreText;
    public GameObject inGamePanel;
    public GameObject gameOverPanel;
    public PauseController pauseController;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

        score = 0;
        livesText.text = "Lives: " + lives;
        scoreText.text = "Score: " + score;
        //Debug.Log("Lives: " + lives);
    }

    // Update is called once per frame
    void Update()
    {
        //Turn Player
        if (Input.GetKeyDown(KeyCode.D))
        {
            Turn(90);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Turn(-90);
        }
    }

    private void FixedUpdate()
    {
        //Checks if we are touching the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Gravity Stuff
        if (isGrounded && velocity.y < 0)
        {
            fallTimer = 0.25f;
            canMove = true;
            velocity.y = -2f;
        }
        else
            fallTimer -= Time.deltaTime;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (fallTimer <= 0)
            canMove = false;

        if(canMove)
            controller.Move(transform.forward * playerSpeed * Time.deltaTime);
    }

    void Turn(float _rotation)
    {
        if (!isGrounded)
            return;

        transform.Rotate(new Vector3(0, _rotation, 0));
        FireRigidbody();
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

    public void IncreaseScore(int _points)
    {
        score += _points;
        scoreText.text = "Score: " + score;
        //Debug.Log("Score: " + score);
    }

    void EndGame()
    {
        finalScoreText.text = "Score: " + score;
        inGamePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        pauseController.gameEnded = true;
        Time.timeScale = 0;
    }

    public void KillPlayer()
    {
        lives -= 1;
        livesText.text = "Lives: " + lives;

        if (lives <= 0)
            EndGame();

        //if (lives >= 1)
        //    Debug.Log("Lives: " + lives);
        //else
        //    Debug.Log("Game Over");

        velocity.y = -2f;
        fallTimer = 0.25f;
        //controller.excludeLayers = blankMask;
        transform.rotation = startPoint.transform.rotation;
        transform.position = startPoint.transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Enemy"))
        //    Debug.Log("ded");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathZone"))
        {
            KillPlayer();
        }
    }
}
