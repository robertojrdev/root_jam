using UnityEngine;

public abstract class Game
{
    public abstract void StartGame(Game previousGame);
    public abstract void Update();
    public abstract void OnFinishGame();

    public void FinishGame()
    {
        OnFinishGame();
    }
}