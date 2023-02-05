using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings Instance { get; private set; }

    public Vector3 pongPlayerInitialPosition;
    public Vector3 pongAIInitialPosition;
    public float pongPlayerMovementMinMaxHeight;
    public float pongPlayerMovementSpeed;
    public Vector3 pongBallInitialPosition;
    // The starting speed and the speed the ball will have once the pong game ends
    public Vector2 pongBallMinMaxSpeed; 
    // Make sure this matches the music
    public float pongGameDuration; 

    public ObjectState breakoutInitialPlayerPosition;

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