using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePT3 : MonoBehaviour
{
    public float lifeSpan = 5;      //Seconds until projectile is destroyed

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, lifeSpan);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
