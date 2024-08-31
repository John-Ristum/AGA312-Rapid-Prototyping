using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZonePT5 : MonoBehaviour
{
    public GameObject winPanel;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovementPT5>().canMove = false;
            winPanel.SetActive(true);
        }
    }
}
