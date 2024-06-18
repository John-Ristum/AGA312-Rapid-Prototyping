using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BounceBlock : GameBehaviour
{
    Vector3 originalScale;
    Vector3 scaleTo;

    AudioSource audioSource;
    public AudioClip bounceSound;

    public float bounceStrength = 5;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        originalScale = transform.localScale;
        scaleTo = originalScale * 1.1f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //float formerSpeed = collision.rigidbody.velocity.x / 2;
            //float horizintalInput = Input.GetAxis("Horizontal");
            //collision.rigidbody.velocity = Vector3.zero;
            //collision.rigidbody.AddForce(Vector3.right * formerSpeed * horizintalInput);
            collision.rigidbody.AddForce(Vector3.up * bounceStrength, ForceMode.Impulse);
            _AM.PlaySound(bounceSound, audioSource);

            transform.DOScale(scaleTo, 0.1f).SetEase(Ease.InBounce).OnComplete(() =>
            {
                transform.DOScale(originalScale, 0.3f).SetEase(Ease.OutBounce);
            });
        }
    }
}
