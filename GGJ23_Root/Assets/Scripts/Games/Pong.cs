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
    private PongState state;
    private Vector3 ballVelocity;

    private void Awake()
    {
        ball.onBallCollision += OnBallCollide;
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
        SetBallDirection(Vector3.left);
    }

    public void SetBallDirection(Vector3 direction)
    {
        direction.y = 0;
        direction.Normalize();
        ballVelocity = direction * ballSpeed;
    }

    private void OnBallCollide(Collision other)
    {
        var ySpeed = other.rigidbody.velocity.z;
        var ballSpeed = -ballVelocity;

        ballSpeed.z += ySpeed;
        SetBallDirection(ballSpeed);
    }

}