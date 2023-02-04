using UnityEngine;

public class BreakoutGame : Game
{
    public override void StartGame(Game previousGame)
    {
        Settings.Instance.breakoutInitialPlayerPosition.ApplyState(GameManager.Instance.Player.gameObject);
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }

    public override void OnFinishGame()
    {
        throw new System.NotImplementedException();
    }
}