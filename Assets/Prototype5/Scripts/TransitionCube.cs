using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionCube : MonoBehaviour
{
    public GameObject oldSection;
    public GameObject newSection;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            newSection.SetActive(true);
            oldSection.SetActive(false);
        }
    }
}
