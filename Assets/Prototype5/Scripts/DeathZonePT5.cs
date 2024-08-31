using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZonePT5 : MonoBehaviour
{
    public float moveTweenTime = 1f;
    public float shakeStrength = 0.4f;
    public Transform respawnPoint;
    public CameraControllerPT5 cam;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Respawn(other.gameObject));
        }
    }

    IEnumerator Respawn(GameObject _player)
    {
        cam.playerDead = true;
        _player.GetComponent<PlayerMovementPT5>().canMove = false;
        Camera.main.DOShakePosition(moveTweenTime / 2, shakeStrength);

        yield return new WaitForSeconds(1);

        _player.transform.position = respawnPoint.position;
        _player.GetComponent<PlayerMovementPT5>().canMove = true;
        cam.playerDead = false;
    }
}
