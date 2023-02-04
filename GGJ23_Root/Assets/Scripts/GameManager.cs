using UnityEngine;

public enum Mode
{
    Debug,
    Release
}

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Mode Mode = Mode.Release;
    [SerializeField] private Camera camera;
    public Camera Camera { get => camera; }

    public static bool GamePlaying { get; set; }

    private void Awake()
    {
        if (Instance)
        {
            Debug.LogError("Multiple Game Managers");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}