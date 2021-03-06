﻿using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private float moveY = -5f;

    [SerializeField]
    private float lerpTime = 1f;

    private CinemachineImpulseSource cinemachineImpulseSource;
    private AudioSource audioSource;

    private void Start()
    {
        this.cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        this.audioSource = GetComponent<AudioSource>();
    }

    public void Open()
    {
        LeanTween.moveY(this.gameObject, this.transform.position.y + this.moveY, this.lerpTime);
        this.cinemachineImpulseSource.GenerateImpulse();
        this.audioSource.Play();
    }
}
