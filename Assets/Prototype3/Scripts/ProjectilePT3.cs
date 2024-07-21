using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePT3 : MonoBehaviour
{
    public enum ProjectileType { Player, Enemy }
    public ProjectileType type;

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
                ApplyKnockbackEnemy(other.attachedRigidbody);
            }
        }

        if (type == ProjectileType.Enemy)
        {
            if (other.CompareTag("Enemy"))
                return;

            if (other.CompareTag("Player"))
            {
                other.gameObject.GetComponent<PlayerMovementPT3>().KillPlayer();
            }
        }

        Destroy(this.gameObject);
    }

    void ApplyKnockbackEnemy(Rigidbody _opposingRB)
    {
        //Determine direction of knockback
        Vector3 knockbackDirection = _opposingRB.transform.position - transform.position;
        //Resets velocity to prevent knockback compounding
        _opposingRB.velocity = new Vector3(0, 0, 0);
        //Apply knockback
        _opposingRB.AddForce(rb.velocity.x * 100, 0, rb.velocity.z * 100, ForceMode.Force);
    }
}
