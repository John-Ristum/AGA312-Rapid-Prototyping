using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleCollectPT2 : MonoBehaviour
{
    public float respawnTime = 5;
    SpriteRenderer mesh;
    Collider boxCollider;

    private void Start()
    {
        mesh = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<Collider>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovementPT2>().canShoot = true;
            mesh.enabled = false;
            boxCollider.enabled = false;
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        ResetApple();
    }

    void ResetApple()
    {
        mesh.enabled = true;
        boxCollider.enabled = true;
    }

    private void OnEnable()
    {
        DeathZonePT1.PlayerDead += ResetApple;
    }

    private void OnDisable()
    {
        DeathZonePT1.PlayerDead -= ResetApple;
    }
}
