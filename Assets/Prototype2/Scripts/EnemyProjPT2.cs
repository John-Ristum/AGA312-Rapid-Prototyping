using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjPT2 : GameBehaviour
{
    GameObject player;
    public string playerName;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find(playerName);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != player.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 50 * Time.deltaTime);
        }
    }
}
