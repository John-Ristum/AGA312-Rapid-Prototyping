using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playground : GameBehaviour
{
    public GameObject player;

    void Start()
    {
        ExecuteAfterSeconds(2f, () =>
        {
            player.transform.localScale = Vector3.one * 2;
        });
        print("Game Started");

        ExecuteAfterFrames(1, () =>
        {
            print("one frame later");
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.GetComponent<Renderer>().material.color = ColorX.GetRandomColour();
        }
    }
}
