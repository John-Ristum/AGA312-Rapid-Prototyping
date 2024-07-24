using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManagerPT3 : MonoBehaviour
{
    public GameObject[] powerUps;
    public List<GameObject> powerUpRotation;
    public GameObject currentPowerUp;
    public GameManagerPT3 gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    SpawnPowerUp();
        //}
    }

    void AddPowerUpsToList()
    {
        if (powerUpRotation.Count == 0)
        {
            powerUpRotation = new List<GameObject>(powerUps);
        }
    }

    public void SpawnPowerUp()
    {
        if (currentPowerUp != null)
            Destroy(currentPowerUp);

        AddPowerUpsToList();

        int i = 0;

        if (powerUpRotation.Count > 0)
            i = Random.Range(0, powerUpRotation.Count);

        currentPowerUp = Instantiate(powerUpRotation[i], transform.position, transform.rotation);
        currentPowerUp.GetComponent<PowerUpPT3>().gameManager = gameManager;

        powerUpRotation.Remove(powerUpRotation[i]);
    }
}
