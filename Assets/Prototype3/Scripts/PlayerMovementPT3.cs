using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementPT3 : MonoBehaviour
{
    CharacterController controller;
    [Header("Player Attributes")]
    public float playerSpeed = 5;

    [Header("Rigidbody Projectiles")]
    public GameObject projectilePrefab;     //The projectile we wish to instantiate
    public float projectileSpeed = 1000;    //The speed that our ptojectile fires at
    public Transform firingPoint;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
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
        controller.Move(transform.forward * playerSpeed * Time.deltaTime);
    }

    void Turn(float _rotation)
    {
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
}
