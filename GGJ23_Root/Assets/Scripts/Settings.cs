using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings Instance { get; private set; }

    [Header("Pong")]
    public Vector3 pongPlayerInitialPosition;
    public Vector3 pongAIInitialPosition;
    public float pongPlayerMovementMinMaxHeight;
    public float pongPlayerMovementSpeed;
    public Vector3 pongBallInitialPosition;
    // The starting speed and the speed the ball will have once the pong game ends
    public Vector2 pongBallMinMaxSpeed;
    // Make sure this matches the music
    public float pongGameDuration;

    [Header("Breakout")]
    public ObjectState breakoutInitialPlayerPosition;
    public Vector3 breakoutBallInitialPosition;
    public Vector2 breakoutMinMaxTimeToReachMaxSpeed = new Vector2(30, 10);
    public Vector2 breakoutMinMaxSlowestSpeed;
    public Vector2 breakoutMinMaxFastestSpeed;
    public float breakoutPlayerMovementSpeed = 15;

    [Header("Runner")]
    public float runnerPlayerMovementSpeed;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}