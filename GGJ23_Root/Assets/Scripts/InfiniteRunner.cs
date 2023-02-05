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
    public float brickTransitionDuration = 3f;
    public float cameraTransitionDuration = 2f;
    public Material groundLinesMaterial;
    public Color groundLinesStartColor, groundLinesEndColor;
    private bool transitionBricks = false;
    private float transtionTime = 0f;

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

        for(int i = 0; i < GameManager.Instance.whiteboard.breakout_LastBricksPos.Count; i++)
        {
            startBricks[i] = Instantiate(brickPrefab, GameManager.Instance.whiteboard.breakout_LastBricksPos[i], GameManager.Instance.whiteboard.breakout_LastBricksRot[i]).transform;
        }

        //TODO: ROBERTO SUBSTITUI ESTAS VARIAVEIS pelas que tao comentadas

        cam.transform.position = GameManager.Instance.whiteboard.breakout_CameraPos;
        cam.transform.rotation = GameManager.Instance.whiteboard.breakout_CameraRot;
        cam.fieldOfView = GameManager.Instance.whiteboard.breakout_CameraFoV;
        
        groundLinesMaterial.color = groundLinesStartColor;
    }

    protected override void OnStartGame()
    {
        // inicia transicao
        // call start level no fim da transicao
        print("ONSTARTGAME");   
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
            transtionTime += Time.deltaTime;
            
            // Transition Bricks
            for (int i = 0; i < startBricks.Length; i++)
            {
                startBricks[i].position = Vector3.Lerp(startBricks[i].position, initialBrickPositions[i].position, transtionTime / brickTransitionDuration);
                startBricks[i].rotation = Quaternion.Lerp(startBricks[i].rotation, initialBrickPositions[i].rotation, transtionTime / brickTransitionDuration);
            }
            
            // Transition Camera
            cam.transform.position = Vector3.Lerp(GameManager.Instance.whiteboard.breakout_CameraPos, endTransitionCamTarget.position, transtionTime / cameraTransitionDuration);
            cam.transform.rotation = Quaternion.Slerp(GameManager.Instance.whiteboard.breakout_CameraRot, endTransitionCamTarget.rotation, transtionTime / cameraTransitionDuration);
            cam.fieldOfView = Mathf.Lerp(GameManager.Instance.whiteboard.breakout_CameraFoV, endTransitionCamFoV, transtionTime / cameraTransitionDuration);

            // Transition level visuals
            groundLinesMaterial.color = Color.Lerp(groundLinesMaterial.color, groundLinesEndColor, transtionTime / cameraTransitionDuration);

            if (transtionTime >= cameraTransitionDuration)
                StartLevel();
        }
    }
    private IEnumerator StartTransitionIn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        transitionBricks = true;
    }
    private IEnumerator StartGameIn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        StartLevel();
    }


}
