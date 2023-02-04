using UnityEngine;

public enum Mode
{
    Debug,
    Release
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Mode Mode = Mode.Release;



    [SerializeField] private Camera camera;
    [SerializeField] private Player player;

    public Player Player { get => player; }
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

    private void Start()
    {
        player.controller = new RunnerController();
    }
}