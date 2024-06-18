using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class PlayerController : GameBehaviour
{
    AudioSource audioSource;
    public AudioClip boostChargeSound;
    public AudioClip boostSound;

    private GameObject focalPoint;
    private Rigidbody playerRb;
    private TrailRenderer boostTrail;
    public GameObject boostBar;
    BoostBar boostBarScript;
    private Color transparent;
    public Material defaultMaterial;
    public Material balloonMaterial;
    Renderer meshRenderer;
    public float speed = 5f;
    public float groundSpeed = 5f;
    public float airSpeed = 2.5f;
    public float groundDrag = 1.5f;
    public float airDrag = 3f;
    public float boostTime;
    public float boostPower = 10f;
    public float groundBoostPower = 10f;
    public float airBoostPower = 5f;
    private bool boosting;
    private bool canBoost = true;
    Vector3 boostDirection = new Vector3(1, 0, 0);
    public bool hasPowerup;
    public float floatForce = 1f;
    public float floatTime = 5f;
    float balloonTimer;
    private float powerupStrength = 15f;
    //public GameObject powerupIndicator;
    //public TMP_Text boostIndicator;

    bool isGrounded;
    bool isAirborne;    //Used to determinbe if speed and drag variables need to be altered
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Transform startPoint;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<Renderer>();
        startPoint = GameObject.Find("StartPoint").transform;
        groundCheck = GameObject.Find("PlayerGroundCheck").transform;

        playerRb.maxAngularVelocity = float.PositiveInfinity;
        boostTrail = GetComponent<TrailRenderer>();
        boostTrail.time = 0;
        boostBarScript = boostBar.GetComponent<BoostBar>();
        boostBar.SetActive(false);

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPlayer();
        }

        groundCheck.transform.position = transform.position + new Vector3(0, -0.25f, 0);
        boostBar.transform.position = transform.position + new Vector3(0, 1f, -1.5f);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isAirborne = !Physics.CheckSphere(transform.position, 0.7f, groundMask);

        if (isGrounded)
        {
            canBoost = true;
            boostPower = groundBoostPower;
        }
        else
            boostPower = airBoostPower;


        if (isAirborne)
        {
            speed = airSpeed;
            playerRb.drag = airDrag;
        }
        else
        {
            if (!boosting)
                speed = groundSpeed;
            else
                speed = airSpeed;

            playerRb.drag = groundDrag;
        }
            

        if (Input.GetKeyDown(KeyCode.D))
            boostDirection = new Vector3(1, 0, 0);
        else if (Input.GetKeyDown(KeyCode.A))
            boostDirection = new Vector3(-1, 0, 0);

        float horizintalInput = Input.GetAxis("Horizontal");
        playerRb.AddForce(Vector3.right * speed * horizintalInput * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && !hasPowerup)
        {
            if (canBoost)
            {
                boosting = true;
                _AM.PlaySound(boostChargeSound, audioSource);
            }
            else if (!hasPowerup)
            {
                //playerRb.velocity = Vector3.zero;
                //playerRb.AddForce(Vector3.down * 50, ForceMode.Impulse);
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && !hasPowerup)
        {
            if (canBoost)
                _AM.PlaySound(boostSound, audioSource);

            canBoost = false;
            boosting = false;
            playerRb.AddForce(boostDirection * boostPower * boostTime, ForceMode.Impulse);
            boostTime = 0;
            boostTrail.time = 1;
        }

        Boost();

        if (hasPowerup)
        {
            boostTime = 0;
            canBoost = false;
            boosting = false;
            playerRb.AddForce(Vector3.up * floatForce * Time.deltaTime);
            balloonTimer += Time.deltaTime;

            if (balloonTimer >= floatTime)
                PowerupCountdownRoutine();
        }

        //powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);

        if (transform.position.y < -10)
            transform.position = new Vector3(0, 0, 0);
    }

    void ResetPlayer()
    {
        playerRb.useGravity = true;
        hasPowerup = false;
        playerRb.velocity = Vector3.zero;
        canBoost = true;
        transform.position = startPoint.transform.position;
        boostTrail.time = 0;
    }

    void Boost()
    {
        if (boosting && !hasPowerup)
        {
            boostTime += Time.deltaTime * 5;
            boostBar.SetActive(true);
        }
        else
            boostBar.SetActive(false);

        if (boostTime > 5)
            boostTime = 5;

        boostBarScript.UpdateBoostBar(boostTime);
        //boostIndicator.text = boostTime.ToString("F2");

        if (boostTrail.time > 0)
            boostTrail.time -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            meshRenderer.material = balloonMaterial;
            //ChangeMaterialAlpha(50);

            balloonTimer = 0;
            //boostPower = 0;
            playerRb.velocity = Vector3.zero;
            hasPowerup = true;
            //Destroy(other.gameObject);
            playerRb.useGravity = false;
            //powerupIndicator.gameObject.SetActive(true);
            //StartCoroutine(PowerupCountdownRoutine());
        }
    }

    void PowerupCountdownRoutine()
    {
        //yield return new WaitForSeconds(floatTime);
        hasPowerup = false;
        playerRb.useGravity = true;
        meshRenderer.material = defaultMaterial;
        //ChangeMaterialAlpha(100);
        //powerupIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        speed = groundSpeed;
        playerRb.drag = groundDrag;

        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            Debug.Log("Collided with " + collision.gameObject.name + " with powerup set to " + hasPowerup);
            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
    }

    void ChangeMaterialAlpha(float _alpha = 0)
    {
        Material material = GetComponent<MeshRenderer>().material;
        Color colour = material.color;
        colour.a = _alpha;
        material.color = colour;
    }

    private void OnEnable()
    {
        DeathZone.PlayerDead += ResetPlayer;
    }

    private void OnDisable()
    {
        DeathZone.PlayerDead -= ResetPlayer;
    }
}
