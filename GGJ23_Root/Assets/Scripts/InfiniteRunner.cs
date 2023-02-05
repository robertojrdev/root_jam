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
    public Transform[] initialBrickPositions = new Transform[6]; // Isto tem de ser 6
    public Transform[] startBricks = new Transform[6];
    
    public float transitionDelay = 5f;
    public float brickTransitionSpeed = 4f;
    public float cameraTransitionSpeed = 4f;
    public Material groundLinesMaterial;
    public Color groundLinesStartColor, groundLinesEndColor;
    private bool transitionBricks = false;
    private bool transitionCamera = false;

    public Camera cam;
    public Transform endTransitionCamTarget;
    public float endTransitionCamFoV;

    [Header("Audio")]
    public AudioSource songAudioSorce;
    private float totalDuration = 69.3f;

    public GameObject brickPrefab;
    private bool startedGame = false;

    protected override void SetupGame()
    {
        startedGame = false;

        // setup da transicao
        // pegar valores instanciar etc
        for (int i = 0; i < Whiteboard.instance.runner_BrickPositions.Length; i++)
            startBricks[i] = Instantiate(brickPrefab, Whiteboard.instance.runner_BrickPositions[i], Quaternion.identity).transform;

        cam.transform.position = new Vector3(0f, 200f, 0f);
        //cam.transform.position = Whiteboard.instance.runner_CameraPos;
        cam.transform.rotation = Quaternion.LookRotation(Vector3.down);
        //cam.transform.rotation = Whiteboard.instance.runner_CameraRot;
        cam.fieldOfView = 2;
        //cam.fieldOfView = Whiteboard.instance.runner_CameraFoV;
        
        groundLinesMaterial.color = groundLinesStartColor;

        OnStartGame();
    }

    protected override void OnStartGame()
    {
        // inicia transicao
        // call start level no fim da transicao
        StartCoroutine(StartTransitionIn(transitionDelay));
    }

    protected override void OnFinishGame()
    {
        // passar info po proximo jogo
        // tipo dar skip ao menu na main scene
    }

    void StartLevel()
    {
        if (startedGame) return;
        startedGame = true;
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

        if (transitionBricks)
        {
            // Transition Bricks
            for (int i = 0; i < startBricks.Length; i++)
            {
                startBricks[i].position = Vector3.Lerp(startBricks[i].position, initialBrickPositions[i].position, brickTransitionSpeed * Time.deltaTime);
                startBricks[i].rotation = Quaternion.Lerp(startBricks[i].rotation, initialBrickPositions[i].rotation, brickTransitionSpeed * Time.deltaTime);
            }
        }

        if (transitionCamera)
        {
            // Transition Camera
            cam.transform.position = Vector3.Lerp(cam.transform.position, endTransitionCamTarget.position, cameraTransitionSpeed * Time.deltaTime);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, endTransitionCamTarget.rotation, cameraTransitionSpeed * 1.2f * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, endTransitionCamFoV, cameraTransitionSpeed * 1.3f * Time.deltaTime);

            // Transition level visuals
            groundLinesMaterial.color = Color.Lerp(groundLinesMaterial.color, groundLinesEndColor, cameraTransitionSpeed * Time.deltaTime);

            if (Mathf.Abs(cam.fieldOfView - endTransitionCamFoV) < 0.01f)
                StartLevel();
        }
    }
    private IEnumerator StartTransitionIn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        transitionBricks = true;
        yield return new WaitForSeconds(1f);
        transitionCamera = true;
    }
    private IEnumerator StartGameIn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        StartLevel();
    }


}
