using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pong : Game
{
    private enum PongState
    {
        Idle,
        Game
    }

    public Player player;
    public Ball ball;
    public PongAI pongAI;
    private PongState state;
    private Vector3 ballVelocity;
    private float currentBallSpeed;
    private float minBallSpeed, maxBallSpeed;
    float elapsedTime = 0;
    float timePerStage;
    float gameDuration;

    #region  Unity Lifecycle

    private void OnEnable()
    {
        //pongAI.OnAllBricksSpawned += FinishGame;
    }

    private void OnDisable()
    {
        //pongAI.OnAllBricksSpawned -= FinishGame;
    }

    private void Start()
    {
        currentStage = 0;
        timePerStage = gameDuration / stages;
        elapsedTime = 0;
    }

    private void FixedUpdate()
    {
        if (!GameManager.GamePlaying) return;

        ball.Rigidbody.velocity = Vector3.zero;
        var movement = ballVelocity * Time.deltaTime;
        var position = ball.Rigidbody.position + movement;
        ball.Rigidbody.MovePosition(position);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= gameDuration) FinishGame();

        if (GameManager.GamePlaying)
        {
            UpdateBallSpeed();
        }

        if (elapsedTime > timePerStage * (currentStage + 1))
        {
            StageUpdate();
        }

        if (GameManager.Instance.Mode == Mode.Debug && Input.GetKeyDown(KeyCode.N))
            FinishGame();
    }

    #endregion

    protected override void SetupGame()
    {
        ball.onBallCollision += OnBallCollide;
        //ball.onBallCollision += pongAI.OnBallCollision;

        player.controller = new PongController();
        player.position = Settings.Instance.pongPlayerInitialPosition;
        ball.Rigidbody.position = Settings.Instance.pongBallInitialPosition;

        currentStage = 0;
        timePerStage = gameDuration / stages;
        elapsedTime = 0;
        gameDuration = Settings.Instance.pongGameDuration;
        minBallSpeed = Settings.Instance.pongBallMinMaxSpeed.x;
        maxBallSpeed = Settings.Instance.pongBallMinMaxSpeed.y;
        currentBallSpeed = minBallSpeed;
        SetBallDirection(Vector3.left + Vector3.forward);
        pongAI.SetTarget(ball.transform);
        GameManager.GamePlaying = false;
    }

    protected override void OnStartGame()
    {
        StartCoroutine(StartGameTimer());
    }
    bool firstPlayFlag = true;
    private IEnumerator StartGameTimer()
    {
        // Wait for player to press Start on main menu
        if (firstPlayFlag)
        {
            yield return new WaitWhile(() => !Input.GetKeyDown(KeyCode.Space) && !Input.GetKeyDown(KeyCode.Return));
            firstPlayFlag = false;
        }

        UIManager.Instance.ShowCountdown(true);

        // Wait 3 seconds (call some animation that shows this)
        yield return new WaitForSeconds(3);

        // Start the game
        GameManager.GamePlaying = true;
    }

    protected override void OnRestartGame()
    {
        player.position = Settings.Instance.pongPlayerInitialPosition;
        ball.Rigidbody.position = Settings.Instance.pongBallInitialPosition;
        SetBallDirection(Vector3.left + Vector3.forward);
        GameManager.GamePlaying = false;
        UIManager.Instance.ShowCountdown(true);
        OnStartGame();
    }

    #region Game State
    protected override void OnFinishGame()
    {
        GameManager.GamePlaying = false;
        Whiteboard.instance.pong_BrickPos = pongAI.transform.position;
        Whiteboard.instance.pong_BallDirection = ballVelocity.normalized;
        Whiteboard.instance.pong_BallPosition = ball.transform.position;
        Whiteboard.instance.pong_PlayerPosition = player.position;
    }

    #endregion

    public void UpdateBallSpeed()
    {
        float percentTime = elapsedTime / gameDuration;
        currentBallSpeed = Mathf.Lerp(minBallSpeed, maxBallSpeed, percentTime);
    }
    public void SetBallDirection(Vector3 direction)
    {
        direction.y = 0;
        direction.Normalize();
        ballVelocity = direction * currentBallSpeed;
    }

    private void OnBallCollide(Collision other)
    {
        // Lose condition
        if (other.gameObject.CompareTag("Bounds"))
        {
            Debug.Log("Should lose game");
            RestartGame();
            return;
        }

        var reflectDirection = Vector3.Reflect(ballVelocity, other.contacts[0].normal);
        var ySpeed = other.rigidbody.velocity.z;

        reflectDirection.z += ySpeed;
        SetBallDirection(reflectDirection);
    }

}