using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPT3 : MonoBehaviour
{
    public enum PowerUpType { Rapidfire, Shotgun, Ghost }
    public PowerUpType type;
    public GameManagerPT3 gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (type)
            {
                case PowerUpType.Rapidfire:
                    gameManager.powerUp = GameManagerPT3.PowerUpState.Rapidfire;
                    Destroy(this.gameObject);
                    return;
                case PowerUpType.Shotgun:
                    gameManager.powerUp = GameManagerPT3.PowerUpState.Shotgun;
                    Destroy(this.gameObject);
                    return;
                case PowerUpType.Ghost:
                    gameManager.powerUp = GameManagerPT3.PowerUpState.Ghost;
                    Destroy(this.gameObject);
                    return;
            }
        }
    }
}
