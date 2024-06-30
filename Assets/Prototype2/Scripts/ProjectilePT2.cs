using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePT2 : MonoBehaviour
{
    public int damage = 20;
    public float lifeSpan = 5;      //Seconds until projectile is destroyed

    void Start()
    {
        Destroy(this.gameObject, lifeSpan);
        if (GetComponent<AudioSource>() != null)
            GetComponent<AudioSource>().pitch = Random.Range(0.7f, 1.3f);
    }
}
