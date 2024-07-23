using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePT3 : MonoBehaviour
{
    public enum ProjectileType { Player, Enemy }
    public ProjectileType type;

    public bool isGhost;

    public float lifeSpan = 5;      //Seconds until projectile is destroyed
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, lifeSpan);
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
            return;

        if(type == ProjectileType.Player)
        {
            if (other.CompareTag("Player"))
                return;

            if (other.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<EnemyPT3>().Stun();
                other.attachedRigidbody.isKinematic = false;

                if (isGhost)
                {
                    other.gameObject.GetComponent<EnemyPT3>().SetGhost();
                }

                ApplyKnockback(other.attachedRigidbody);
            }

            if (!other.CompareTag("PlayerProjectile"))
                if (!isGhost)
                    Destroy(this.gameObject);
        }

        if (type == ProjectileType.Enemy)
        {
            if (other.CompareTag("Enemy"))
                return;

            if (other.CompareTag("Player"))
            {
                //other.gameObject.GetComponent<PlayerMovementPT3>().KillPlayer();
                other.gameObject.GetComponent<PlayerMovementPT3>().Stun();
                ApplyKnockback(other.attachedRigidbody);
            }

            Destroy(this.gameObject);
        }
    }

    void ApplyKnockback(Rigidbody _opposingRB)
    {
        //Determine direction of knockback
        Vector3 knockbackDirection = _opposingRB.transform.position - transform.position;
        //Resets velocity to prevent knockback compounding
        _opposingRB.velocity = new Vector3(0, 0, 0);
        //Apply knockback
        _opposingRB.AddForce(rb.velocity.x * 100, 0, rb.velocity.z * 100, ForceMode.Force);
    }
}
