using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteRunner : MonoBehaviour
{
    [Header("Gameplay")]
    public Animator animator;
    [Range(0,1)]
    public float debugSkipTime = 0.5f;
    public Transform[] tracks;
    public WalkerRunnerManager wrManager;

    [Header("Audio")]
    public AudioSource songAudioSorce;
    private float totalDuration = 69.3f;

    void StartLevel()
    {
        songAudioSorce.Play();
        animator.Play("runner_level_anim", 0, 0f);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
            StartLevel();
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            animator.Play("runner_level_anim", 0, debugSkipTime);
            songAudioSorce.Stop();
            songAudioSorce.time = debugSkipTime * totalDuration;
            songAudioSorce.Play();
        }
    }

    void TransitionToWalker()
    {
        wrManager.StartWalker();
    }
}
