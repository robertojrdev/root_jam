using UnityEngine;

public class PongController : IController
{
    public void ApplyMovement(Player player)
    {
        var pos = player.position;
        var movement = Settings.Instance.pongPlayerMovementSpeed * Time.deltaTime;
        var direction = Input.GetAxisRaw("Vertical");
        pos += movement * direction * Vector3.forward;
        pos.z = Mathf.Clamp(pos.z, -Settings.Instance.pongPlayerMovementMinMaxHeight, Settings.Instance.pongPlayerMovementMinMaxHeight);
        player.position = pos;
    }
}