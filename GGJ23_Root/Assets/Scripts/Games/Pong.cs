using System;
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

    private void Start()
    {
        StartGame();
    }

    private void FixedUpdate()
    {
        ball.Rigidbody.velocity = Vector3.zero;
        var movement = ballVelocity * Time.deltaTime;
        var position = ball.Rigidbody.position + movement;
        ball.Rigidbody.MovePosition(position);
    }

    public void StartGame()
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

}