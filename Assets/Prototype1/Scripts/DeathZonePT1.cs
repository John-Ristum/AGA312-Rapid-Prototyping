using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZonePT1 : GameBehaviour
{
    public static event Action PlayerDead = null;

    AudioSource audioSource;
    public AudioClip deathSound;

    public float moveTweenTime = 1f;
    public float shakeStrength = 0.4f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerDead();
            _AM.PlaySound(deathSound, audioSource);
            ShakeCamera();
        }
    }

    void ShakeCamera()
    {
        Camera.main.DOShakePosition(moveTweenTime / 2, shakeStrength);
    }
}
