using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        this.audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.Tag_GerbilFollower))
        {
            GameManager.Instance.Win();
        }
    }
}
