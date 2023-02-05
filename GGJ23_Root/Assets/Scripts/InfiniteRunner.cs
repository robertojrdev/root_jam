using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteRunner : Game
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

    public GameObject brickPrefab;

    protected override void SetupGame()
    {
        // setup da transicao
        // pegar valores instanciar etc
        foreach(Vector3 brickPos in Whiteboard.instance.brickPositions)
            Instantiate(brickPrefab, brickPos, Quaternion.identity);
    }

    protected override void OnStartGame()
    {
        // inicia transicao
        // call start level no fim da transicao
    }

    protected override void OnFinishGame()
    {
        // passar info po proximo jogo
        // tipo dar skip ao menu na main scene
    }

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
}
