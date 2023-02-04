using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BreakoutGame : MonoBehaviour
{
    public Ball ball;
    public float ballSpeed;
    public List<Transform> bricks;
    public Player player;
    public Vector3 ballInitialPosition;

    private Vector3 ballVelocity;

    private void OnEnable()
    {
        ball.onBallCollision += OnBallCollide;
    }

    private IEnumerator Start()
    {
        // Initialize objects
        Initialize();
        // Make sure game doesn't start immediately
        GameManager.GamePlaying = false;

        //UIManager.Instance.ShowCountdown(true);

        // Wait 3 seconds (call some animation that shows this)
        yield return new WaitForSeconds(3);

        // Start the game
        GameManager.GamePlaying = true;
    }

    public void Initialize()
    {
        player.controller = new BreakoutController();
        player.position = Settings.Instance.pongPlayerInitialPosition;
        ball.Rigidbody.position = ballInitialPosition;
        SetBallDirection(Vector3.left + Vector3.forward);
    }

    public void SetBallDirection(Vector3 direction)
    {
        direction.y = 0;
        direction.Normalize();
        ballVelocity = direction * ballSpeed;
    }

    private void DespawnBrick(Collision other)
    {
        other.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        ball.Rigidbody.velocity = Vector3.zero;
        var movement = ballVelocity * Time.deltaTime;
        var position = ball.Rigidbody.position + movement;
        ball.Rigidbody.MovePosition(position);
    }

    private void OnBallCollide(Collision other)
    {
        var reflectDirection = Vector3.Reflect(ballVelocity, other.contacts[0].normal);
        var ySpeed = other.rigidbody.velocity.z;

        reflectDirection.z += ySpeed;
        SetBallDirection(reflectDirection);

        if (other.transform.CompareTag("Wall")) return;
        if (other.transform.CompareTag("Player")) return;

        DespawnBrick(other);
    }
}