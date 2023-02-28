using System;
using System.Collections;
using System.Collections.Generic;
using Interactable;
using UnityEngine;

public class SecretWall : MonoBehaviour
{
    public List<Transform> shouldBeIgnitedTorches;
    public List<Transform> shouldBePuttedOffTorches;

    private bool ignitedTorchesCorrect = false;
    private bool puttedOffTorchesCorrect = false;

    private Animator animator;
    private AudioSource audioSource;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void CheckAllTorches()
    {
        foreach (Transform t in shouldBeIgnitedTorches)
        {
            if (t.GetComponent<Torch>().isIgnited)
            {
                ignitedTorchesCorrect = true;
            }
            else
            {
                ignitedTorchesCorrect = false;
                return;
            }
        }

        if (ignitedTorchesCorrect == false)
        {
            return;
        }
        
        foreach (Transform t in shouldBePuttedOffTorches)
        {
            if (!t.GetComponent<Torch>().isIgnited)
            {
                puttedOffTorchesCorrect = true;
            }
            else
            {
                puttedOffTorchesCorrect = false;
                return;
            }
        }

        if (puttedOffTorchesCorrect == false)
        {
            return;
        }

        if (ignitedTorchesCorrect != true || puttedOffTorchesCorrect != true) return;
        
        animator.enabled = true;
        audioSource.enabled = true;
    }
}
