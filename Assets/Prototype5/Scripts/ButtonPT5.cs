using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPT5 : MonoBehaviour
{
    bool buttonWasPressed;
    public Animator doorLeft;
    public Animator doorRight;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !buttonWasPressed)
        {
            Debug.Log("ButtonWasPressed");
            doorLeft.SetTrigger("Open");
            doorRight.SetTrigger("Open");
            buttonWasPressed = true;
        }
    }
}
