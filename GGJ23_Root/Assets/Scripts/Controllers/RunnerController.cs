using UnityEngine;

public class RunnerController : IController
{
    public void ApplyMovement(Player player)
    {
        var pos = player.position;
        var movement = Settings.Instance.runnerPlayerMovementSpeed * Time.deltaTime;
        var direction = Input.GetAxisRaw("Horizontal");
        pos += movement * direction * Vector3.right;
        pos.x = Mathf.Clamp(pos.z, -1f, 1f);
        player.position = pos;
    }
}