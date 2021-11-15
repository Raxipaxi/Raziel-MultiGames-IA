using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellReseter : MonoBehaviour,IInteractable
{
    private AudioSource _audioSrc;

    private void Awake()
    {
        _audioSrc = GetComponent<AudioSource>();
    }

    public bool OnInteract()
    {
        _audioSrc.Play();
        GameManager.Instance.resetHandler.HandleResetOfLevel();
        return false;
    }

    public bool OnTriggerContinueInteract()
    {
        return false;
    }

    public Canvas InteractCanvas { get; }
}
