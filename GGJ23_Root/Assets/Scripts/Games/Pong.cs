using System;
using System.Collections;
using UnityEngine;

public class Pong : MonoBehaviour
{
    private enum PongState
    {
        Idle,
        Game
    }

    public Vector3 ballInitialPosition;
    public float ballSpeed;
    public Ball ball;
    public PongAI pongAI;
    private PongState state;
    private Vector3 ballVelocity;

    private void Awake()
    {
        ball.onBallCollision += OnBallCollide;
        ball.onBallCollision += pongAI.SpawnBrickOnCollision;
    }

    private void OnEnable()
    {
        pongAI.OnAllBricksSpawned += OnGameEnd;
    }

    private void OnDisable()
    {
        pongAI.OnAllBricksSpawned -= OnGameEnd;
    }

    private IEnumerator Start()
    {
        // Initialize objects
        Initialize();
        // Make sure game doesn't start immediately
        GameManager.GamePlaying = false;

        UIManager.Instance.ShowCountdown(true);

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

    public void Initialize()
    {
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

    private void OnGameEnd()
    {
        GameManager.GamePlaying = false;
        Whiteboard.instance.pong_BrickPos = pongAI.transform.position;
    }

}