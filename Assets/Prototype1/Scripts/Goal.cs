using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : GameBehaviour
{
    AudioSource audioSource;
    public AudioClip goalSound;

    public string nextLevel;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && nextLevel != null)
        {
            _AM.PlaySound(goalSound, audioSource);
            SceneManager.LoadScene(nextLevel);
        }
    }
}
