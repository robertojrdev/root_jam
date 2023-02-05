using UnityEngine;

public class BreakoutController : IController
{
    public float MinMaxMovement { get; private set; }

    public void SetMaxMovement(float movement) => MinMaxMovement = movement;

    public void ApplyMovement(Player player)
    {
        if (!GameManager.GamePlaying) return;

        var pos = player.position;
        var movement = GameManager.Instance.settings.pongPlayerMovementSpeed * Time.deltaTime;
        var direction = Input.GetAxisRaw("Horizontal");
        pos += movement * direction * Vector3.right;
        pos.x = Mathf.Clamp(pos.x, -MinMaxMovement, MinMaxMovement);
        player.position = pos;
    }
}