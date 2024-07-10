using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringPointPT2 : MonoBehaviour
{
    [Header("Rigidbody Projectiles")]
    public GameObject projectilePrefab;     //The projectile we wish to instantiate
    public Rigidbody projectileRB;
    public float projectileSpeedH = 1000;    //The speed that our ptojectile fires at
    public float projectileSpeedV = 1000;    //The speed that our ptojectile fires at
    

    //[Header("Trajectory Line")]
    //public LineRenderer lineRenderer;
    //public Transform releasePosition;

    //[Header("Display Controls")]
    //[SerializeField]
    //[Range(10, 100)]
    //private int linePoints = 25;
    //[SerializeField]
    //[Range(0.01f, 0.25f)]
    //private float timeBetweenPoints = 0.1f;


    PlayerMovementPT2 player;

    private void Start()
    {
        player = GetComponentInParent<PlayerMovementPT2>();
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1") && player.canShoot)
            FireRigidbody();

        //DrawProjection();
    }

    void FireRigidbody()
    {
        float direction;
        if (player.isFacingRight)
            direction = 1;
        else
            direction = -1;

        //Create a reference to hold out instantiated object
        GameObject projectileInstance;
        //Insantiate our projectile prefab at the firing points position and rotation
        projectileInstance = Instantiate(projectilePrefab, transform.position, transform.rotation);
        //Get the rigidbody component of the projectile and add force to "fire" it
        projectileInstance.GetComponent<Rigidbody>().AddForce((transform.right * direction) * projectileSpeedH + transform.up * projectileSpeedV);

        player.canShoot = false;
    }

    //void DrawProjection()
    //{
    //    lineRenderer.enabled = true;
    //    lineRenderer.positionCount = Mathf.CeilToInt(linePoints / timeBetweenPoints) + 1;
    //    Vector3 startPosition = releasePosition.position;
    //    Vector3 startVelocity = projectileSpeedH * transform.right / projectileRB.mass;
    //    int i = 0;
    //    lineRenderer.SetPosition(i, startPosition);
    //    for (float time = 0; time < linePoints; time += timeBetweenPoints)
    //    {
    //        i++;
    //        Vector3 point = startPosition + time * startVelocity;
    //        point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);

    //        lineRenderer.SetPosition(i, point);
    //    }
    //}
}
