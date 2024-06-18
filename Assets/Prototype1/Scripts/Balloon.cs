using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    public float respawnTime = 5;
    MeshRenderer mesh;
    Collider boxCollider;

    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<Collider>();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetBalloon();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mesh.enabled = false;
            boxCollider.enabled = false;
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        ResetBalloon();
    }

    void ResetBalloon()
    {
        mesh.enabled = true;
        boxCollider.enabled = true;
    }

    private void OnEnable()
    {
        DeathZone.PlayerDead += ResetBalloon;
    }

    private void OnDisable()
    {
        DeathZone.PlayerDead -= ResetBalloon;
    }
}