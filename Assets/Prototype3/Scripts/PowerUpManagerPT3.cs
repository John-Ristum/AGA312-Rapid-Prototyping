using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManagerPT3 : MonoBehaviour
{
    public GameObject[] powerUps;
    public GameObject currentPowerUp;
    public GameManagerPT3 gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SpawnPowerUp();
    }

    public void SpawnPowerUp()
    {
        if (currentPowerUp != null)
            Destroy(currentPowerUp);

        currentPowerUp = Instantiate(powerUps[Random.Range(0, 3)], transform.position, transform.rotation);
        currentPowerUp.GetComponent<PowerUpPT3>().gameManager = gameManager;
    }
}
