using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Camera camera;
    [SerializeField] private Player player;

    public Player Player { get => player; }
    public Camera Camera { get => camera; }

    private void Awake()
    {
        if(Instance)
        {
            Debug.LogError("Multiple Game Managers");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        player.controller = new PongController();
        player.position = Settings.Instance.pongPlayerInitialPosition;
    }
}