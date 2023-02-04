using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;

    private void Start()
    {
        player.controller = new PongController();
    }
}