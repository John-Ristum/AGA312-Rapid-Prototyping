using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePT2 : MonoBehaviour
{
    public float lifeSpan = 5;      //Seconds until projectile is destroyed
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool isGrounded;

    void Start()
    {
        Destroy(this.gameObject, lifeSpan);
        if (GetComponent<AudioSource>() != null)
            GetComponent<AudioSource>().pitch = Random.Range(0.7f, 1.3f);
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }
}
