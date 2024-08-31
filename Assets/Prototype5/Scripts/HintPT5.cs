using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintPT5 : MonoBehaviour
{
    public GameObject hintText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hintText.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
