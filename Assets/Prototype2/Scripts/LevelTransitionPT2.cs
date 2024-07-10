using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransitionPT2 : MonoBehaviour
{
    public Transform nextStartPoint;
    public Camera oldCam;
    public Camera nextCam;
    public GameObject boundary;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.transform.position = nextStartPoint.position;
            other.gameObject.GetComponent<PlayerMovementPT2>().startPoint = nextStartPoint;
            nextCam.gameObject.SetActive(true);
            oldCam.gameObject.SetActive(false);
            if (boundary != null)
                boundary.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
