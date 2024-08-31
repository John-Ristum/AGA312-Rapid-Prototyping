using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerPT5 : MonoBehaviour
{
    public GameObject player;
    public bool playerDead;

    Vector3 offset;

    void Start()
    {
        offset = transform.position - player.transform.position;
    }


    void LateUpdate()
    {
        if (!playerDead)
            transform.position = player.transform.position + offset;
    }

    public void ResetCam()
    {

    }
}
