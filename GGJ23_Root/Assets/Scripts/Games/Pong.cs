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

    public Vector3 ballInitialPosition;
    public float ballSpeed;
    public Player player;
    public Ball ball;
    public PongAI pongAI;
    private PongState state;
    private Vector3 ballVelocity;

    #region  Unity Lifecycle
    private void Awake()
    {
        ball.onBallCollision += OnBallCollide;
        ball.onBallCollision += pongAI.SpawnBrickOnCollision;
    }

    private void OnEnable()
    {
        pongAI.OnAllBricksSpawned += FinishGame;
    }

    private void OnDisable()
    {
        pongAI.OnAllBricksSpawned -= FinishGame;
    }

    protected override void OnStartGame(Game previousGame)
    {
        // Initialize objects
        Initialize();
        // Make sure game doesn't start immediately
        // GameManager.GamePlaying = false;
        GameManager.GamePlaying = true;

        //UIManager.Instance.ShowCountdown(true);

        // StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        // Wait 3 seconds (call some animation that shows this)
        yield return new WaitForSeconds(3);

        // Start the game
        GameManager.GamePlaying = true;
    }

    private void FixedUpdate()
    {
        if (!GameManager.GamePlaying) return;

        ball.Rigidbody.velocity = Vector3.zero;
        var movement = ballVelocity * Time.deltaTime;
        var position = ball.Rigidbody.position + movement;
        ball.Rigidbody.MovePosition(position);
    }

    #endregion

    #region Game State
    protected override void OnFinishGame()
    {
        GameManager.GamePlaying = false;
        Whiteboard.instance.pong_BrickPos = pongAI.transform.position;
    }

    #endregion

    public void Initialize()
    {
        player.controller = new PongController();
        player.position = Settings.Instance.pongPlayerInitialPosition;
        ball.Rigidbody.position = ballInitialPosition;
        pongAI.SetTarget(ball.transform);
        SetBallDirection(Vector3.left + Vector3.forward);
    }

    public void SetBallDirection(Vector3 direction)
    {
        direction.y = 0;
        direction.Normalize();
        ballVelocity = direction * ballSpeed;
    }

    private void OnBallCollide(Collision other)
    {
        var reflectDirection = Vector3.Reflect(ballVelocity, other.contacts[0].normal);
        var ySpeed = other.rigidbody.velocity.z;

        reflectDirection.z += ySpeed;
        SetBallDirection(reflectDirection);
    }
}