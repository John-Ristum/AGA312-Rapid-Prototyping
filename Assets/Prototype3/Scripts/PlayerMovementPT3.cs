using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovementPT3 : MonoBehaviour
{
    public enum PlayerType { P1, P2 }
    public PlayerType type;
    public enum PlayerState { Moving, Turning, Stunned }
    public PlayerState state;

    CharacterController controller;
    Rigidbody rb;
    [Header("Player Attributes")]
    public float playerSpeed = 5;
    public float rotationSpeed = 40;
    public Transform startPoint;
    public GameManagerPT3 gameManager;
    //public int lives = 3;
    //public int score;

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
    public GameObject projectileNormal;     //The projectile we wish to instantiate
    public GameObject projectileGhost;
    public float projectileSpeed = 1000;    //The speed that our ptojectile fires at
    public Transform[] firingPoints;
    float rapidFireTimer = 0f;

    [Header("UI")]
    public TMP_Text livesText;
    public TMP_Text scoreText;
    public TMP_Text finalScoreText;
    public GameObject inGamePanel;
    public GameObject gameOverPanel;
    public PauseController pauseController;

    float x;
    float y;

    bool gameStarted;

    // Start is called before the first frame update
    void Start()
    {
        //controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();

        gameStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStarted)
            return;

        //Get the input from the player
        if (type == PlayerType.P1)
            Player1Inputs();
        else
            Player2Inputs();

        Vector3 move = transform.right * x;

        //Turn Player
        if (x != 0)
            Turn();
        else if (state != PlayerState.Stunned)
            state = PlayerState.Moving;


        if (gameManager.powerUp == GameManagerPT3.PowerUpState.Rapidfire)
            RapidFireProjectile();

        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    Turn(90);
        //}
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    Turn(-90);
        //}
    }

    private void FixedUpdate()
    {
        if (!gameStarted)
            return;

        //Checks if we are touching the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Gravity Stuff
        if (isGrounded) //&& velocity.y < 0
        {
            fallTimer = 0.05f;
            canMove = true;
            rb.useGravity = false;
            //velocity.y = -2f;
        }
        else
            fallTimer -= Time.deltaTime;

        velocity.y += gravity * Time.deltaTime;
        //controller.Move(velocity * Time.deltaTime);

        if (fallTimer <= 0)
        {
            canMove = false;
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            rb.useGravity = true;
        }

        if (canMove && state == PlayerState.Moving)
            rb.AddForce(transform.forward * playerSpeed * Time.deltaTime, ForceMode.Force);
        //controller.Move(transform.forward * playerSpeed * Time.deltaTime);
    }

    void Player1Inputs()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
            if (state != PlayerState.Stunned || gameManager.powerUp != GameManagerPT3.PowerUpState.Rapidfire)
                FireProjectile();
    }

    void Player2Inputs()
    {
        x = Input.GetAxisRaw("HorizontalP2");
        y = Input.GetAxisRaw("VerticalP2");

        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
            if (state != PlayerState.Stunned || gameManager.powerUp != GameManagerPT3.PowerUpState.Rapidfire)
                FireProjectile();
    }

    void Turn(float _rotation = 90)
    {
        if (!isGrounded || state == PlayerState.Stunned)
            return;

        rb.velocity = new Vector3(0, 0, 0);
        state = PlayerState.Turning;
        transform.Rotate(transform.up * rotationSpeed * x * Time.deltaTime);
        //transform.Rotate(new Vector3(0, _rotation, 0) * Time.deltaTime * 1);
        //FireRigidbody();
    }

    void FireProjectile()
    {
        switch (gameManager.powerUp)
        {
            case GameManagerPT3.PowerUpState.Normal:
                FireRigidbody(projectileNormal, firingPoints[0]);
                return;
            case GameManagerPT3.PowerUpState.Shotgun:
                FireRigidbody(projectileNormal, firingPoints[0]);
                FireRigidbody(projectileNormal, firingPoints[1]);
                FireRigidbody(projectileNormal, firingPoints[2]);
                return;
            case GameManagerPT3.PowerUpState.Ghost:
                FireRigidbody(projectileGhost, firingPoints[0]);
                return;
        }
    }

    void RapidFireProjectile()
    {
        if (!canMove)
            return;

        if (rapidFireTimer <= 0)
        {
            FireRigidbody(projectileNormal, firingPoints[0]);
            rapidFireTimer = 0.25f;
        }

        rapidFireTimer -= Time.deltaTime;
    }

    void FireRigidbody(GameObject _projectile, Transform _firingPoint)
    {
        //Create a reference to hold out instantiated object
        GameObject projectileInstance;
        //Insantiate our projectile prefab at the firing points position and rotation
        projectileInstance = Instantiate(_projectile, _firingPoint.position, _firingPoint.rotation);
        //Get the rigidbody component of the projectile and add force to "fire" it
        projectileInstance.GetComponent<Rigidbody>().AddForce(_firingPoint.transform.forward * projectileSpeed);
    }

    public void Stun()
    {
        if (state == PlayerState.Stunned)
            return;

        state = PlayerState.Stunned;
    }

    //public void IncreaseScore(int _points)
    //{
    //    score += _points;
    //    scoreText.text = "Score: " + score;
    //    //Debug.Log("Score: " + score);
    //}

    //void EndGame()
    //{
    //    finalScoreText.text = "Score: " + score;
    //    inGamePanel.SetActive(false);
    //    gameOverPanel.SetActive(true);
    //    pauseController.gameEnded = true;
    //    Time.timeScale = 0;
    //}

    public void KillPlayer()
    {
        gameManager.lives -= 1;
        gameManager.livesText.text = "Lives: " + gameManager.lives;

        if (gameManager.lives <= 0)
            gameManager.EndGame();

        //if (lives >= 1)
        //    Debug.Log("Lives: " + lives);
        //else
        //    Debug.Log("Game Over");

        velocity.y = -2f;
        fallTimer = 0.25f;
        //controller.excludeLayers = blankMask;
        //powerUp = PowerUpState.Normal;
        transform.rotation = startPoint.transform.rotation;
        transform.position = startPoint.transform.position;
        state = PlayerState.Moving;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Enemy"))
        //    Debug.Log("ded");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            state = PlayerState.Moving;
        }

        if (other.CompareTag("DeathZone"))
        {
            KillPlayer();
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
