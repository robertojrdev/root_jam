using UnityEngine;

public class PongController : IController
{
    public void ApplyMovement(Player player)
    {

        var pos = player.position;
        var movement = GameManager.Instance.settings.pongPlayerMovementSpeed * Time.deltaTime;
        var direction = Input.GetAxisRaw("Vertical");
        pos += movement * direction * Vector3.forward;
        pos.z = Mathf.Clamp(pos.z, -GameManager.Instance.settings.pongPlayerMovementMinMaxHeight, GameManager.Instance.settings.pongPlayerMovementMinMaxHeight);
        player.position = pos;
    }
}