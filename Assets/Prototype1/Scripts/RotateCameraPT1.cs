using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCameraPT1 : MonoBehaviour
{
    public float rotationSpeed;

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
    }
}
