using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings Instance { get; private set; }

    public Vector3 pongPlayerInitialPosition;
    public float pongPlayerMovementMinMaxHeight;
    public float pongPlayerMovementSpeed;

    private void Awake()
    {
        if(Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}