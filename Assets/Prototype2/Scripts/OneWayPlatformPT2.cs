using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatformPT2 : MonoBehaviour
{
    public Collider platform;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            platform.isTrigger = true;
    }
}
